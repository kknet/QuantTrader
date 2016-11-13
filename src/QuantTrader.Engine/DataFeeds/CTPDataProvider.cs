using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CTP;
using QuantTrader.Entities;

namespace QuantTrader.DataFeeds
{
    /// <summary>
    /// 行情数据提供者
    /// </summary>
    public class CTPDataProvider : IDataProvider,IDisposable
    {
        #region 静态方法或字段

        private static volatile CTPDataProvider _defaultInstance = null;
        private static readonly object lockObject = new object();

        public static CTPDataProvider GetInstance(IDataReceiver dataReceiver)
        {
            if (CTPDataProvider._defaultInstance == null)
            {
                lock (CTPDataProvider.lockObject)
                {
                    if (CTPDataProvider._defaultInstance == null)
                    {
                        CTPDataProvider._defaultInstance = new CTPDataProvider(dataReceiver);
                    }
                }
            }
            return CTPDataProvider._defaultInstance;
        }

        #endregion

        private readonly static ConcurrentDictionary<string, ConcurrentDictionary<string, Tick>> _dictQuote = new ConcurrentDictionary<string, ConcurrentDictionary<string, Tick>>();
        private System.Threading.Thread _tickThread = null;
        private IDataReceiver _dataReceiver = null;
        
        private CTPDataProvider(IDataReceiver dataReceiver)
        {
            _dataReceiver = dataReceiver;
            // 初始化数据
            Initialize();
            // 处理实时数据
            DoProcessMarketDataQuote();
        }

        private void _loadTickData()
        {

        }

        private void _loadDayData()
        {

        }

        private void _loadMinuteData()
        {

        }

        /// <summary>
        /// 导入历史行情数据
        /// </summary>
        /// <param name="kType"></param>
        private void LoadHistoryData(KlineTypes kType = KlineTypes.Minute)
        {
            // 导入历史数据
            switch(kType)
            {
                case KlineTypes.Tick:
                    _loadMinuteData();
                    break;
                case KlineTypes.Minute:
                    _loadMinuteData();
                    break;
                case KlineTypes.Day:
                    _loadMinuteData();
                    break;
                default:
                    _loadMinuteData();
                    break;
            }
        }

        private void Initialize()
        {
            _loadTickData();            
            _loadMinuteData();
            _loadDayData();
        }

        private void _doProcessMarketDataQuote()
        {
            while (true)
            {
                ThostFtdcDepthMarketDataField quote = null;
                if(_dataReceiver.MarketDataQueue.TryDequeue(out quote))
                {
                    
                }
            }
        }

        public void DoProcessMarketDataQuote()
        {
            if (_tickThread == null)
            {
                _tickThread = new System.Threading.Thread(_doProcessMarketDataQuote);
            }

            _tickThread.IsBackground = true;
            _tickThread.Start();
        }        


        public void SaveQuoteToCsv()
        {
            // 保存数据csv文件
        }
        public void Dispose()
        {
            if(_tickThread != null)
            {
                _tickThread.Abort();
            }

            _tickThread = null;

            SaveQuoteToCsv();

            _dictQuote.Clear();
        }
    }
}
