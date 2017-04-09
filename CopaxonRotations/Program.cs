using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopaxonRotations
{
    class Program
    {
        static void Recurse( ref Area A, ref Stack<Cell> s )
        {
            uint Free = A.FreeCells;

            if ( Free == 0 )
            {
                Cell[] x = s.ToArray();

                double D1 = 0;
                for (int i = 0; i < x.Count(); i++)
                {
                    int adjacent = (i + 1) % x.Count();
                    D1 += x[adjacent].Distance(x[i]);
                }

                double D2 = 0;
                for (int i = 0; i < x.Count(); i++)
                {
                    int adjacent = (i + 2) % x.Count();
                    D2 += x[adjacent].Distance(x[i]);
                }

                double D3 = 0;
                for (int i = 0; i < x.Count(); i++)
                {
                    int adjacent = (i + 3) % x.Count();
                    D3 += x[adjacent].Distance(x[i]);
                }

                Console.Write("\"");
                for ( int i = x.Count() - 1; i >= 0; i-- )
                {
                    Console.Write("({0},{1}) ", x[i].Row, x[i].Column);
                }
                Console.WriteLine("\",{0:N3},{1:N3},{2:N3}", D1, D2,D3);
                return;
            }

            uint After = 0;
            if ( Free == A.Cells )
            {
                Cell c = A.AcquireNextCell(ref After);
                s.Push(c);
                Recurse(ref A, ref s);
            }
            else
            {
                while (Free > 0)
                {
                    Cell c = A.AcquireNextCell(ref After);
                    s.Push(c);
                    Recurse(ref A, ref s);
                    A.ReturnCell(c);
                    s.Pop();
                    Free--;
                }
            }
        }

        static void Main(string[] args)
        {
            Area A = new Area(3, 2);
#if false
            Cell Base = A.AcquireCell(1, 1);

            Console.WriteLine("Cell count = {0}", A.Cells);
            uint After = 0;
            while (A.FreeCells > 0)
            {
                Cell c = A.AcquireNextCell(ref After);

                Console.WriteLine("Cell ({0},{1}), Index = {3} -- Distance from (1,1) to ({0},{1}) = {2}", c.Row, c.Column, Base.Distance(c), A.Index(c));
            }

            Console.WriteLine("Returning some cells...");
            A.ReturnCell(A.Cell(2, 2));
            A.ReturnCell(A.Cell(2, 1));

            After = 0;
            while (A.FreeCells > 0)
            {
                Cell c = A.AcquireNextCell(ref After);

                Console.WriteLine("Cell ({0},{1}) -- Distance from (1,1) to ({0},{1}) = {2}", c.Row, c.Column, Base.Distance(c));
            }

            Console.WriteLine("Recursion..."); 
#endif
            Console.WriteLine("\"Order\",D1,D2,D3");
            A = new Area(4, 2);
            Stack<Cell> s = new Stack<Cell>();
            Recurse(ref A, ref s);

#if false
            Console.WriteLine("Press enter to continue...");
            Console.Read(); 
#endif
        }
    }
}
