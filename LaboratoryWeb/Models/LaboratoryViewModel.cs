using LaboratoryCore;
using System.Collections.Generic;

namespace LaboratoryWeb
{
    public class LaboratoryViewModel<T,J>
    {
        public PlateSizes PlateSize { get; set; } = PlateSizes.Small;

        public List<Plate<T,J>> Plates { get; set; }

        public string Samples { get; set; }

        public string Reagents { get; set; }

        public string Repititions { get; set; }

        public string Output { get; set; }

        public string ErrorMessage { get; set; }
    }
}