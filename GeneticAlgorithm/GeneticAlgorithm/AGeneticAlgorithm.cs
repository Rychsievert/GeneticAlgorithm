using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.GeneticAlgorithm
{
    internal abstract class AGeneticAlgorithm
    {
        public List<IIndividual> individuals;
        public int maxGeneration;
        public int currentGeneration;
        public int populationSize;
        public int numIndividualsToKeep;
        public int numIndividualsToEliminate;
        public IIndividual? bestSolution;
        public UniRandom random;

        protected AGeneticAlgorithm(int populationSize, double percentToEliminate)
        {
            individuals = new List<IIndividual>();
            this.populationSize = populationSize;
            this.numIndividualsToEliminate = (int)(populationSize * percentToEliminate);
            this.numIndividualsToKeep = populationSize - numIndividualsToEliminate;
            currentGeneration = 0;

            random = UniRandom.getInstance();
        }

        public IIndividual Run()
        {
            Initialize();
            bestSolution ??= individuals[0];
            Evaluation();

            while (!ExitCondition())
            {
                Selection();
                GenerateNewPopulation();
                Evaluation();
                currentGeneration++;
            }

            return bestSolution;
        }

        public abstract void Initialize();

        public void Evaluation()
        {
            foreach (var individual in individuals)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (bestSolution.GetFitness() < individual.GetFitness())
                {
                    bestSolution = individual;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        }

        public abstract void Selection();

        public void FitnessProportionalRouletteSelection()
        {
            var totalFitness = 0.0;
            foreach (var individual in individuals)
            {
                totalFitness += individual.GetFitness();
            }
            
            for (var i = 0; i < numIndividualsToEliminate; i++)
            {
                var target = random.NextDouble(totalFitness);
                var currentFitness = 0.0;
                for (var j = 0; j < individuals.Count; j++)
                {
                    if (currentFitness <= target && target < currentFitness + individuals[j].GetFitness())
                    {
                        totalFitness -= individuals[j].GetFitness();
                        individuals.RemoveAt(j);
                        j = individuals.Count;
                    }
                    else
                    {
                        currentFitness += individuals[j].GetFitness();
                    }
                }
            }
        }

        public void TopSelection()
        {
            individuals.Sort(new IndividualComparer());
            for (var i = numIndividualsToKeep; i < individuals.Count; )
            {
                individuals.RemoveAt(i);
            }
        }

        

        public abstract void GenerateNewPopulation();

        public abstract bool ExitCondition();
        
        public bool GenerationLimitExitCondition()
        {
            return currentGeneration >= maxGeneration;
        }
    }
}
