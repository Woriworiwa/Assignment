namespace LaboratoryCore
{
    /// <summary>
    /// A big plate of 16 rows and 24 columns.
    /// </summary>
    public class BigPlate: Plate
    {
        /// <summary>
        /// Number of rows in this plate.
        /// </summary>
        public override int Rows
        {
            get
            {
                return 16;
            }
        }

        /// <summary>
        /// Number of columns in this plate.
        /// </summary>
        public override int Columns
        {
            get
            {
                return 24;
            }
        }
    }
}
