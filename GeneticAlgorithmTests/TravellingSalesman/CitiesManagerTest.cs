using Microsoft.VisualStudio.TestTools.UnitTesting;

using GeneticAlgorithm.TravelingSalesman;
using Xunit;
using Assert = Xunit.Assert;
using static GeneticAlgorithm.Utils;
using GeneticAlgorithm;

namespace GeneticAlgorithmTests.TravellingSalesman
{
    public class CitiesManagerTest
    {
        public class TotalDistanceInputs : TheoryData<KeyValuePair<int, int>[], int[], double>
        {
            public TotalDistanceInputs()
            {
                // ----- Generating first test, standard inputs
                //Generating cities
                CitiesBuilder citiesBuilder = new();
                citiesBuilder.Add(3, 3);
                citiesBuilder.Add(4, 4);
                citiesBuilder.Add(0, 0);
                citiesBuilder.Add(1, 1);
                citiesBuilder.Add(2, 2);

                //Generating order
                int[]? order = new int[5];
                order[0] = 2;
                order[1] = 3;
                order[2] = 4;
                order[3] = 0;
                order[4] = 1;

                //Generating expectedValue
                double expectedValue = Math.Sqrt(32);

                //Adding test inputs
                Add(citiesBuilder.Construct(), order, expectedValue);



                // ----- Generating second test, no cities
                //Generating cities
                citiesBuilder = new();

                //Generating order
                order = null;

                //Generating expectedValue
                expectedValue = 0.0;

                //Adding test inputs
                Add(citiesBuilder.Construct(), order, expectedValue);
            }
        }

        /// <summary>
        /// Tests the method "TotalDistance(int[] order)"
        /// </summary>
        /// <param name="cities">The cities to be used during the test</param>
        /// <param name="order">The order of cities to be calculated</param>
        /// <param name="expectedValue">The expected total distance</param>
        [Theory]
        [ClassData(typeof(TotalDistanceInputs))]
        public void TotalDistanceStandardInput(KeyValuePair<int, int>[] cities, int[] order, double expectedValue)
        {
            CitiesManager cm = new(cities);
            double actualValue = cm.TotalDistance(order);

            Assert.Equal(expectedValue, actualValue);
        }

        public class BestOrderInputs : TheoryData<KeyValuePair<int, int>[], int[][], double>
        {
            public BestOrderInputs()
            {
                // ----- Generating first test, standard inputs
                //Generating unsortedCities
                CitiesBuilder citiesBuilder = new();
                citiesBuilder.Add(3, 3);
                citiesBuilder.Add(4, 4);
                citiesBuilder.Add(0, 0);
                citiesBuilder.Add(1, 1);
                citiesBuilder.Add(2, 2);

                //Generating citiesOrders
                int[][]? order = new int[2][];
                //Order 1
                order[0] = new int[5];
                order[0][0] = 2;
                order[0][1] = 3;
                order[0][2] = 4;
                order[0][3] = 0;
                order[0][4] = 1;
                //Order 2
                order[1] = new int[5];
                order[1][0] = 1;
                order[1][1] = 0;
                order[1][2] = 4;
                order[1][3] = 3;
                order[1][4] = 2;

                //Generating expectedValue
                double expectedValue = 1.0 / Math.Sqrt(32);

                //Adding test inputs
                Add(citiesBuilder.Construct(), order, expectedValue);



                // ----- Generating second test, no cities
                //Generating unsortedCities
                citiesBuilder = new();

                //Generating citiesOrders
                order = null;

                //Generating expectedValue
                expectedValue = 1.0 / 0.0;

                //Adding test inputs
                Add(citiesBuilder.Construct(), order, expectedValue);
            }
        }

        /// <summary>
        /// Tests the method "BestOrder()"
        /// </summary>
        /// <param name="unsortedCities">The cities to be used during the test</param>
        /// <param name="citiesOrders">All acceptable orders of the unsorted cities</param>
        /// <param name="expectedValue">The expected total distance</param>
        [Theory]
        [ClassData(typeof(BestOrderInputs))]
        public void BestOrderStandardInput(KeyValuePair<int, int>[] unsortedCities, int[][] citiesOrders, double expectedValue)
        {
            int tempNumCorrect = 0;
            int numCorrect = 0;

            CitiesManager cm = new(unsortedCities);
            cm.BestOrder();

            var bestScore = cm.bestScore;
            var actualOrder = cm.bestSolution;

            Assert.NotNull(actualOrder);
            Assert.NotNull(bestScore);

            Console.WriteLine("expectedScore is: " + expectedValue);
            Console.WriteLine("actualScore is: " + cm.bestScore);

            Console.Write("expectedOrder is: ");
            if (citiesOrders != null)
            {
                for (int i = 0; i < citiesOrders.Length; i++)
                {
                    if (citiesOrders[i] != null)
                    {
                        tempNumCorrect = citiesOrders[i].Where((b, i) => b == actualOrder[i]).Count();

                        if (tempNumCorrect > numCorrect)
                        {
                            numCorrect = tempNumCorrect;
                        }

                        Console.WriteLine(Utils.IntArrToString(citiesOrders[i]));

                        Console.WriteLine("\nOR\n");
                    }
                }

                Assert.Equal(unsortedCities.Length, numCorrect);
            }
            else
            {
                Assert.Equal(0, numCorrect);
            }


            Console.Write("\nactualOrder is: " + Utils.IntArrToString(actualOrder));

            Assert.Equal(expectedValue, bestScore);
        }
    }
}
