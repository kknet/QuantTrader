using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantTrader.Entities
{
    /// <summary>
    /// K线类型
    /// </summary>
    public enum KlineTypes
    {
        Tick = 1,
        Second = 2,
        Minute = 3,
        Hour = 4,
        Day = 5,
        Week = 6,
        Month = 7,
        Custom = 10
    }
}
