using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using System.Linq;
using System.Text;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Globalization
{
    /// <summary>
    /// Specifies different data formats.
    /// </summary>
    public enum DateFormats
    {
        /// <summary>
        /// e.g. 21 Jan 11
        /// </summary>
        ddMMMyy,
        /// <summary>
        /// e.g. 21st Jan 2011
        /// </summary>
        ddsMMMyyyy,
        /// <summary>
        /// e.g. 21 Jan 11, 21:30
        /// </summary>
        ddMMMyyHHMM,
        /// <summary>
        /// e.g. 21:30
        /// </summary>
        HHMM,
        /// <summary>
        /// e.g. 9:30 pm
        /// </summary>
        hMMtt,
        /// <summary>
        /// e.g. Jan
        /// </summary>
        MMM,
        /// <summary>
        /// e.g. January
        /// </summary>
        MMMM
    }

    /// <summary>
    /// Provides helper methods for date formatting.
    /// </summary>
    public class DateHelper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateHelper"/> class.
        /// </summary>
        private DateHelper() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the string value of the specified data in the specified format for the current
        /// culture.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string ToString(DateTime date, DateFormats format)
        {
            #region Check Parameters

            if (date == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "date is nothing"));

            #endregion

            string r = string.Empty;

            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "am";
            fi.PMDesignator = "pm";

            string dateSuffix = GetDateSuffix(date.Day);

            switch (format)
            {
                case DateFormats.ddMMMyy:
                    r = date.ToString(@"dd MMM \'yy", Thread.CurrentThread.CurrentCulture);
                    break;
                case DateFormats.ddsMMMyyyy:
                    r = date.Day.ToString() + dateSuffix + " " + date.ToString(@"MMM yyyy", Thread.CurrentThread.CurrentCulture);
                    break;
                case DateFormats.ddMMMyyHHMM:
                    r = date.ToString("dd MMM yy, HH:mm", Thread.CurrentThread.CurrentCulture);
                    break;
                case DateFormats.HHMM:
                    r = date.ToString("HH:mm", Thread.CurrentThread.CurrentCulture);
                    break;
                case DateFormats.hMMtt:
                    r = date.ToString("h:mm tt", fi);
                    break;
                case DateFormats.MMM:
                    r = date.ToString("MMM", Thread.CurrentThread.CurrentCulture);
                    break;
                case DateFormats.MMMM:
                    r = date.ToString("MMMM", Thread.CurrentThread.CurrentCulture);
                    break;
                default:
                    break;
            }

            return r;
        }

        /// <summary>
        /// Determines whether [is julian date asynchronous] [the specified year].
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">This date is not valid as it does not exist in either the Julian or the Gregorian calendars.</exception>
        public static bool IsJulianDateYN(int year, int month, int day)
        {
            // All dates prior to 1582 are in the Julian calendar
            if (year < 1582)
                return true;
            // All dates after 1582 are in the Gregorian calendar
            else if (year > 1582)
                return false;
            else
            {
                // If 1582, check before October 4 (Julian) or after October 15 (Gregorian)
                if (month < 10)
                    return true;
                else if (month > 10)
                    return false;
                else
                {
                    if (day < 5)
                        return true;
                    else if (day > 14)
                        return false;
                    else
                        // Any date in the range 10/5/1582 to 10/14/1582 is invalid 
                        throw new ArgumentOutOfRangeException(
                            "This date is not valid as it does not exist in either the Julian or the Gregorian calendars.");
                }
            }
        }

        /// <summary>
        /// Automatics the julian date.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        /// <param name="millisecond">The millisecond.</param>
        /// <returns></returns>
        static public double ToJulianDate(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return DoToJulianDate(year, month, day, hour, minute, second, millisecond);
        }


        /// <summary>
        /// Automatics the julian date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        static public double ToJulianDate(DateTime date)
        {
            return DoToJulianDate(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        static public double ToUnixTimestamp(DateTime date)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimestampInTicks = (date.ToUniversalTime() - unixStart).Ticks;
            return (double)unixTimestampInTicks / TimeSpan.TicksPerSecond;

        }

        static public DateTime FromUnixTimestamp(double unixTimestamp)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimestampInTicks = (long)(unixTimestamp * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimestampInTicks, System.DateTimeKind.Utc);

        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Automatics the julian date.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        /// <param name="millisecond">The millisecond.</param>
        /// <returns></returns>
        static private double DoToJulianDate(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            // Determine correct calendar based on date
            bool JulianCalendar = IsJulianDateYN(year, month, day);

            int M = month > 2 ? month : month + 12;
            int Y = month > 2 ? year : year - 1;
            double D = day + hour / 24.0 + minute / 1440.0 + (second + millisecond * 1000) / 86400.0;
            int B = JulianCalendar ? 0 : 2 - Y / 100 + Y / 100 / 4;

            return (int)(365.25 * (Y + 4716)) + (int)(30.6001 * (M + 1)) + D + B - 1524.5;
        }

        /// <summary>
        /// Gets the date suffix.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        static private string GetDateSuffix(int date)
        {
            #region Check Parameters

            if (date <= 0 || date > 31) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "date is invalid"));

            #endregion

            switch (date)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        #endregion
    }
}
