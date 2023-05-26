using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BillRunStatisticsAndRestarts
{
    public static class StringFunctions
    {
        public static string Left(string s, int length)
        {
            if (string.IsNullOrEmpty(s) || length <= 0)
            {
                return string.Empty;
            }
            
            if (s.Length < length)
            {
                return s;
            }

            return s.Substring(0, length);
        }
    }
}
