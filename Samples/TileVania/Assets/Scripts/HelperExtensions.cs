using System.Collections.Generic;

namespace IronPython.Custom
{
    public static class HelperExtensions
    {
        public static IEnumerable<int> IndexesOf(this string s, char x)
        {
            var foundIndexes = new List<int>();
            for (int i = s.IndexOf(x); i > -1; i = s.IndexOf(x, i + 1))
            {
                // for loop end when i=-1 ('a' not found)
                foundIndexes.Add(i);
            }

            return foundIndexes;
        }
    }
}