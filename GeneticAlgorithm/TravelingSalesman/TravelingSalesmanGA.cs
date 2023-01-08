using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithm.GeneticAlgorithm;
using static GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.TravelingSalesman
{
    internal class TravelingSalesmanGA : AGeneticAlgorithm
    {
        private const double crossoverMutationRate = 0.03;
        private const double reorderRate = 0.08;

        private readonly bool _enableSeqFixing;
        private readonly int _seqLength;
        private readonly SelectionType _selectionType;
        public CitiesManager cm;
        public CrossoverType crossoverType;
        public double? crossoverRate;
        public MutationType mutationType;
        public double? mutationRate;
        public int[] basicOrder;

        public TravelingSalesmanGA(int populationSize, double percentToEliminate, int maxGeneration, CitiesManager citiesManager, CrossoverType crossoverType, MutationType mutationType, bool enableSeqFixing, SelectionType selectionType) : base(populationSize, percentToEliminate)
        {
            individuals = new List<IIndividual>();
            this.maxGeneration = maxGeneration;
            cm = citiesManager;
            this.crossoverType = crossoverType;
            this.mutationType = mutationType;
            basicOrder = new int[cm.numCities];
            for (var i = 0; i < cm.numCities; i++)
            {
                basicOrder[i] = i;
            }
            _seqLength = cm.numCities / 2 < 3 ? cm.numCities / 2 : 3;
            _enableSeqFixing = enableSeqFixing;
            _selectionType = selectionType;
        }

        public TravelingSalesmanGA(int populationSize, double percentToEliminate, int maxGeneration, CitiesManager citiesManager) : this(populationSize, percentToEliminate, maxGeneration, citiesManager, CrossoverType.Static, MutationType.Static, false, SelectionType.Roulette)
        { }

        public override bool ExitCondition()
        {
            return GenerationLimitExitCondition();
        }

        public override void GenerateNewPopulation()
        {
            for (var i = 0; i < numIndividualsToEliminate; i += 2)
            {
                var determinate = random.NextDouble();//TODO Ugly name, rename
                var nextIndex = i + 1 % numIndividualsToEliminate;
                nextIndex = nextIndex == 0 ? -1 : nextIndex;
                KeyValuePair<Route, Route> newIndividuals;
                if (determinate <= GetCrossoverRate())
                {
                    newIndividuals = Crossover((Route)individuals[i], (Route)individuals[i + 1]);
                    newIndividuals.Key.parent1 = ((Route)individuals[i]).numRoute;
                    newIndividuals.Key.parent2 = ((Route)individuals[i + 1]).numRoute;
                    newIndividuals.Key.generation = currentGeneration;
                    newIndividuals.Value.parent1 = ((Route)individuals[i]).numRoute;
                    newIndividuals.Value.parent2 = ((Route)individuals[i + 1]).numRoute;
                    newIndividuals.Value.generation = currentGeneration;
                }
                else //if (determinate >= getMutationRate()) //Currently unnecessary
                {
                    newIndividuals = new KeyValuePair<Route, Route>(Mutation((Route)individuals[i]), Mutation((Route)individuals[i + 1]));
                    newIndividuals.Key.parent1 = ((Route)individuals[i]).numRoute;
                    newIndividuals.Key.parent2 = ((Route)individuals[i]).numRoute;
                    newIndividuals.Key.generation = currentGeneration;
                    newIndividuals.Value.parent1 = ((Route)individuals[i + 1]).numRoute;
                    newIndividuals.Value.parent2 = ((Route)individuals[i + 1]).numRoute;
                    newIndividuals.Value.generation = currentGeneration;
                }

                if (_enableSeqFixing && random.NextDouble() <= reorderRate)
                {
                    var order = new int[cm.numCities];
                    newIndividuals.Key.order.CopyTo(order, 0);
                    for (var index = random.Next(_seqLength); index < cm.numCities - _seqLength; index += _seqLength)
                    {
                        var seqToReorder = new int[_seqLength];
                        for (var j = 0; j < _seqLength; j++)
                        {
                            seqToReorder[j] = newIndividuals.Key.order[index + j];
                        }

                        if (_seqLength < 4)
                            seqToReorder = cm.BestOrder(seqToReorder);
                        else
                        {
                            var percentToEliminate = populationSize / (double)numIndividualsToKeep;
                            var ga = new TravelingSalesmanGA(populationSize, percentToEliminate, maxGeneration * _seqLength / cm.numCities, cm, crossoverType, mutationType, true, _selectionType);
                            seqToReorder = ((Route)ga.Run()).order;
                        }
                        seqToReorder.CopyTo(order, index);
                    }
                    order.CopyTo(newIndividuals.Key.order, 0);
                }

                individuals.Add(newIndividuals.Key);
                if (nextIndex != -1)
                    individuals.Add(newIndividuals.Value);
                else
                    Route.allRoutes.RemoveAt(Route.allRoutes.Count-1);
            }
        }

        public KeyValuePair<Route, Route> Crossover(Route p1, Route p2)
        {
            var a = random.Next(basicOrder.Length);
            var b = (a + random.Next(1, basicOrder.Length)) % basicOrder.Length;
            if (a > b)
                (a, b) = (b, a);
            var c1 = new int[basicOrder.Length];
            p1.order.CopyTo(c1, 0);
            var c2 = new int[basicOrder.Length];
            p2.order.CopyTo(c2, 0);

            for (var i = a; i < b; i++)
            {
                (c1[i], c2[i]) = (c2[i], c1[i]);
            }

            List<int> missingC1 = new();
            List<int> missingC2 = new();

            for (var i = 0; i < basicOrder.Length; i++)
            {
                if (c1.Count(n => n == i) == 0)
                    missingC1.Add(i);
                if (c2.Count(n => n == i) == 0)
                    missingC2.Add(i);
            }

            for (var i = 0; i < basicOrder.Length; i++)
            {
                if (c1.Count(n => n == i) > 1)
                {
                    a = random.Next(missingC1.Count);
                    c1[Array.IndexOf(c1, i)] = missingC1[a];
                    missingC1.RemoveAt(a);
                }
                if (c2.Count(n => n == i) > 1)
                {
                    a = random.Next(missingC2.Count);
                    c2[Array.IndexOf(c2, i)] = missingC2[a];
                    missingC2.RemoveAt(a);
                }
            }

            //6% chance of values being swapped with random other values per value
            for (var i = 0; i < basicOrder.Length; i++)
            {
                if (random.NextDouble() < crossoverMutationRate)
                {
                    Swap(ref c1[i], ref c1[random.Next(basicOrder.Length)]);
                }
                if (random.NextDouble() < crossoverMutationRate)
                {
                    Swap(ref c2[i], ref c2[random.Next(basicOrder.Length)]);
                }
            }

            Route child1 = new(c1, cm);
            Route child2 = new(c2, cm);

            return new KeyValuePair<Route, Route>(child1, child2);
        }

        public double GetCrossoverRate()
        {
            if (crossoverRate == null)
            {
                switch (crossoverType)
                {
                    case CrossoverType.Static:
                        crossoverRate =  1.0;
                        break;
                    case CrossoverType.Increasing:
                        crossoverRate =  currentGeneration / maxGeneration;
                        break;
                    case CrossoverType.Decreasing:
                        crossoverRate =  (maxGeneration - currentGeneration) / maxGeneration;
                        break;
                }
            }
#pragma warning disable CS8629 // Nullable value type may be null.
            return (double)crossoverRate;
#pragma warning restore CS8629 // Nullable value type may be null.
        }

        public Route Mutation (Route parent)
        {
            var guarantee = random.Next(basicOrder.Length);
            var order = new int[basicOrder.Length];
            parent.order.CopyTo(order, 0);

            for (var i = 0; i < basicOrder.Length; i++)
            {
                if (guarantee == i || random.NextDouble() <= getMutationRate())
                    Swap(ref order[i], ref order[random.Next(basicOrder.Length)]);
            }

            return new Route(order, cm);
        }

        public double getMutationRate()
        {
            if (mutationRate == null)
            {
                switch (mutationType)
                {
                    case MutationType.Static:
                        mutationRate = 0.15;
                        break;
                    case MutationType.Increasing:
                        mutationRate = currentGeneration / maxGeneration;
                        break;
                    case MutationType.Decreasing:
                        mutationRate = (maxGeneration - currentGeneration) / maxGeneration;
                        break;
                }
            }
#pragma warning disable CS8629 // Nullable value type may be null.
            return (double)mutationRate;
#pragma warning restore CS8629 // Nullable value type may be null.
        }

        public override void Initialize()
        {
            var order = new int[cm.numCities];
            int a;
            int b;
            basicOrder.CopyTo(order, 0);

            for (var i = 0; i < populationSize; i++)
            {
                for (var j = 0; j < order.Length*5; j++)
                {
                    a = random.Next(order.Length);
                    b = random.Next(order.Length);
                    (order[a], order[b]) = (order[b], order[a]);
                }
                base.individuals.Add(new Route(order, cm));
            }
        }

        public override void Selection()
        {
            switch (_selectionType)
            {
                case SelectionType.Roulette:
                    FitnessProportionalRouletteSelection();
                    break;
                case SelectionType.Top:
                    TopSelection();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum CrossoverType { 
        Static = 0,
        Increasing = 1,
        Decreasing = 2
    };

    public enum MutationType { 
        Static = 0,
        Increasing = 1,
        Decreasing = 2
    };

    public enum SelectionType
    {
        Roulette = 0,
        Top = 1
    }
}
