using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SWI.Controls;

namespace OptionChain
{
    public partial class Form1 : Form
    {
        private System.Timers.Timer timerOptChain = new System.Timers.Timer ();
        private LogCtl m_Log;
        private Random m_random = new Random ();
       
        public Form1 ()
        {
            m_Log = new LogCtl ("OptionChain");
            m_Log.LogFilename = Properties.Settings.Default.LogDir + "OptionChain.log";

            InitializeComponent ();
        }

        /***************************************************************
         * 
         * Go
         * 
         * ************************************************************/

        private async void btnGo_Click (object sender, EventArgs e)
        {
            string ticker = tbTicker.Text;

            m_Log.Log (ErrorLevel.logINF, string.Format ("InitializeOptionChains: pre-fetching option chain for {0} {1}", ticker, dtpExpiry.Value));
            List<Option> ol = await InitializeOneOptionChain (ticker, dtpExpiry.Value);

        }

        /****************************************************************
          * 
          * InitializeOneOptionChain
          * 
          * *************************************************************/

        private Task<List<Option>> InitializeOneOptionChain (string ticker, DateTime expiry)
        {
            List<Option> optionchain = new List<Option> ();

            var tcs = new TaskCompletionSource<List<Option>> ();
            TWSLib.IContract contract = axTws.createContract ();

            contract.symbol = ticker;

            string sectype;
            string multiplier;

            switch (ticker.ToUpper ())
            {
                case "ES":
                    sectype = "FOP";
                    multiplier = "50";
                    break;

                case "CL":
                    sectype = "FOP";
                    multiplier = "1000";
                    break;

                case "ZB":
                    sectype = "FOP";
                    multiplier = "1000";
                    break;

                default:
                    sectype = "OPT";
                    multiplier = "100";
                    break;
            }



            contract.secType = sectype;
            contract.expiry = expiry.ToString ("yyyyMMdd");
            contract.strike = 0.0;
            contract.right = "";
            //contract.multiplier = multiplier;  // skips around the mini's in SPY, for example
            contract.exchange = "GLOBEX";
            contract.primaryExchange = "";
            contract.currency = "USD";
            contract.localSymbol = "";
            contract.includeExpired = 0;

            m_Log.Log (ErrorLevel.logDEB, string.Format ("Getting option chain for {0}", ticker));

            var errhandler = default (AxTWSLib._DTwsEvents_errMsgEventHandler);
            var datahandler = default (AxTWSLib._DTwsEvents_contractDetailsExEventHandler);
            var endhandler = default (AxTWSLib._DTwsEvents_contractDetailsEndEventHandler);

            int reqid = m_random.Next (0xFFFF);

            errhandler = new AxTWSLib._DTwsEvents_errMsgEventHandler ((s, e) =>
            {
                if (e.id == -1)
                {
                    m_Log.Log (ErrorLevel.logERR, string.Format ("InitializeOneOptionChain: error {0}", e.errorMsg));
                    return;
                }

                axTws.contractDetailsEx -= datahandler;
                axTws.errMsg -= errhandler;
                axTws.contractDetailsEnd -= endhandler;

                tcs.SetException (new Exception (e.errorMsg));
            });

            endhandler = new AxTWSLib._DTwsEvents_contractDetailsEndEventHandler ((s, e) =>
            {
                if ((e.reqId & 0xFFFF0000) != Utils.ibINIT_OC)
                {
                    return;
                }
                if ((e.reqId & 0xFFFF) == reqid)
                {
                    axTws.contractDetailsEx -= datahandler;
                    axTws.errMsg -= errhandler;
                    axTws.contractDetailsEnd -= endhandler;

                    tcs.SetResult (optionchain);
                }
            });

            datahandler = new AxTWSLib._DTwsEvents_contractDetailsExEventHandler ((s, e) =>
            {
                if ((e.reqId & 0xFFFF0000) != Utils.ibINIT_OC)
                {
                    return;
                }
                TWSLib.IContractDetails c = e.contractDetails;
                TWSLib.IContract d = (TWSLib.IContract) c.summary;
                if (reqid == (e.reqId & 0xFFFF))
                {
                    //m_Log.Log (ErrorLevel.logINF, string.Format ("InitializeOneOptionChain: New opt sym {0} localsym {1} mult: {2}, strike {3}", d.symbol, d.localSymbol, d.multiplier, d.strike));
                    optionchain.Add (new Option (d.localSymbol, d.symbol, d.strike, expiry, d.right == "C" ? true : false));
                }
            });

            axTws.contractDetailsEx += datahandler;
            axTws.errMsg += errhandler;
            axTws.contractDetailsEnd += endhandler;

            axTws.reqContractDetailsEx (Utils.ibINIT_OC | reqid, contract);
            return tcs.Task;
        }     

        /*****************************************************
         * 
         * Form Load
         * 
         * ***************************************************/
        private void Form1_Load (object sender, EventArgs e)
        {
            using (frmConnect Con = new frmConnect ())
            {
                while (true)
                {
                    if (Con.ShowDialog (this) == DialogResult.OK)
                    {
                        string host = "127.0.0.1";
                        try
                        {
                            axTws.connect (host, Con.Port, Con.ClientId);
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show (string.Format ("Please check your connection attributes. {0}", ex.Message));
                        }
                    }
                }
            }
        }
    }
}
