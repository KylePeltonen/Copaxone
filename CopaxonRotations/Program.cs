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
            uint Rows, Columns;

            if ( !uint.TryParse(args[0],out Rows) || !uint.TryParse(args[1],out Columns) )
            {
                Console.WriteLine("Usage: CopaxoneRotations <Rows> <Columns>");
                return;
            }

            int cCPU = Environment.ProcessorCount;
            RotationCalculator[] R = new RotationCalculator[cCPU];
            Thread[] T = new Thread[cCPU];

            for ( int i = 0; i < cCPU; i++ )
            {
                R[i] = new RotationCalculator(Rows, Columns, (Rows*Columns > 6), (Rows * Columns > 6));
                T[i] = new Thread(new ThreadStart(R[i].ComputeOptions));
            }

            for ( int i = 0; i < cCPU; i++ )
            {
                T[i].Start();
            }

            for (int i = 0; i < cCPU; i++)
            {
                T[i].Join();
            }

            // Just to make things simple, move all rotations to R[0]
            for ( int i = 1; i < cCPU; i++ )
            {
                while ( R[i].Options.Count() > 0 )
                {
                    Rotation current = R[i].Options.Dequeue();
                    R[0].Options.Enqueue(current);
                }
            }

            int total = R[0].Options.Count();
            Console.WriteLine(Rotation.CSVHeader());
            while ( R[0].Options.Count() > 0 )
            {
                Rotation current = R[0].Options.Dequeue();
                Console.WriteLine(current.ToCSV());
            }

            Console.WriteLine("{0} possiblities. Press enter to continue...", total);
        }
    }
}
