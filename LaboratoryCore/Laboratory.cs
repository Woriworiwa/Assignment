using System;
using System.Collections.Generic;
using System.Linq;

namespace LaboratoryCore
{
    public class Laboratory
    {
        /// <summary>
        /// Run the experiments and fills the wells on one or multiple plates.
        /// </summary>        
        public List<Plate> RunExperiments(int plateSize, string[][] samples, string[][] reagents, int[] replicates)
        {
            List<Experiment> experiments = new List<Experiment>();

            // Check that all arrays have the same length
            if (samples.GetLength(0) != reagents.GetLength(0) || samples.GetLength(0) != replicates.Length)
            {
                throw new Exception("The array lengths is not the same for all inputs.");
            }

            for (int i = 0; i < samples.Length; i++)
            {
                Experiment experiment = new Experiment()
                {
                    Samples = Array.ConvertAll<string, Sample>(samples[i], value => new Sample { Name = value }).ToList<Sample>(),
                    Reagents = Array.ConvertAll<string, Reagent>(reagents[i], value => new Reagent { Name = value }).ToList<Reagent>(),
                    Replicates = replicates[i],
                    Id = i
                };

                experiments.Add(experiment);
            }

            return RunExperiments((PlateSizes)plateSize, experiments);
        }

        /// <summary>
        /// Run the experiments and fills the wells on one or multiple plates.
        /// </summary>
        private List<Plate> RunExperiments(PlateSizes plateSize, List<Experiment> experiments)
        {
            // Check the contstraints on the samples and reagents.
            CheckConstraints(experiments);

            // prepare plates needed for all the experiments.
            List<Plate> plates = PreparePlates(plateSize, experiments);            

            // Fill the wells
            FillWells(plates, experiments);

            return plates;
        }

        /// <summary>
        /// Generates the plates needed to run all the experiments.
        /// </summary>
        /// <param name="plateSize">The plate size.</param>
        /// <param name="experiments">The experiments that will be put on the plates.</param>
        /// <returns>Plates that will be enough to run the experiments.</returns>
        private List<Plate> PreparePlates(PlateSizes plateSize, List<Experiment> experiments)
        {
            // I have assumed that each repition will take a column.
            double numberOfAllReplicates = experiments.Select(x => x.Replicates).Sum();
            int numberOfPlates = (int)Math.Ceiling(numberOfAllReplicates / (plateSize == PlateSizes.Small ? 12 : 24));

            List<Plate> plates = new List<Plate>();
            for (int i = 0; i < numberOfPlates; i++)
            {
                plates.Add(Plate.CreatePlate(plateSize));
            }

            return plates;
        }        

        /// <summary>
        /// Fills the wells with the experiments.
        /// </summary>
        private void FillWells(List<Plate> plates, List<Experiment> experiments)
        {
            int i = 0;

            foreach (Experiment experiment in experiments)
            {
                for (int rep = 0; rep < experiment.Replicates; rep++)
                {
                    plates[i].MoveToNextColumn();

                    foreach (var sample in experiment.Samples)
                    {                        
                        foreach (var reagent in experiment.Reagents)
                        {

                            if (plates[i].IsFull())
                            {
                                i++;
                                plates[i].MoveToNextColumn();
                            }

                            plates[i].FillWell(experiment, sample, reagent);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Checks if an experiment follows the rules and conditions required before running it.
        /// </summary>
        private void CheckConstraints(List<Experiment> experiments)
        {
            // All reagent names must be unique.
            if (!CheckReagentsUnique(experiments))
            {
                //TODO: Tell the user which experiement has reagents that are not unique.
                throw new Exception("Reagents in the experiment should be unique");
            }

            // a sample can be used in multiple experiments, but is never used in the same experiment multiple times.
            if (!CheckSamplesUnique(experiments))
            {
                //TODO: Tell the user which experiement has samples that are not unique.
                throw new Exception("Samples in the experiment should be unique");
            }
        }

        /// <summary>
        /// Checks if the reagents for each experiment are unique.
        /// </summary>
        private bool CheckReagentsUnique(List<Experiment> experiments)
        {
            foreach (Experiment experiment in experiments)
            {
                if (experiment.Reagents.Distinct().Count() != experiment.Reagents.Count())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the samples for each experiment are unique.
        /// </summary>
        private bool CheckSamplesUnique(List<Experiment> experiments)
        {
            foreach (Experiment experiment in experiments)
            {
                if (experiment.Samples.Distinct().Count() != experiment.Samples.Count())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
