using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CopaxonRotations
{
    class Program
    {
        static void Main(string[] args)
        {
#if false
            Area A = new Area(3, 2);
            Cell Base = A.AcquireCell(1, 1);
            Rotation R = new Rotation(A);
            R.Add(Base);

            Console.WriteLine("Cell count = {0}", A.Cells);
            uint After = 0;
            while (A.FreeCells > 0)
            {
                Cell c = A.AcquireNextCell(ref After);
                R.Add(c);

                Console.WriteLine("Cell {0}, Index = {1} -- Distance from (1,1) to {0} = {2}", c.ToString(), Base.Distance(c), A.Index(c));
            }

            Console.WriteLine("Returning some cells...");
            A.ReturnCell(A.Cell(2, 2));
            A.ReturnCell(A.Cell(2, 1));

            After = 0;
            while (A.FreeCells > 0)
            {
                Cell c = A.AcquireNextCell(ref After);

                Console.WriteLine("Cell {0} -- Distance from (1,1) to {0} = {1}", c.ToString(), Base.Distance(c));
            }

            Console.WriteLine("Rotation...");
            Rotation R2 = (Rotation)R.Clone();
            R2.Remove();
            Console.WriteLine(R.ToCSV());
            Console.WriteLine(R2.ToCSV());

            Console.WriteLine("Press enter to continue...");
            Console.Read();
#endif
            RotationCalculator R = new RotationCalculator(3, 2);
            Thread t = new Thread(new ThreadStart(R.ComputeOptions));
            t.Start();
            t.Join();

            // Only return the first three [unique] scores
            double currentScore = -1;
            int countScores = 0;

            while (R.Options.Count() > 0 && countScores < 4)
            {
                Rotation current = R.Options.Dequeue();
                if (current.Score() != currentScore)
                {
                    currentScore = current.Score();
                    countScores++;
                }

                Console.WriteLine(current.ToCSV());
            }
        }
    }
}
