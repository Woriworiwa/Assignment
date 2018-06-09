
namespace LaboratoryCore
{
    public class Reagent<J>
    {
        public J Data { get; set; }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
