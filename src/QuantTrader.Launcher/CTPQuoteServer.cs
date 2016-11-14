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
        private CTPDataProvider _dataProvider = null;
        public CTPQuoteServer()
        {
            QuantTraderConfig QuantTraderConfig = QuantTraderGlobals.GetInstance().QuantTraderConfig;
            _dataReceiver = new CTPDataReceiver(QuantTraderConfig.AccountInfo);
            _dataProvider = CTPDataProvider.GetInstance(_dataReceiver);
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
            try
            {
                _dataReceiver.Initialize();                
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
            _dataProvider.Dispose();            
        }


        /// <summary>
        /// TopShelf's method delegated to <see cref="Start()"/>.
        /// </summary>
        public bool Start(HostControl hostControl)
        {
            try
            {
                _dataReceiver.Run();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }            
        }

        /// <summary>
        /// TopShelf's method delegated to <see cref="Stop()"/>.
        /// </summary>
        public bool Stop(HostControl hostControl)        
        {
            // 保存数据
            if(_dataReceiver != null)
            {                
                _dataProvider.Dispose();
            }
            return true;
        }
    }
}
