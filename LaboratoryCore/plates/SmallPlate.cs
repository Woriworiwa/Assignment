namespace LaboratoryCore
{
    /// <summary>
    /// A small plate of 8 rows and 12 columns.
    /// </summary>
    public class SmallPlate<T,J>: Plate<T,J>
    {
        /// <summary>
        /// Number of rows in this plate.
        /// </summary>
        public override int Rows
        {
            get
            {
                return 8;
            }
        }
        
        /// <summary>
        /// Number of columns in this plate.
        /// </summary>
        public override int Columns
        {
            get
            {
                return 12;
            }
        }
    }
}
