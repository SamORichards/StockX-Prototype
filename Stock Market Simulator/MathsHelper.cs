using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Market_Simulator {
    static class MathsHelper {
        public static double Lerp(double start, double end, double Percentage) {
            double Differnce = end - start;
            return start + (Differnce * Percentage);
        }
        public static long Lerp(long start, long end, double Percentage) {
            double Differnce = end - start;
            return start + (long)(Differnce * Percentage);
        }
    }
}
