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
        /// <typeparam name="T">Type of samples</typeparam>
        /// <typeparam name="J">Type of reagents</typeparam>
        public List<Plate<T,J>> RunExperiments<T,J>(int plateSize, T[][] samples, J[][] reagents, int[] replicates)
        {
            List<Experiment<T,J>> experiments = new List<Experiment<T,J>>();

            // Check that all arrays have the same length
            if (samples.GetLength(0) != reagents.GetLength(0) || samples.GetLength(0) != replicates.Length)
            {
                throw new Exception("The array lengths is not the same for all inputs.");
            }

            for (int i = 0; i < samples.Length; i++)
            {
                Experiment<T,J> experiment = new Experiment<T,J>()
                {
                    Samples = Array.ConvertAll<T, Sample<T>>(samples[i], value => new Sample<T> { Data = value }).ToList<Sample<T>>(),
                    Reagents = Array.ConvertAll<J, Reagent<J>>(reagents[i], value => new Reagent<J> { Data = value }).ToList<Reagent<J>>(),
                    Replicates = replicates[i],
                    Id = i
                };

                experiments.Add(experiment);
            }

            return RunExperiments<T,J>((PlateSizes)plateSize, experiments);
        }

        /// <summary>
        /// Run the experiments and fills the wells on one or multiple plates.
        /// </summary>
        /// <typeparam name="T">Type of samples</typeparam>
        /// <typeparam name="J">Type of reagents</typeparam>
        private List<Plate<T,J>> RunExperiments<T,J>(PlateSizes plateSize, List<Experiment<T,J>> experiments)
        {
            // Check the contstraints on the samples and reagents.
            CheckConstraints(experiments);

            // prepare plates needed for all the experiments.
            List<Plate<T,J>> plates = PreparePlates(plateSize, experiments);            

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
        private List<Plate<T,J>> PreparePlates<T,J>(PlateSizes plateSize, List<Experiment<T,J>> experiments)
        {
            // I have assumed that each repition will take a column.
            double numberOfAllReplicates = experiments.Select(x => x.Replicates).Sum();
            int numberOfPlates = (int)Math.Ceiling(numberOfAllReplicates / (plateSize == PlateSizes.Small ? 12 : 24));

            List<Plate<T,J>> plates = new List<Plate<T,J>>();
            for (int i = 0; i < numberOfPlates; i++)
            {
                plates.Add(Plate<T,J>.CreatePlate(plateSize));
            }

            return plates;
        }        

        /// <summary>
        /// Fills the wells with the experiments.
        /// </summary>
        private void FillWells<T,J>(List<Plate<T,J>> plates, List<Experiment<T,J>> experiments)
        {
            int i = 0;

            foreach (var experiment in experiments)
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
        private void CheckConstraints<T,J>(List<Experiment<T,J>> experiments)
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
        private bool CheckReagentsUnique<T,J>(List<Experiment<T,J>> experiments)
        {
            foreach (var experiment in experiments)
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
        private bool CheckSamplesUnique<T,J>(List<Experiment<T,J>> experiments)
        {
            foreach (var experiment in experiments)
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
