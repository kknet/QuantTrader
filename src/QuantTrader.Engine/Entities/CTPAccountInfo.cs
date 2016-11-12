using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace QuantTrader.Entities
{
    public class CTPAccountInfo
    {
        public string BrokerID { get; set; }
        public string QuoteServerAddress { get; set; }
        public string TradeServerAddress { get; set; }

        public string UserID { get; set; }
        public string Password { get; set; }

        public CTPAccountInfo()
        {
            BrokerID = "4700";
            QuoteServerAddress = "tcp://ctpdx1.dh168.com.cn:41213";
            TradeServerAddress = "tcp://ctpdx1.dh168.com.cn:41205";
        }
    }

    /// <summary>
    /// 期货代理商
    /// </summary>
    public class CTPBroker
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<string> QuoteServers { get; set; }
        public List<string> TradeServers { get; set; }

        public CTPBroker()
        {
            QuoteServers = new List<string>();
            TradeServers = new List<string>();
        }
    }


}
