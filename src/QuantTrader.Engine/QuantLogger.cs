using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace QuantTrader
{
    public static class QuantLogger
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
