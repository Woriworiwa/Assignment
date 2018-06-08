namespace LaboratoryCore
{
    /// <summary>
    /// A well is the small identation on the plastic plate where the mixin of reagent and samples are placed.
    /// </summary>
    public class Well
    {
        /// <summary>
        /// Creates a new instance of a well.
        /// </summary>
        public Well(Experiment experiment, Sample sample, Reagent reagent)
        {
            ExperimentId = experiment.Id;
            Sample = sample;
            Reagent = reagent;
        }

        /// <summary>
        /// Experiment ID.
        /// </summary>
        public int ExperimentId { get; set; }

        /// <summary>
        /// Sample to put in this well.
        /// </summary>
        public Sample Sample { get; set; }

        /// <summary>
        /// Reagent to put in this well.
        /// </summary>
        public Reagent Reagent { get; set; }
    }
}
