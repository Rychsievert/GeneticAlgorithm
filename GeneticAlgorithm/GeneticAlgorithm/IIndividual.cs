using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.GeneticAlgorithm
{
    internal interface IIndividual
    {

        /// <summary>
        /// Converts an individual to bytes
        /// </summary>
        /// <returns>A series of bytes representing the individual</returns>
        public byte[] Encode();

        /// <summary>
        /// Converts bytes to an individual
        /// </summary>
        /// <param name="data">bytes to become an individual</param>
        public void Decode(byte[] data);

        /// <summary>
        /// Gets the fitness score used when selecting a final individual
        /// </summary>
        /// <returns>A fitness score</returns>
        //public double GetFinalFitness();

        /// <summary>
        /// Creates a fitness score based on current generation
        /// </summary>
        /// <param name="currentGen"></param>
        /// <param name="maxGen"></param>
        /// <returns>Fitness Score</returns>
        public double GetFitness(int currentGen, int maxGen)
        {
            return GetFitness();
        }

        /// <summary>
        /// Creates/Gets a fitness score
        /// </summary>
        /// <returns>Fitness Score</returns>
        public double GetFitness();
    }

    public class IndividualComparer : IComparer<IIndividual>
    {
        int IComparer<IIndividual>.Compare(IIndividual? x, IIndividual? y)
        {
            if (x == null || y == null)
            {
                throw new ArgumentException("Invalid Arguments: Cannot be null");
            }

            return x.GetFitness().CompareTo(y.GetFitness());
        }
    }
}
