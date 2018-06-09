using Newtonsoft.Json;
using System.Collections.Generic;

namespace LaboratoryCore
{
    /// <summary>
    /// An experiment is a mix of small volume of sample with a certain number of reagents.
    /// </summary>
    public class Experiment<T,J>
    {
        public int Id { get; set; }

        /// <summary>
        /// Samples that will be used in this experiment.
        /// </summary>        
        public List<Sample<T>> Samples { get; set; }

        /// <summary>
        /// Reagents that will be used in this experiment.
        /// </summary>
        public List<Reagent<J>> Reagents { get; set; }

        /// <summary>
        /// The number of replicates for this experiment.
        /// </summary>
        public int Replicates { get; set; }
    }
}
