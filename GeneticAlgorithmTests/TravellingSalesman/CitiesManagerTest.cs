using Microsoft.VisualStudio.TestTools.UnitTesting;

using GeneticAlgorithm.TravelingSalesman;
using Xunit;
using Assert = Xunit.Assert;

namespace GeneticAlgorithmTests.TravellingSalesman
{
    public class CitiesManagerTest
    {
        public class TotalDistanceInputs : TheoryData<KeyValuePair<int, int>[], double>
        {
            public TotalDistanceInputs()
            {
                CitiesBuilder citiesBuilder = new();
                citiesBuilder.Add(0, 0);
                citiesBuilder.Add(1, 1);
                citiesBuilder.Add(2, 2);
                citiesBuilder.Add(3, 3);
                citiesBuilder.Add(4, 4);
                Add(citiesBuilder.Construct(), Math.Sqrt(32));

                citiesBuilder = new();
                Add(citiesBuilder.Construct(), 0.0);
            }
        }

        [Theory]
        [ClassData(typeof(TotalDistanceInputs))]
        public void TotalDistanceStandardInput(KeyValuePair<int, int>[] cities, double expectedValue)
        {            
            int[] order = new int[cities.Length];
            for (int i = 0; i < order.Length; i++)
            {
                order[i] = i;
            }

            CitiesManager cm = new(cities);
            double actualValue = cm.TotalDistance(order);

            Assert.Equal(expectedValue, actualValue);
        }


        [Fact]
        public void BestOrderStandardInput()
        {
            KeyValuePair<int, int>[] cities = new KeyValuePair<int, int>[5];

            cities[0] = new KeyValuePair<int, int>(0, 0);
            cities[1] = new KeyValuePair<int, int>(1, 1);
            cities[2] = new KeyValuePair<int, int>(2, 2);
            cities[3] = new KeyValuePair<int, int>(3, 3);
            cities[4] = new KeyValuePair<int, int>(4, 4);

            CitiesManager cm = new(cities);
            cm.BestOrder();
            var bestScore = cm.bestScore;
            var bestSolution = cm.bestSolution;

            double expectedScore = 1.0 / Math.Sqrt(32);
            int[] expectedOrder = new int[cities.Length];
            for (int i = 0; i < expectedOrder.Length; i++)
            {
                expectedOrder[i] = i;
            }

            Assert.NotNull(bestSolution);
            Assert.NotNull(bestScore);

            int numCorrect = expectedOrder.Where((b, i) => b == bestSolution[i]).Count();

            Console.WriteLine("expectedScore is: " + expectedScore);
            Console.WriteLine("actualScore is: " + cm.bestScore);
            Console.Write("expectedOrder is: " + expectedOrder[0]);
            for (int i = 1; i < expectedOrder.Length; i++)
            {
                Console.Write(", " + expectedOrder[i]);
            }
            Console.Write("\nactualOrder is: " + bestSolution[0]);
            for (int i = 1; i < bestSolution.Length; i++)
            {
                Console.Write(", " + bestSolution[i]);
            }

            Assert.Equal(120, cm.numPermutations);
            Assert.Equal(expectedScore, bestScore);
            Assert.Equal(expectedOrder.Length, numCorrect);
        }
    }
}
