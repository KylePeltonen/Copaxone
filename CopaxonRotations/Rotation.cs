using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopaxonRotations
{
    class Rotation : IComparable<Rotation>, ICloneable
    {
        private uint size;
        private Cell[] sequence;
        private int next;

        private Rotation()
        {
        }

        public Rotation( Area A )
        {
            size = A.Cells;
            sequence = new Cell[size];
            next = 0;
        }

        public void Add( Cell C )
        {
            if ( next >= sequence.Length )
            {
                throw new IndexOutOfRangeException();
            }

            sequence[next++] = C;
        }

        public void Remove()
        {
            if ( next == 0 )
            {
                throw new IndexOutOfRangeException();
            }

            next--;
        }

        public Cell MostRecent()
        {
            if ( next == 0 )
            {
                return new Cell();
            }

            return sequence[next - 1];
        }

        public double Distance(int Gap)
        {
            double D1 = 0;
            for (int i = 0; i < next; i++)
            {
                int adjacent = (i - Gap + (int)size) % next;
                D1 += sequence[adjacent].Distance(sequence[i]);
            }

            return D1;
        }

        public double Score()
        {
            double D1 = Distance(1);
            double D2 = Distance(2);
            double D3 = Distance(3);

            return D1 + D2 * 0.5 + D3 * 0.25;
        }

        public static string CSVHeader()
        {
            return "\"Sequence\",D1,D2,D3,Score";
        }

        public string ToCSV()
        {
            string s = "\"";
            if (next > 1)
            {
                for (int i = 0; i < (next - 1); i++)
                {
                    s += string.Format("({0},{1}), ", sequence[i].Row, sequence[i].Column);
                }
            }

            if ( next > 0 )
            {
                s += string.Format("({0},{1})", sequence[next-1].Row, sequence[next-1].Column);
            }

            double D1 = Distance(1);
            double D2 = Distance(2);
            double D3 = Distance(3);
            double S = Score();

            s += string.Format("\",{0:N3},{1:N3},{2:N3},{3:N3}", D1, D2, D3, S);

            return s;
        }

        public override string ToString()
        {
            return ToCSV();
        }

        #region IComparable
        public int CompareTo(Rotation other)
        {
            double s1 = this.Score();
            double s2 = other.Score();

            // Small scores are worse, so put them at the bottom of the heap (e.g. smaller == 1)
            if (s1 < s2)
                return 1;
            else if (s1 > s2)
                return -1;
            else
                return 0;
        } 
        #endregion

        public object Clone()
        {
            Rotation clone = new Rotation();

            clone.size = this.size;
            clone.sequence = (Cell[])this.sequence.Clone();
            clone.next = this.next;

            return clone;
    }
}
}
