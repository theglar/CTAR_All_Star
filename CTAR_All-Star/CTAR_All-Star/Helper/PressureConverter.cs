using System;
using System.Collections.Generic;
using System.Text;

namespace CTAR_All_Star.Helper
{
    static class PressureConverter
    {
        public static double convertToPSI(int value)
        {
            return ((7.25 / 819) * value) - ((7.25 * 106) / 819);
        }

        public static double convertToKPA(int value)
        {
            return ((50 / 819) * value) - ((50 * 106) / 819);
        }

        public static double convertToMMHG(int value)
        {
            double x = ((375.031 / 819) * value) - ((375.031 * 106) / 819);
            return x;
        }
    }
}
