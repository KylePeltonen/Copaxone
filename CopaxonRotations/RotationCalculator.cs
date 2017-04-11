using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopaxonRotations
{
    class RotationCalculator
    {
        private Area area;
        private Rotation currentRotation;
        private PriorityQueue<Rotation> options;

        public RotationCalculator(uint Rows, uint Columns)
        {
            area = new Area(Rows, Columns);
            currentRotation = new Rotation(area);
            options = new PriorityQueue<Rotation>();
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
                options.Enqueue((Rotation)currentRotation.Clone());
                //Console.WriteLine(R.ToCSV());
                return;
            }

            uint After = 0;
            if (Free == area.Cells)
            {
                currentRotation.Add(area.AcquireNextCell(ref After));
                ComputeOptions();
            }
            else
            {
                while (Free > 0)
                {
                    Cell c = area.AcquireNextCell(ref After);
                    currentRotation.Add(c);
                    ComputeOptions();
                    area.ReturnCell(c);
                    currentRotation.Remove();
                    Free--;
                }
            }
        }
    }
}
