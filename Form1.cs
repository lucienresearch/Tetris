using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Tetris
{
    
    public partial class Form1 : Form
    {
       
        List<RankItem> rankList = new List<RankItem>();

        public Form1()
        {
            InitializeComponent();
            //Task.Run(() =>
            //{
            //    SoundPlayer sp = new SoundPlayer(Assembly.GetExecutingAssembly().GetManifestResourceStream("Tetris.Properties.Resources.backmusic.wav"));
            //    sp.PlayLooping();
            //});
            timer1.Interval = 500;

            LoadRankList();

            // 设置 ListBox 的数据源为 rankList
            listBox1.DataSource = rankList;
            // 设置 ListBox 的显示成员为 RankItem.ToString() 方法返回的字符串
            listBox1.DisplayMember = "ToString";
            // 设置 ListBox 的 DrawMode 属性为 OwnerDrawFixed
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            // 设置 ListBox 的 ItemHeight 属性为 30，调整项的高度
            listBox1.ItemHeight = 30;

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

                // 弹出输入框，让用户输入名字
                string name = InputBox.Show("游戏结束，您的得分是：" + game.score + "，请输入您的名字：");
                if (!string.IsNullOrEmpty(name))
                {
                    // 将分数和名字保存到排行榜中
                    rankList.Add(new RankItem { Name = name, Score = game.score });
                    rankList = rankList.OrderByDescending(i => i.Score).ToList();

                    // 如果排行榜超过了10个，删除最后一个
                    if (rankList.Count > 10)
                    {
                        rankList.RemoveAt(rankList.Count - 1);
                    }

                    // 更新 ListBox 的数据源
                    listBox1.DataSource = null;
                    listBox1.DataSource = rankList;

                    // 保存排行榜数据到文件中
                    SaveRankList();
                }

                // 显示游戏结束提示，重新启用开始游戏按钮
                MessageBox.Show("游戏结束", "提示");
                button1.Enabled = true;
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // 绘制项的背景色
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            }

            // 绘制分割线和文本
            if (e.Index >= 0 && e.Index < listBox1.Items.Count)
            {
                RankItem item = (RankItem)listBox1.Items[e.Index];
                int nameWidth = e.Bounds.Width / 2 - 10;
                int scoreWidth = e.Bounds.Width / 2 - 10;
                Rectangle nameRect = new Rectangle(e.Bounds.Left + 5, e.Bounds.Top + 5, nameWidth, e.Bounds.Height - 10);
                Rectangle scoreRect = new Rectangle(e.Bounds.Left + nameWidth + 10, e.Bounds.Top + 5, scoreWidth, e.Bounds.Height - 10);

                // 绘制竖直分割线
                int lineX = nameRect.Right + 5;
                int lineY1 = e.Bounds.Top + 5;
                int lineY2 = e.Bounds.Bottom - 5;
                e.Graphics.DrawLine(Pens.Gray, lineX, lineY1, lineX, lineY2);

                // 绘制姓名和分数
                TextRenderer.DrawText(e.Graphics, item.Name, listBox1.Font, nameRect, Color.Black, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(e.Graphics, item.Score.ToString(), listBox1.Font, scoreRect, Color.Black, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
            }
        }


        private void SaveRankList()
        {
            string json = JsonConvert.SerializeObject(rankList);
            File.WriteAllText("ranklist.json", json);
        }

        private void LoadRankList()
        {
            try
            {
                if (File.Exists("ranklist.json"))
                {
                    string json = File.ReadAllText("ranklist.json");
                    rankList = JsonConvert.DeserializeObject<List<RankItem>>(json);
                    rankList = rankList.OrderByDescending(i => i.Score).ToList();
                    // 更新 ListBox 的数据源
                    listBox1.DataSource = null;
                    listBox1.DataSource = rankList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取排行榜数据失败：" + ex.Message);
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


        public class RankItem
        {
            public string Name { get; set; }
            public int Score { get; set; }

            public override string ToString()
            {
                return string.Format("{0,-10}{1}", Name, Score);
            }
        }

        public class InputBox : Form
        {
            private TextBox textBox1;
            private Button button1;
            private Button button2;

            public InputBox()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.textBox1 = new System.Windows.Forms.TextBox();
                this.button1 = new System.Windows.Forms.Button();
                this.button2 = new System.Windows.Forms.Button();
                this.SuspendLayout();
                // 
                // textBox1
                // 
                this.textBox1.Location = new System.Drawing.Point(12, 12);
                this.textBox1.Name = "textBox1";
                this.textBox1.Size = new System.Drawing.Size(260, 21);
                this.textBox1.TabIndex = 0;
                // 
                // button1
                // 
                this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.button1.Location = new System.Drawing.Point(116, 39);
                this.button1.Name = "button1";
                this.button1.Size = new System.Drawing.Size(75, 23);
                this.button1.TabIndex = 1;
                this.button1.Text = "确定";
                this.button1.UseVisualStyleBackColor = true;
                // 
                // button2
                // 
                this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.button2.Location = new System.Drawing.Point(197, 39);
                this.button2.Name = "button2";
                this.button2.Size = new System.Drawing.Size(75, 23);
                this.button2.TabIndex = 2;
                this.button2.Text = "取消";
                this.button2.UseVisualStyleBackColor = true;
                // 
                // InputBox
                // 
                this.AcceptButton = this.button1;
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.CancelButton = this.button2;
                this.ClientSize = new System.Drawing.Size(284, 74);
                this.Controls.Add(this.button2);
                this.Controls.Add(this.button1);
                this.Controls.Add(this.textBox1);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "InputBox";
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                this.Text = "请输入名字";
                this.ResumeLayout(false);
                this.PerformLayout();

            }

            public static string Show(string prompt)
            {
                using (InputBox inputBox = new InputBox())
                {
                    inputBox.Text = prompt;
                    if (inputBox.ShowDialog() == DialogResult.OK)
                    {
                        return inputBox.textBox1.Text;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
