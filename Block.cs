using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Block
    {
        private short width;//方块的高度
        private short height;//方块的宽度
        private short top;//方块位置的纵坐标
        private short left;//方块位置的横坐标
        private int ID;    　　　//方块部件的ID
        public int[,] shape;  //存储方块部件的形状，０为空白，１为有砖块
        public short Width
        {
            get { return width; }
            set { width = value; }
        }
        public short Height
        {
            get { return height; }
            set { height = value; }
        }
        public short Top
        {
            get { return top; }
            set { top = value; }
        }

        public short Left
        {
            get { return left; }
            set { left = value; }
        }
        public Block()
        {
            Random randomGenerator = new Random();
            int randomBlock = randomGenerator.Next(1, 9);//产生1—7的数
            this.ID = randomBlock;
            switch (this.ID)
            {
                case 1:   //正方形
                    this.Width = 2;
                    this.Height = 2;
                    this.Top = 0;
                    this.Left = 4;
                    shape = new int[this.Width, this.Height];
                    shape[0, 0] = 1;
                    shape[0, 1] = 1;
                    shape[1, 0] = 1;
                    shape[1, 1] = 1;
                    break;
                case 2:　 //横形
                    this.Width = 4;
                    this.Height = 1;
                    this.Top = 0;
                    this.Left = 3;
                    shape = new int[this.Width, this.Height];
                    shape[0, 0] = 1;
                    shape[1, 0] = 1;
                    shape[2, 0] = 1;
                    shape[3, 0] = 1;
                    break;
                case 3:　 //山形
                    this.Width = 3;
                    this.Height = 2;
                    this.Top = 0;
                    this.Left = 4;
                    shape = new int[this.Width, this.Height];
                    shape[0, 1] = 1;
                    shape[1, 0] = 1;
                    shape[1, 1] = 1;
                    shape[2, 1] = 1;
                    break;
                case 4:　 //之形
                    this.Width = 3;
                    this.Height = 2;
                    this.Top = 0;
                    this.Left = 4;
                    shape = new int[this.Width, this.Height];
                    shape[0, 1] = 1;
                    shape[1, 0] = 1;
                    shape[1, 1] = 1;
                    shape[2, 0] = 1;
                    break;
                case 5:　 //之形
                    this.Width = 3;
                    this.Height = 2;
                    this.Top = 0;
                    this.Left = 4;
                    shape = new int[this.Width, this.Height];
                    shape[0, 0] = 1;
                    shape[1, 0] = 1;
                    shape[1, 1] = 1;
                    shape[2, 1] = 1;
                    break;
                case 6:　 //L形
                    this.Width = 3;
                    this.Height = 2;
                    this.Top = 0;
                    this.Left = 4;
                    shape = new int[this.Width, this.Height];
                    shape[0, 1] = 1;
                    shape[1, 1] = 1;
                    shape[2, 0] = 1;
                    shape[2, 1] = 1;
                    break;
                case 7:　 //L形
                    this.Width = 3;
                    this.Height = 2;
                    this.Top = 0;
                    this.Left = 4;
                    shape = new int[this.Width, this.Height];
                    shape[0, 0] = 1;
                    shape[0, 1] = 1;
                    shape[1, 1] = 1;
                    shape[2, 1] = 1;
                    break;
                case 8: //小方块
                    this.Width = 1;
                    this.Height = 1;
                    this.Top = 0;
                    this.Left = 4;
                    shape = new int[this.Width, this.Height];
                    shape[0, 0] = 1;
                    break;
            }
        }
        public void Draw(Graphics g)//随机生成方块类型的编号(this.ID),该构造函数中Switch/Case语句段根据类型编号this.ID生成相应方块的形状
        {
            ResourceManager resourceManage = new ResourceManager("Tetris.Properties.Resources", Assembly.GetExecutingAssembly());
            Image brickImage = null;
            switch (this.ID)
            {
                case 1:   //正方形
                    brickImage = (Image)resourceManage.GetObject("red");
                    break;
                case 2:　 //横形
                    brickImage = (Image)resourceManage.GetObject("blue");
                    break;
                case 3:　 //山形
                    brickImage = (Image)resourceManage.GetObject("yellow");
                    break;
                case 4:　 //之形
                    brickImage = (Image)resourceManage.GetObject("pink");
                    break;
                case 5:　 //之形
                    brickImage = (Image)resourceManage.GetObject("orange");
                    break;
                case 6:　 //L形
                    brickImage = (Image)resourceManage.GetObject("darkblue");
                    break;
                case 7:　 //L形
                    brickImage = (Image)resourceManage.GetObject("purple");
                    break;
                case 8: //小方块
                    brickImage = (Image)resourceManage.GetObject("green");
                    break;
            }

            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Height; j++)
                {
                    if (this.shape[i, j] == 1)//黑色格子
                    {
                        //得到绘制这个格子的在游戏面板中的矩形区域
                        Rectangle rect = new Rectangle((this.Left + i) * Game.BlockImageWidth, (this.Top + j) * Game.BlockImageHeight, Game.BlockImageWidth, Game.BlockImageHeight);
                        g.DrawImage(brickImage, rect);
                    }
                }
            }
        }
    }
}
