using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.TravelingSalesman
{
    /// <summary>
    /// A class designed for instantiating City arrays simpler
    /// </summary>
    public class CitiesBuilder
    {
        private List<KeyValuePair<int, int>> cities;

        public CitiesBuilder()
        {
            cities = new();
        }

        /// <summary>
        /// Adds a city to internal list
        /// </summary>
        /// <param name="city">The city to be added</param>
        public void Add(KeyValuePair<int, int> city)
        {
            cities.Add(city);
        }

        /// <summary>
        /// Adds a city to internal list
        /// </summary>
        /// <param name="x">The x coordinate of the city to be added</param>
        /// <param name="y">The y coordinate of the city to be added</param>
        public void Add(int x, int y)
        {
            cities.Add(new KeyValuePair<int, int> (x, y));
        }

        /// <summary>
        /// Converts its internal list into an array for external use
        /// </summary>
        /// <returns>The array of KeyValuePairs</returns>
        public KeyValuePair<int, int>[] Construct()
        {
            return cities.ToArray();
        }
    }
}
