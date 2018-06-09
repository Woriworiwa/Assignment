
namespace LaboratoryCore
{
    public class Sample<T>
    {
        public T Data { get; set; }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
