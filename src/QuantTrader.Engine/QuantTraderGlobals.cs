using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QuantTrader.Configuration;

namespace QuantTrader
{
    public class QuantTraderGlobals
    {
        #region 静态方法或字段
        private static volatile QuantTraderGlobals _defaultInstance = null;
        private static readonly object lockObject = new object();

        private QuantTraderGlobals()
        {

        }

        public static QuantTraderGlobals GetInstance()
        {
            if (QuantTraderGlobals._defaultInstance == null)
            {
                lock (QuantTraderGlobals.lockObject)
                {
                    if (QuantTraderGlobals._defaultInstance == null)
                    {
                        QuantTraderGlobals._defaultInstance = new QuantTraderGlobals();
                    }
                }
            }
            return QuantTraderGlobals._defaultInstance;
        }

        #endregion

        public QuantTraderConfig _quantTraderConfig = null;

        public QuantTraderConfig QuantTraderConfig
        {
            get
            {
                if(_quantTraderConfig == null)
                {
                    QuantTraderConfigHelper configHelper = QuantTrader.Singleton<QuantTrader.Configuration.QuantTraderConfigHelper>.Instance;
                    _quantTraderConfig = configHelper.LoadConfig();
                }
                return _quantTraderConfig;
            }
        }
    }
}
