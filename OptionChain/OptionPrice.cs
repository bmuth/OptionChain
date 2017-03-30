using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionChain
{
    class OptionPrice
    {
        public DateTime Stamp { get; set; }
        public double Open { get; set; }
        public double Close  { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Delta {get; set;}


        public OptionPrice (DateTime stamp, double open, double close, double high, double low)
        {
            Stamp = stamp;
            Open = open;
            Close = close;
            High = high;
            Low = low;
        }

        internal double ComputeDelta (Option opt, double stockprice)
        {
            double iv;
            double delta = 0;
            double gamma = 0;
            double theta = 0;
            double vega = 0;
            double rho = 0;

            int? days_left = Utils.ComputeDaysToExpire (Stamp);
            if (days_left == null)
            {
                throw new Exception (string.Format ("Failed to ComputeDaysToExpire for {0}", Stamp.ToString ()));
            }

            try
            {
                if (opt.bIfCall)
                {
                    iv = FinancialRecipes.FR.OptionPriceImpliedVolatilityCallBlackScholes (stockprice, opt.Strike, 0.0 /*rate*/, (int) days_left, Close);
                    FinancialRecipes.FR.OptionPricePartialsCallBlackScholes (stockprice, opt.Strike, 0.0 /*rate*/, iv, (int) days_left, out delta, out gamma, out theta, out vega, out rho);
                }
                else
                {
                    iv = FinancialRecipes.FR.OptionPriceImpliedVolatilityPutBlackScholes (stockprice, opt.Strike, 0.0 /*rate*/, (int) days_left, Close);
                    FinancialRecipes.FR.OptionPricePartialsPutBlackScholes (stockprice, opt.Strike, 0.0 /*rate*/, iv, (int) days_left, out delta, out gamma, out theta, out vega, out rho);
                }

                Delta = delta;
                //this.MyTheta = theta;
                //this.MyGamma = gamma;
                //this.MyVega = vega;

                return Delta;
            }
            catch (Exception e)
            {
                throw new Exception (string.Format ("ComputeDelta for {0} close: {1}, {2}", opt.LocalSymbol, Close, e.Message));
            }
        }
    }

    class Option
    {
        public string LocalSymbol { get; set; }
        public string Ticker { get; set; }
        public double Strike { get; set; }
        public DateTime Expiry { get; set; }
        public bool bIfCall { get; set; }

        public DateTime Stamp { get; set; }
        public float Bid { get; set; }
        public float Ask { get; set; }
        public float Close { get; set; }
        public float Last { get; set; }
        public float? OptPrice { get; set; }
        public float? UndPrice { get; set; }
        public double Delta { get; set; }
        public int ctr { get; set; }

        public Option (string local_symbol, string ticker, double strike, DateTime expiry, bool ifcall)
        {
            LocalSymbol = local_symbol;
            Ticker = ticker;
            Strike = strike;
            Expiry = expiry;
            bIfCall = ifcall;
            UndPrice = null;
            OptPrice = null;
            ctr = 0;
        }

        //public double? ComputeDelta ()
        //{
        //    if (OptionPriceList.Count == 0)
        //    {
        //        return null;
        //    }

        //    /* Get the approx. price of underlying
        //     * ----------------------------------- */

        //    OptionPrice op = OptionPriceList[0];

        //    double UnderlyingPrice;
        //    using (dbOptionsDataContext dc = new dbOptionsDataContext ())
        //    {
        //        var s = (from p in dc.PriceHistories
        //                 where p.Ticker == Ticker && op.Stamp.Date == p.PriceDate && p.PriceTime == new TimeSpan (0, 0, 0)
        //                 select p.ClosingPrice).FirstOrDefault ();
        //        if (s == null)
        //        {
        //            throw (new Exception (string.Format ("Failed to find the underlying price for {0} on {1}", Ticker, op.Stamp.Date.ToLongDateString ())));
        //        }
        //        UnderlyingPrice = (double) s;
        //    }

        //    return op.ComputeDelta (this, UnderlyingPrice);
        //}
    }
}
