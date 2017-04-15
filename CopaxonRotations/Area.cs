using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopaxonRotations
{
    class Area
    {
        #region Sizing
        private uint rows;
        private uint columns;

        public uint Rows
        {
            get
            {
                return rows;
            }
        }

        public uint Columns
        {
            get
            {
                return columns;
            }
        }

        public uint Cells
        {
            get
            {
                return rows * columns;
            }
        }
        #endregion

        #region Constructors
        public Area(uint Rows, uint Columns)
        {
            rows = Rows;
            columns = Columns;
            used = new bool[this.Cells];
            usedCount = 0;
        }
        #endregion

        #region Basic Cells
        public Cell Cell(uint Row, uint Column)
        {
            if (Row > rows || Column > columns)
            {
                throw new IndexOutOfRangeException();
            }

            return new Cell(Row, Column);
        }

        public Cell Cell(uint Index)
        {
            if (Index > this.Cells)
            {
                throw new IndexOutOfRangeException();
            }

            uint Row = (Index / columns) + 1;
            uint Column = (Index % columns) + 1;
            return new Cell(Row, Column);
        }

        public uint Index( Cell C )
        {
            return (C.Row - 1) * columns + C.Column - 1; 
        }
        #endregion

        #region Free Cells
        private bool[] used;
        uint usedCount;

        public Cell AcquireNextCell(ref uint i)
        {
            for (; i < this.Cells; i++)
            {
                if (!used[i])
                {
                    used[i] = true;
                    usedCount++;
                    Cell c = Cell(i);
                    i++;
                    return c;
                }
            }

            return new Cell();
        }

        public Cell AcquireCell(uint Row, uint Column)
        {
            if (Row > rows || Column > columns)
            {
                throw new IndexOutOfRangeException();
            }

            uint Index = (Row - 1) * columns + Column - 1;
            if (used[Index])
            {
                // BUGBUG: Not really a good choice...
                throw new IndexOutOfRangeException();
            }

            used[Index] = true;
            usedCount++;
            return Cell(Index);
        }

        public void ReturnCell(Cell C)
        {
            uint Index = (C.Row - 1) * columns + C.Column - 1;

            if (!used[Index])
            {
                // BUGBUG: Not really a good choice...
                throw new IndexOutOfRangeException();
            }

            used[Index] = false;
            usedCount--;
        }

        public uint FreeCells
        {
            get
            {
                return this.Cells - usedCount;
            }
        } 
        #endregion
    }
}
