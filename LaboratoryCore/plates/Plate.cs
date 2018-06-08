namespace LaboratoryCore
{
    public abstract class Plate
    {
        private int rowNumber = -1;
        private int colNumber = -1;
        Well[,] wells;

        /// <summary>
        /// Number of rows in this plate. 
        /// </summary>
        public virtual int Rows
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Number of columns in this plate.
        /// </summary>
        public virtual int Columns
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// The wells in this plate.
        /// </summary>
        public Well[,] Wells
        {
            get
            {
                return this.wells;
            }
        }

        /// <summary>
        /// Creates a new instsnce.
        /// </summary>
        public Plate()
        {
            wells = new Well[Rows, Columns];
        }

        /// <summary>
        /// Creates a plate;
        /// </summary>
        public static Plate CreatePlate(PlateSizes plateSize)
        {
            switch (plateSize)
            {
                case PlateSizes.Small:
                    return new SmallPlate();
                case PlateSizes.Big:
                    return new BigPlate();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Fills the well.
        /// </summary>       
        public void FillWell(Experiment experiment, Sample sample, Reagent reagent)
        {
            wells[++rowNumber, colNumber] = new Well(experiment, sample, reagent);
        }

        /// <summary>
        /// Moves the index to start filling the next column.
        /// </summary>
        public void MoveToNextColumn()
        {
            colNumber++;
            rowNumber = -1;
        }
        
        /// <summary>
        /// Checks if there is any empty columns left in this plate.
        /// </summary>
        public bool IsFull()
        {
            return colNumber == Columns;
        }
    }
}
