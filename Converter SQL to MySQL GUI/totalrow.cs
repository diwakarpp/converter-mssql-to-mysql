using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter_SQL_to_MySQL_GUI {
    static class totalrow {
        private static float n;
        public static void setv(float n) {
            totalrow.n = n;
        }
        public static float getv() {
            return totalrow.n;
        }
    }
}
