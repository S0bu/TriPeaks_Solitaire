using System;

namespace str
{
    public sealed class CustomStringFunctions
    {
        //Inclusive of startIndex and exclusive of EndIndex
        public static string NewSubString(string s, Int32 startIndex, Int32 endIndex)
        {
            string new_s = "";
            for(int i = startIndex; i < endIndex; i++)
            {
                new_s += s[i];
            }
            return new_s;
        }
    }
}
