namespace GeneticAlgorithm
{
    public static class Utils
    {
        /// <summary>
        /// TO BE DEPRECATED - Bytes are no longer used
        /// 
        /// Splits a byte in half, with the left 4 bits and right 4 bits becoming two separate numbers.
        /// The left 4 bits are shifted four bits to the right when becoming their own byte
        /// </summary>
        /// <param name="byteToSplit">The byte to split</param>
        /// <returns>A KeyValuePair containing both bytes, with the leftByte as the key, and the rightByte as the value</returns>
        public static KeyValuePair<byte, byte> SplitByteKVP(byte byteToSplit)
        {
            int val = byteToSplit;
            var key = val >> 4;
            val -= key << 4;

            return new KeyValuePair<byte, byte>((byte)key, (byte)val);
        }

        /// <summary>
        /// TO BE DEPRECATED - Bytes are no longer used
        /// 
        /// Combines two bytes less than 16 into one byte for space efficiency
        /// The leftByte (or the key in this instance) is bit shifted 4 bits to the left in the process
        /// </summary>
        /// <param name="bytesToCombine">A KeyValuePair containing both bytes, with the key referring to the leftByte and the value referring to the rightByte</param>
        /// <returns>The new byte containing both bytes</returns>
        public static byte CombineBytes(KeyValuePair<byte, byte> bytesToCombine)
        {
            return (byte)((bytesToCombine.Key << 4) + bytesToCombine.Value);
        }

        /// <summary>
        /// TO BE DEPRECATED - Bytes are no longer used
        /// 
        /// Combines two bytes less than 16 into one byte for space efficiency
        /// The leftByte is bit shifted 4 bits to the left in the process
        /// </summary>
        /// <param name="leftByte">The byte to be stored to the left of the other byte</param>
        /// <param name="rightByte">The byte to be stored to the right of the other byte</param>
        /// <returns>The new byte containing both parameters</returns>
        public static byte CombineBytes(byte leftByte, byte rightByte)
        {
            return (byte)((leftByte << 4) + rightByte);
        }

        /// <summary>
        /// TO BE DEPRECATED - Unnecessary
        /// 
        /// Swaps both objects provided
        /// </summary>
        public static void Swap(ref int a, ref int b)
        {
            (a, b) = (b, a);
        }

        /// <summary>
        /// DO NOT DEPRECATE - Useful in verifying extensive solutions
        /// 
        /// Determines how many times each solution appears in the given list of solutions
        /// </summary>
        /// <param name="solutions">The list of solutions to be used</param>
        /// <returns>A Dictionary pairing each unique solution to the number of times it appears in the list of solutions</returns>
        public static Dictionary<int[], int> TallySolutions(List<int[]> solutions)
        {
            var output = new Dictionary<int[], int>();
            var key = new int[1];

            for (var i = 0; i < solutions.Count(); i++)
            {
                var contains = false;
                foreach (var kvp in output)
                {
                    if (!kvp.Key.SequenceEqual(solutions[i])) continue;
                    contains = true;
                    key = kvp.Key;
                }

                if (!contains)
                {
                    output.Add(solutions[i], 1);
                }
                else
                {
                    if (key != null && output.ContainsKey(key))
                    {
                        output[key]++;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// TO BE DEPRECATED - Unused
        /// 
        /// Converts an array of integers into a single string, with values separated by commas
        /// </summary>
        /// <param name="arr">The array to be converted</param>
        /// <returns>The string containing all the array's values</returns>
        public static string IntArrToString(int[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                return "()";
            }
            var output = "(";
            foreach (var i in arr)
                output += i + ", ";
            return output[..^2] + ")";
        }

        /// <summary>
        /// TO BE DEPRECATED - Unused
        /// 
        /// Returns a string with the number of spaces equal to num
        /// </summary>
        /// <param name="num">The number of spaces to return</param>
        /// <returns>The string containing the spaces</returns>
        public static string Tab(int num)
        {
            var output = "";
            for (var i = 0; i < num; i++)
                output += "   ";
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cities"></param>
        /// <returns></returns>
        public static string DetailedCities(KeyValuePair<int, int>[] cities)
        {
            var output = "";

            if (cities == null || cities.Length == 0)
            {
                return output;
            }

            for (var i = 0; i < cities.Length; i++)
            {
                output += "City " + i + ": " + cities[i].Key + " " + cities[i].Value + "\n";
            }

            return output[..^1];
        }
    }
}
