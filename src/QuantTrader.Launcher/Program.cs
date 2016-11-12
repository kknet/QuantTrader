using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

using QuantTrader;
using QuantTrader.Configuration;
using QuantTrader.DataFeeds;
using QuantTrader.Entities;

namespace QuantTrader.Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            // change from service account's dir to more logical one
            Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

            HostFactory.Run(x =>
                                {
                                    x.RunAsLocalSystem();

                                    // x.SetDescription("QuoteServer");
                                    // x.SetDisplayName("CTP Quote Server");
                                    // x.SetServiceName("CTP Server Name");

                                    x.Service(factory =>
                                                  {
                                                      CTPQuoteServer server = new CTPQuoteServer();
                                                      server.Initialize();
                                                      return server;
                                                  });
                                });

            //CTPDataReceiver dataReceiver = new CTPDataReceiver(new CTPAccountInfo());
            //dataReceiver.Initialize();
            //dataReceiver.Run();
            
        }
    }
}
