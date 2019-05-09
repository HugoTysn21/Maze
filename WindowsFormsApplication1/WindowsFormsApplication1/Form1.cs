using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphicsObj = this.CreateGraphics();

            int WallWeight = 26;
            int SemiWallWeight = 26 / 2;

            Pen myPen = new Pen(System.Drawing.Color.Red, 2);
            Pen pathPen = new Pen(System.Drawing.Color.Blue, 4);
            Pen startPen = new Pen(System.Drawing.Color.Yellow, 4);
            Pen endPen = new Pen(System.Drawing.Color.Green, 4);

            int size = 15;
            Maze laby = new Maze(size);
            Cell[,] CellTable = laby.Table;

           // for( int x = 0, i = )

            foreach (Cell cell in CellTable)
            {
                Corner TopLeftCorner;
                Corner BottomLeftCorner;
                Corner TopRightCorner;
                Corner BottomRightCorner;

                Console.WriteLine($"Cell {cell.x} {cell.y} : {cell}");

                TopLeftCorner = new Corner((cell.x - 1) * WallWeight, (cell.y - 1) * WallWeight);
                BottomLeftCorner = new Corner((cell.x - 1) * WallWeight, cell.y * WallWeight);
                TopRightCorner = new Corner(cell.x * WallWeight, (cell.y - 1) * WallWeight);
                BottomRightCorner = new Corner(cell.x  * WallWeight, cell.y * WallWeight);

                if (cell.isInPath)
                {
                    graphicsObj.DrawRectangle(pathPen, TopLeftCorner.X + 10, TopLeftCorner.Y + 10, 5, 5);
                }

                if (cell.top)
                {
                    graphicsObj.DrawLine(myPen, TopLeftCorner.X, TopLeftCorner.Y, TopRightCorner.X, TopRightCorner.Y);
                }

                if (cell.left)
                {
                    graphicsObj.DrawLine(myPen, TopLeftCorner.X, TopLeftCorner.Y, BottomLeftCorner.X, BottomLeftCorner.Y);
                }

                if (cell.right)
                {
                    graphicsObj.DrawLine(myPen, BottomRightCorner.X, BottomRightCorner.Y, TopRightCorner.X, TopRightCorner.Y);
                }

                if (cell.bottom)
                {
                    graphicsObj.DrawLine(myPen, BottomRightCorner.X, BottomRightCorner.Y, BottomLeftCorner.X, BottomLeftCorner.Y);
                }
            }

            
            graphicsObj.DrawRectangle(startPen, (laby.startCell.x - 1) * WallWeight + 50, (laby.startCell.y - 1) * WallWeight + 50, 5, 5);
            graphicsObj.DrawRectangle(endPen, (laby.endCell.x - 1) * WallWeight + 50, (laby.endCell.y - 1) * WallWeight + 50, 5, 5);

        }

        public class Corner
        {
            public int X { get; }
            public int Y { get; }

            public Corner(int _X, int _Y)
            {
                X = _X + 40;
                Y = _Y + 40;
            }
        }
    }
}
