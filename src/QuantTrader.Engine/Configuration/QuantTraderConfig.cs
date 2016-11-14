using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using NLog;

using QuantTrader.Entities;

namespace QuantTrader.Configuration
{
    public class DataPathConfig
    {
        public string Tick { get; set; }
        public string Minute { get; set; }
        public string Day { get; set; }

        public DataPathConfig()
        {
            Tick = "Data\\Tick";
            Minute = "Data\\Minute";
            Day = "Data\\Day";
        }
    }
    public class QuantTraderConfig
    {
        public List<CTPBroker> Brokers { get; set; }
        public List<string> SubscribeMarketDatas { get; set; }

        public DataPathConfig DataPath { get; set; }

        public CTPAccountInfo AccountInfo { get; set; }        

        public QuantTraderConfig()
        {
            //CTPBroker defaultBroker = new CTPBroker(){ID = "4700",Name = "东海期货"};
            //defaultBroker.QuoteServers = new List<string>(){"tcp://ctpdx1.dh168.com.cn:41213"};
            //defaultBroker.TradeServers = new List<string>(){"tcp://ctpdx1.dh168.com.cn:41205"};

            CTPBroker defaultBroker = new CTPBroker() { ID = "9999", Name = "SimNow" };
            defaultBroker.QuoteServers = new List<string>() { "tcp://180.168.146.187:10010" };
            defaultBroker.TradeServers = new List<string>() { "tcp://180.168.146.187:10000" };

            Brokers = new List<CTPBroker>() { defaultBroker };

            SubscribeMarketDatas = new List<string>();

            //SubscribeMarketDatas.Add("cu1705");
            //SubscribeMarketDatas.Add("ni1705");
            //SubscribeMarketDatas.Add("ru1705");
            //SubscribeMarketDatas.Add("ag1706");
            //SubscribeMarketDatas.Add("rb1705");
            //SubscribeMarketDatas.Add("i1705");
            //SubscribeMarketDatas.Add("j1705");
            //SubscribeMarketDatas.Add("jm1705");
            //SubscribeMarketDatas.Add("p1705");
            //SubscribeMarketDatas.Add("m1705");
            //SubscribeMarketDatas.Add("cf705");

            // 数据默认路径
            DataPath = new DataPathConfig();

            AccountInfo = new CTPAccountInfo();
        }
    }

    internal class QuantTraderConfigHelper
    {
        private string _localConfigFileName = "";
        public QuantTraderConfig LoadConfig()
        {
            QuantTraderConfig config = null;
            if (System.IO.File.Exists(_localConfigFileName))
            {
                string jsonText = File.ReadAllText(_localConfigFileName);
                if (jsonText.Length > 0)
                {
                    try
                    {
                        config = JsonConvert.DeserializeObject<QuantTraderConfig>(jsonText);
                    }
                    catch (Exception ex)
                    {
                        config = new QuantTraderConfig();
                        SaveConfig(config);

                        QuantLogger.logger.Error(ex);
                    }
                }
            }
            else
            {
                config = new QuantTraderConfig();
                SaveConfig(config);
            }
            return config;
        }

        public void SaveConfig(QuantTraderConfig config)
        {
            try
            {
                string strJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(_localConfigFileName, strJson, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                QuantLogger.logger.Error(ex);
            }
        }

        public QuantTraderConfigHelper()
        {
            _localConfigFileName = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "QuantTraderConfig.json");
        }
    }
}
