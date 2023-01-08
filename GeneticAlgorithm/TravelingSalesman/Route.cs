using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithm.GeneticAlgorithm;
using static GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.TravelingSalesman
{
    internal class Route : IIndividual
    {
        public static int routeNumber = 0;
        public static List<Route> allRoutes = new List<Route>();

        public int parent1;
        public int parent2;
        public int generation;
        public int[] order;
        public readonly int numRoute;
        private readonly double _fitness;

        public Route(int[] order, CitiesManager cm)
        {
            this.order = new int[order.Length];
            order.CopyTo(this.order, 0);

            numRoute = routeNumber++;
            this._fitness = 1 / cm.TotalDistance(order);
            allRoutes.Add(this);
        }

        public Route(byte[] data, CitiesManager cm)
        {
            order = new int[data.Length];
            Decode(data);

            numRoute = routeNumber++;
            this._fitness = 1 / cm.TotalDistance(order);
        }

        public void Decode(byte[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                order[i] = data[i];
            }
        }

        public byte[] Encode()
        {
            var output = new byte[order.Length];

            return output;
        }

        public double GetFitness()
        {
            return _fitness;
        }

        override
        public string ToString()
        {
            return "Route " + numRoute;
        }

        public string DetailedString()
        {
            var output = this + " (";

            for (var i = 0; i < order.Length; i++)
            {
                output += order[i] + ", ";
            }

            return output[..^2] + " )";
        }

        public static void ResetRouteNumber()
        {
            routeNumber = 0;
        }
    }
}
