using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantTrader.Entities
{
    public abstract class BarData
    {
        public double OpenPrice { get; set; }
        public double HighestPrice { get; set; }
        public double LowestPrice { get; set; }
        public double ClosePrice { get; set; }

        public int Volume { get; set; }

        ///成交金额
        public double Turnover;

        ///持仓量
        public int OpenInterest;
    }
    public class Bar
    {

    }
}
