using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopaxonRotations
{
    class Cell
    {
        private uint row;
        private uint column;

        public uint Row
        {
            get
            {
                return row;
            }
        }

        public uint Column
        {
            get
            {
                return column;
            }
        }

        public Cell( uint Row, uint Column )
        {
            row = Row;
            column = Column;
        }
            
        public double Distance( Cell Dst )
        {
            return Distance(this, Dst);
        }

        public static double Distance( Cell Src, Cell Dst )
        {
            return Math.Sqrt(Math.Pow((double)Src.row - (double)Dst.row, 2) + Math.Pow((double)Src.column - (double)Dst.column, 2));
        }
    }
}
