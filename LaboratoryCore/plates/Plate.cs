namespace LaboratoryCore
{
    public abstract class Plate<T,J>
    {
        private int rowNumber = -1;
        private int colNumber = -1;
        Well<T,J>[,] wells;

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
        public Well<T,J>[,] Wells
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
            wells = new Well<T,J>[Rows, Columns];
        }

        /// <summary>
        /// Creates a plate;
        /// </summary>
        public static Plate<T,J> CreatePlate(PlateSizes plateSize)
        {
            switch (plateSize)
            {
                case PlateSizes.Small:
                    return new SmallPlate<T,J>();
                case PlateSizes.Big:
                    return new BigPlate<T,J>();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Fills the well.
        /// </summary>       
        public void FillWell(Experiment<T,J> experiment, Sample<T> sample, Reagent<J> reagent)
        {
            wells[++rowNumber, colNumber] = new Well<T,J>(experiment, sample, reagent);
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
