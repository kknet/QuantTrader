using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantTrader.Launcher
{
    /// <summary>
    /// Factory class to create Quartz server implementations from.
    /// </summary>
    public class CTPQuoteServerFactory
    {
        // private static readonly ILog logger = LogManager.GetLogger(typeof (QuartzServerFactory));

        /// <summary>
        /// Creates a new instance of an Quartz.NET server core.
        /// </summary>
        /// <returns></returns>
        public static CTPQuoteServer CreateServer()
        {
            string typeName = typeof(CTPQuoteServer).AssemblyQualifiedName;

            Type t = Type.GetType(typeName, true);

            // logger.Debug("Creating new instance of server type '" + typeName + "'");
            CTPQuoteServer retValue = (CTPQuoteServer)Activator.CreateInstance(t);
            // logger.Debug("Instance successfully created");

            return retValue;
        }
    }
}
