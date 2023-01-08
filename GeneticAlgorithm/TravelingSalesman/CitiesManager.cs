using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using static GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.TravelingSalesman
{
    public class CitiesManager
    {
        private readonly KeyValuePair<int, int>[] _cities;
        public int[]? bestSolution;
        public double? bestScore;
        public int numPermutations;
        public readonly int numCities;
        public List<int[]> allSolutions;

        public CitiesManager(int numCities)
        {
            var rand = UniRandom.getInstance();
            _cities = new KeyValuePair<int, int>[numCities];
            for (var i = 0; i < numCities; i++)
            {
                _cities[i] = new KeyValuePair<int, int>(rand.Next(0, 255), rand.Next(0, 255));
            }
            this.numCities = numCities;

            allSolutions = new List<int[]>();
        }

        public CitiesManager(KeyValuePair<int, int>[] cities)
        {
            this._cities = cities;
            numCities = cities.Length;

            allSolutions = new List<int[]>();
        }

        public double TotalDistance(int[] order)
        {
            var output = 0.0;

            for (var i = 0; i < order.Length-1; i++)
            {
                var tempX = _cities[order[i]].Key - _cities[order[i + 1]].Key;
                var tempY = _cities[order[i]].Value - _cities[order[i + 1]].Value;
                output += Math.Sqrt(Math.Pow(tempX,2) + Math.Pow(tempY,2));
            }

            return output;
        }

        public void BestOrder()
        {
            var order = new int[_cities.Length];
            bestSolution = new int[_cities.Length];
            for (var i = 0; i < _cities.Length; i++)
            {
                order[i] = i;
                bestSolution[i] = i;
            }
            bestScore = 1 / TotalDistance(order);
            numPermutations = 0;

            Permutate(order, 0);
        }

        public int[] BestOrder(int[] citiesToOptimize)
        {
            var bestSolution = new int[citiesToOptimize.Length];
            citiesToOptimize.CopyTo(bestSolution, 0);
            var bestScore = 1 / TotalDistance(bestSolution);

            Permutate(citiesToOptimize, 0, bestSolution, ref bestScore);

            return bestSolution;
        }

        private void Permutate(int[] nums, int start)
        {
            if (start >= nums.Length - 1 && bestSolution != null && bestScore != null)
            {
                numPermutations++;

                var temp = 1 / TotalDistance(nums);
                if (bestScore < temp)
                {
                    nums.CopyTo(bestSolution, 0);
                    bestScore = temp;
                }

                return;
            }

            for (var i = start; i < nums.Length - 1; i++)
            {
                Permutate(nums, start + 1);
                Swap(ref nums[start], ref nums[i + 1]);
            }

            Permutate(nums, start + 1);

            for (var i = nums.Length - 2; i > start - 1; i--)
            {
                Swap(ref nums[start], ref nums[i + 1]);
            }
        }

        private void Permutate(int[] nums, int start, int[] bestSolution, ref double bestScore)
        {
            if (start >= nums.Length - 1)
            {
                numPermutations++;

                var temp = 1 / TotalDistance(nums);
                if (bestScore < temp)
                {
                    nums.CopyTo(bestSolution, 0);
                    bestScore = temp;
                }

                return;
            }

            for (var i = start; i < nums.Length - 1; i++)
            {
                Permutate(nums, start + 1, bestSolution, ref bestScore);
                Swap(ref nums[start], ref nums[i + 1]);
            }

            Permutate(nums, start + 1, bestSolution, ref bestScore);

            for (var i = nums.Length - 2; i > start - 1; i--)
            {
                Swap(ref nums[start], ref nums[i + 1]);
            }
        }

        public string DetailedBestSolution()
        {
            var output = "( ";

            foreach (var city in bestSolution)
            {
                output += city + ", ";
            }

            return output[..^2] + " )";
        }

        public string DetailedCities()
        {
            var output = "";

            for (var i = 0; i < numCities; i++)
            {
                output += "City " + i + ": " + _cities[i].Key + " " + _cities[i].Value + "\n";
            }

            return output[..^1];
        }
    }
}
