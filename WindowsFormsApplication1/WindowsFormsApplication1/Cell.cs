using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Cell
    {
        public bool top;
        public bool bottom;
        public bool left;
        public bool right;
        public int x;
        public int y;
        public bool isVisited;
        public bool isInPath;

        public Cell(int x, int y)
        {
            this.top = true;
            this.bottom = true;
            this.left = true;
            this.right = true;
            this.isVisited = false;


            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// je réécris la fonctions ToString() pour lmes test console
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ReturnWallLetter(top, "T") + ReturnWallLetter(right, "R")
             + ReturnWallLetter(bottom, "B") + ReturnWallLetter(left, "L");
        }

        private string ReturnWallLetter(bool Wall, string Letter)
        {
            if (Wall)
            {
                return Letter;
            }

            return "_";
        }
    }
}
