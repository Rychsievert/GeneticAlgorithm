namespace GeneticAlgorithm
{
    public static class Utils
    {

        public static KeyValuePair<byte, byte> SplitByteKVP(byte byteToSplit)
        {
            int val = byteToSplit;
            var key = val >> 4;
            val -= key << 4;

            return new KeyValuePair<byte, byte>((byte)key, (byte)val);
        }

        public static byte CombineBytes(KeyValuePair<byte, byte> bytesToCombine)
        {
            return (byte)((bytesToCombine.Key << 4) + bytesToCombine.Value);
        }

        public static byte CombineBytes(byte leftByte, byte rightByte)
        {
            return (byte)((leftByte << 4) + rightByte);
        }

        public static void Swap(ref int a, ref int b)
        {
            (a, b) = (b, a);
        }

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

        public static string IntArrToString(int[] arr)
        {
            var output = "(";
            foreach (var i in arr)
                output += i + ", ";
            return output[..^2] + ")";
        }

        public static string tab(int num)
        {
            var output = "";
            for (var i = 0; i < num; i++)
                output += "   ";
            return output;
        }
    }
}
