using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWI.Controls;

namespace OptionChain
{
    class Holidays
    {
        public static List<DateTime> MarketHolidays = new List<DateTime> 
                                                            {   
                                                                new DateTime (2014, 4, 18),
                                                                new DateTime (2014, 5, 26),
                                                                new DateTime (2014, 7, 4),
                                                                new DateTime (2014, 9, 1),
                                                                new DateTime (2014, 11, 27),
                                                                new DateTime (2014, 12, 25),
                                                                new DateTime (2015, 1, 1),
                                                                new DateTime (2015, 1, 19),
                                                                new DateTime (2015, 2, 16),
                                                                new DateTime (2015, 4, 3)
                                                            };
    }
    class Utils
    {
        public const int ibINIT_OC      = 0x00010000;
        public const int ibDATA         = 0x00020000;
        public const int ibPRICE        = 0x00040000;

        public static int GCD (int[] numbers)
        {
            return numbers.Aggregate (GCD);
        }

        static int GCD (int a, int b)
        {
            return b == 0 ? a : GCD (b, a % b);
        }

        /************************************************************
          * 
          * Massage
          * 
          * *********************************************************/

        public static string Massage (string ticker)
        {
            return ticker.Replace ('.', ' ');
        }

        /*****************************************************
         * 
         * Is this a trading day?
         * 
         * **************************************************/

        public static bool IfTradingDay (DateTime n)
        {
            DayOfWeek d = n.DayOfWeek;
            if (d == DayOfWeek.Saturday || d == DayOfWeek.Sunday)
            {
                return false;
            }
            if (Holidays.MarketHolidays.Contains (n))
            {
                return false;
            }
            return true;
        }

        /*******************************************************
         * 
         * Is the market open?
         * 
         * ****************************************************/

        public static bool IfTradingNow ()
        {
            DateTime today = DateTime.Today;
            if (IfTradingDay (today))
            {
                if (today + new TimeSpan (6, 30, 0) <= DateTime.Now && today + new TimeSpan (12, 59, 58) >= DateTime.Now)
                {
                    return true;
                }
            }
            return false;
        }

        /***********************************************************
         * 
         * GetNextTradingDay
         * 
         * ********************************************************/

        public static DateTime GetNextTradingDay ()
        {
            DateTime n = DateTime.Today;
            do
            {
                n += new TimeSpan (1, 0, 0, 0);
                if (IfTradingDay (n))
                {
                    return n;
                }
            } while (true);
        }

        /************************************************************
          * 
          * Compute Days to Expire (both trading days and all days)
          * 
          * ********************************************************/

        public static int? ComputeDaysToExpire (DateTime? expiry)
        {
            if (expiry == null)
            {
                return null;
            }
            TimeSpan full_days = (DateTime) expiry - DateTime.Now;
            return (int) (Math.Ceiling (full_days.TotalDays)) + 1;
        }

        public static string ComputeDaysToExpire (DateTime Expires, out int TradingDays)
        {
            TradingDays = 1;
            TimeSpan one_day = new TimeSpan (1, 0, 0, 0);

            DateTime d = DateTime.Now;
            while (d < Expires)
            {
                if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                {
                    TradingDays++;
                }
                d += one_day;
            }

            TimeSpan full_days = Expires - DateTime.Now;

            string display_days = TradingDays.ToString () + " (" + (full_days.Days + 1).ToString () + ")";

            return display_days;
        }

        /*******************************************************************
         * 
         * Compute Next Expiry Date
         * 
         * ****************************************************************/

        static internal DateTime ComputeNextExpiryDate (DateTime dt)
        {
            DateTime d = new DateTime (dt.Year, dt.Month, 1);
            if (d.DayOfWeek == DayOfWeek.Saturday)
            {
                d += new TimeSpan (7, 0, 0, 0);
            }
            d -= new TimeSpan ((int) d.DayOfWeek, 0, 0, 0);

            d += new TimeSpan (5 + 14, 0, 0, 0);

            if (Holidays.MarketHolidays.Contains (d))
            {
                d -= new TimeSpan (1, 0, 0, 0);
            }

            if (d < dt)
            {
                return ComputeNextExpiryDate (dt += new TimeSpan (14, 0, 0, 0));
            }
            return d;
        }

    }
}
