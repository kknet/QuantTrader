using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Topshelf;

using QuantTrader.DataFeeds;
using QuantTrader.Entities;
using QuantTrader.Configuration;

namespace QuantTrader
{
    /// <summary>
    /// 
    /// </summary>
    public class CTPQuoteServer : ServiceControl
    {
        private CTPAccountInfo _accountInfo = null;
        private CTPDataReceiver _dataReceiver = null;
        public CTPQuoteServer()
        {
            QuantTraderConfig QuantTraderConfig = QuantTraderGlobals.GetInstance().QuantTraderConfig;

            string userID = Console.ReadLine();
            string password = Console.ReadLine();

            _accountInfo = new CTPAccountInfo() { UserID = userID,Password = password };
            _dataReceiver = new CTPDataReceiver(_accountInfo);
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
            try
            {
                _dataReceiver.Initialize();
                _dataReceiver.Run();
            }
            catch (Exception e)
            {
                string strError = e.Message;
                throw;
            }
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            // no-op for now
        }


        /// <summary>
        /// TopShelf's method delegated to <see cref="Start()"/>.
        /// </summary>
        public bool Start(HostControl hostControl)
        {
            return true;
        }

        /// <summary>
        /// TopShelf's method delegated to <see cref="Stop()"/>.
        /// </summary>
        public bool Stop(HostControl hostControl)        
        {       
            return true;
        }
    }
}
