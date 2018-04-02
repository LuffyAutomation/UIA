﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.Util
{
    public class UtilTime
    {
        public static void WaitTime(double timeout)
        {
            Thread.Sleep((int)(timeout * 1000));
        }
        #region datediff
        public enum DateInterval
        {
            /// <summary>
            /// 
            /// </summary>
            Day,
            /// <summary>
            /// 
            /// </summary>
            DayOfYear,
            /// <summary>
            /// 
            /// </summary>
            Hour,
            /// <summary>
            /// 
            /// </summary>
            Minute,
            /// <summary>
            /// 
            /// </summary>
            Month,
            /// <summary>
            /// 
            /// </summary>
            Quarter,
            /// <summary>
            /// 
            /// </summary>
            Second,
            /// <summary>
            /// 
            /// </summary>
            Weekday,
            /// <summary>
            /// 
            /// </summary>
            WeekOfYear,
            /// <summary>
            /// 
            /// </summary>
            Year
        }
        public static long DateDiff(DateTime oldTime)
        {
            return DateDiff(oldTime, DateTime.Now, System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek, DateInterval.Second);
        }
        public static long DateDiff(DateTime oldTime, DateTime newTime, DateInterval interval = DateInterval.Second)
        {
            return DateDiff(oldTime, newTime, System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek, interval);
        }
        private static int GetQuarter(int nMonth)
        {
            if (nMonth <= 3)
                return 1;
            if (nMonth <= 6)
                return 2;
            if (nMonth <= 9)
                return 3;
            return 4;
        }
        public static long DateDiff(DateTime oldTime, DateTime newTime, DayOfWeek eFirstDayOfWeek, DateInterval interval = DateInterval.Second)
        {
            if (interval == DateInterval.Year)
                return newTime.Year - oldTime.Year;

            if (interval == DateInterval.Month)
                return (newTime.Month - oldTime.Month) + (12 * (newTime.Year - oldTime.Year));

            TimeSpan ts = newTime - oldTime;

            if (interval == DateInterval.Day || interval == DateInterval.DayOfYear)
                return Round(ts.TotalDays);

            if (interval == DateInterval.Hour)
                return Round(ts.TotalHours);

            if (interval == DateInterval.Minute)
                return Round(ts.TotalMinutes);

            if (interval == DateInterval.Second)
                return Round(ts.TotalSeconds);

            if (interval == DateInterval.Weekday)
            {
                return Round(ts.TotalDays / 7.0);
            }

            if (interval == DateInterval.WeekOfYear)
            {
                while (newTime.DayOfWeek != eFirstDayOfWeek)
                    newTime = newTime.AddDays(-1);
                while (oldTime.DayOfWeek != eFirstDayOfWeek)
                    oldTime = oldTime.AddDays(-1);
                ts = newTime - oldTime;
                return Round(ts.TotalDays / 7.0);
            }

            if (interval == DateInterval.Quarter)
            {
                double d1Quarter = GetQuarter(oldTime.Month);
                double d2Quarter = GetQuarter(newTime.Month);
                double d1 = d2Quarter - d1Quarter;
                double d2 = (4 * (newTime.Year - oldTime.Year));
                return Round(d1 + d2);
            }

            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dVal"></param>
        /// <returns></returns>
        private static long Round(double dVal)
        {
            if (dVal >= 0)
                return (long)Math.Floor(dVal);
            return (long)Math.Ceiling(dVal);
        }
        #endregion
    }
}
