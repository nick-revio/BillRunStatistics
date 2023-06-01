using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillRunStatisticsAndRestarts
{
    public class ParserFunctions
    {
        public static int GetInt(object input)
        {
            var s = Convert.ToString(input);
            return int.TryParse(s, out var x) ? x : 0;
        }

        public static double GetDouble(object input)
        {
            var s = Convert.ToString(input);
            return double.TryParse(s, out var x) ? x : 0;
        }

        public static DateTime? GetDateTime(object input) 
        {
            var s = Convert.ToString(input);
            return DateTime.TryParse(s, out var x) ? x : null;
        }
    }
}
