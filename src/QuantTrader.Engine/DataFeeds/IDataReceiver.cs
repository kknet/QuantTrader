using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CTP;

namespace QuantTrader.DataFeeds
{
    public interface IDataReceiver
    {
        ConcurrentQueue<ThostFtdcDepthMarketDataField> MarketDataQueue { get; }
    }
}
