using System;
using System.Collections.Generic;
using System.Text;

namespace PoolControllerCross
{
    public static class Extensions
    {
        /// <summary>
        /// Does a comparison against the hour and minute
        /// </summary>
        public static bool IsEqualTo(this TimeSpan thisTime, TimeSpan other)
        {
            return thisTime.Days == other.Days
                && thisTime.Hours == other.Hours
                && thisTime.Minutes == other.Minutes;
        }
    }
}
