using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    internal class UniRandom
    {
        static UniRandom? instance;
        Random rand;

        public static UniRandom getInstance()
        {
            if (instance == null)
                return new UniRandom();
            return instance;
        }

        private UniRandom()
        {
            var seed = DateTime.Now.Year*100000+DateTime.Now.Month*10000+DateTime.Now.Day*1000+DateTime.Now.Hour*100+DateTime.Now.Minute*10+DateTime.Now.Second;
            rand = new Random(seed);
            instance = this;
        }

        public int Next()
        {
            return rand.Next();
        }

        public int Next(int max)
        {
            return rand.Next(max);
        }

        public int Next(int min, int max)
        {
            return rand.Next(min, max);
        }

        public void NextBytes(byte[] byteArr)
        {
            rand.NextBytes(byteArr);
        }

        public double NextDouble()
        {
            return rand.NextDouble();
        }

        public double NextDouble(double max)
        {
            return rand.NextDouble() * max;
        }

        public double NextDouble(double min, double max)
        {
            return min + (rand.NextDouble() * (max - min));
        }
    }
}
