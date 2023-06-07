using System;
using System.Windows.Forms;

namespace Tetris
{
    
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            //Task.Run(() =>
            //{
            //    SoundPlayer sp = new SoundPlayer(Assembly.GetExecutingAssembly().GetManifestResourceStream("Tetris.Properties.Resources.backmusic.wav"));
            //    sp.PlayLooping();
            //});
            timer1.Interval = 500;

        }
        Game game = null;

        private void button1_Click(object sender, EventArgs e)//开始游戏，启动定时器

        {
            game = new Game();
            pictureBox1.Height = Game.BlockImageHeight * Game.PlayingFieldHeight + 3;
            pictureBox1.Width = Game.BlockImageWidth * Game.PlayingFieldWidth + 3;
            pictureBox1.Invalidate();//重画游戏面板区域
            timer1.Enabled = true;
            button1.Enabled = false;
            //timer1.Interval = 500;
        }

        private void button2_Click(object sender, EventArgs e)//暂停游戏
        {
            if (button2.Text == "暂停游戏")
            {
                timer1.Enabled = false; button2.Text = "继续游戏";
            }
            else
            {
                timer1.Enabled = true; button2.Text = "暂停游戏";
            }
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)//刷新游戏面板屏幕，重画当前下落方块和pile存储的固定方块
        {
            //重画游戏面板
            if (game != null)
            {
                game.DrawPile(e.Graphics);
                game.DrawCurrentBlock(e.Graphics);
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)//重画下一个方块
        {
            if (game != null) game.DrawNextBlock(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!game.DownCurrentBlock())
            {
                pictureBox1.Invalidate();//重画游戏面板区域
                pictureBox2.Invalidate();//重画下一个方块
            }
            textBox1.Text = game.score.ToString();
            if (game.over == true)
            {
                timer1.Enabled = false;
                MessageBox.Show("游戏结束", "提示");
                button1.Enabled = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys e)
        //重写ProcessCmdKey方法
        {
            if (button2.Text == "继续游戏") return true;//暂停时不响应键盘
            if (e == Keys.Up || e == Keys.Down || e == Keys.Space ||
                     e == Keys.Left || e == Keys.Right)
            {
                MyKeyPress(this, new KeyPressEventArgs((char)e)); //然后在MyKeyPress方法中处理   
            }
            return true;
        }

        private void MyKeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Up:
                    game.RotateCurrentBlock();
                    break;
                case (char)Keys.Down:
                    if (!game.DownCurrentBlock())
                        pictureBox1.Invalidate();//重画游戏面板区域
                    break;
                case (char)Keys.Right:
                    game.MoveCurrentBlockSide(false);
                    break;
                case (char)Keys.Left:
                    game.MoveCurrentBlockSide(true);
                    break;
                case (char)Keys.Space:
                    button2.PerformClick();
                    break;
            }
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }


        private void 简单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Interval = 550 - 1 * 50;
        }

        private void 一般ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Interval = 550 - 4 * 50;
        }

        private void 困难ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Interval = 550 - 7 * 50;
        }

        private void 地狱ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Interval = 550 - 10 * 50;
        }
    }
}
