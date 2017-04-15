using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CopaxonRotations
{
    class RotationCalculator
    {
        private Area area;
        private Rotation currentRotation;
        private PriorityQueue<Rotation> options;

        private bool ignoreLowScores;
        double[] highScore;

        private bool ignoreAdjacentCells;

        private static uint topAfter = 0;
        private static object topMutex = topAfter;

        public RotationCalculator(uint Rows, uint Columns, bool IgnoreLowScores = false, bool IgnoreAdjacentCells = false)
        {
            area = new Area(Rows, Columns);
            currentRotation = new Rotation(area);
            options = new PriorityQueue<Rotation>();

            ignoreLowScores = IgnoreLowScores;
            highScore = new[]{ 0.0, 0.0, 0.0 };

            ignoreAdjacentCells = IgnoreAdjacentCells;

            // Optimization: Always acquire the first cell. E.g. only try rotations starting with (1,1).
            uint After = 0;
            currentRotation.Add(area.AcquireNextCell(ref After));
            Debug.Assert(After == 1);
        }

        public PriorityQueue<Rotation> Options
        {
            get
            {
                return options;
            }
        }
            
        public void ComputeOptions()
        {
            uint Free = area.FreeCells;

            if (Free == 0)
            {
                double score = currentRotation.Score();
                if ( !ignoreLowScores || score >= highScore[0] )
                {
                    options.Enqueue((Rotation)currentRotation.Clone());
                }

                if ( score > highScore[0] )
                {
                    highScore[0] = score;
                    Array.Sort(highScore);
                }

                return;
            }

            // Use global tracking at top level, to support threading
            if ( Free == area.Cells - 1 )
            {
                Monitor.Enter(topMutex);
                Cell c = area.AcquireNextCell(ref topAfter);
                Monitor.Exit(topMutex);

                while ( c.IsInitialized() )
                {
                    InnerComputeOptions(c);

                    Monitor.Enter(topMutex);
                    c = area.AcquireNextCell(ref topAfter);
                    Monitor.Exit(topMutex);
                }
            }
            else
            {
                uint After = 1;

                for (Cell c = area.AcquireNextCell(ref After); c.IsInitialized(); c = area.AcquireNextCell(ref After))
                {
                    InnerComputeOptions(c);
                }
            }
        }

        private void InnerComputeOptions( Cell c )
        {
            // Optimization: Early exit for path unlikely to be a high scorer.
            if ( ignoreAdjacentCells && c.Distance(currentRotation.MostRecent()) == 1 )
            {
                area.ReturnCell(c);
                return;
            }

            currentRotation.Add(c);
            ComputeOptions();
            area.ReturnCell(c);
            currentRotation.Remove();
        }
    }
}
