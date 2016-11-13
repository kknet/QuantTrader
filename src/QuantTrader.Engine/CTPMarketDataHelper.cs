using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;
using QuantTrader.Entities;

namespace QuantTrader
{

    public static class CTPMarketDataHelper
    {
        private const int Hour = 16;

        private static TimeSpan nightTimeSpan = new TimeSpan(20, 0, 0);

        private static TimeSpan dayTimeSpan = new TimeSpan(8, 0, 0);

        public static DateTime PreTradingDay
        {
            get;
            set;
        }

        public static Tick ConvertToTick(ThostFtdcDepthMarketDataField field)
        {
            return new Tick
            {
                InstrumentID = field.InstrumentID,
                ExchangeID = field.ExchangeID,
                ExchangeInstID = field.ExchangeID,
                LastPrice = CTPMarketDataHelper.GetDoubleValue(field.LastPrice),
                PreSettlementPrice = CTPMarketDataHelper.GetDoubleValue(field.PreSettlementPrice),
                PreClosePrice = CTPMarketDataHelper.GetDoubleValue(field.PreClosePrice),
                OpenPrice = CTPMarketDataHelper.GetDoubleValue(field.OpenPrice),
                HighestPrice = CTPMarketDataHelper.GetDoubleValue(field.HighestPrice),
                LowestPrice = CTPMarketDataHelper.GetDoubleValue(field.LowestPrice),
                Volume = field.Volume,
                Turnover = CTPMarketDataHelper.GetDoubleValue(field.Turnover),
                OpenInterest = CTPMarketDataHelper.GetDoubleValue(field.OpenInterest),
                ClosePrice = CTPMarketDataHelper.GetDoubleValue(field.ClosePrice),
                SettlementPrice = CTPMarketDataHelper.GetDoubleValue(field.SettlementPrice),

                // HighLimitedPrice = CTPMarketDataHelper.GetDoubleValue(field.UpperLimitPrice),
                // LowLimitedPrice = CTPMarketDataHelper.GetDoubleValue(field.LowerLimitPrice),

                PreDelta = CTPMarketDataHelper.GetDoubleValue(field.PreDelta),
                CurrDelta = CTPMarketDataHelper.GetDoubleValue(field.CurrDelta),
                UpdateTime = field.UpdateTime,
                UpdateMillisec = field.UpdateMillisec,
                BidPrice1 = CTPMarketDataHelper.GetDoubleValue(field.BidPrice1),
                BidVolume1 = field.BidVolume1,
                AskPrice1 = CTPMarketDataHelper.GetDoubleValue(field.AskPrice1),
                AskVolume1 = field.AskVolume1,
                AveragePrice = CTPMarketDataHelper.GetDoubleValue(field.AveragePrice),
                PreOpenInterest = field.PreOpenInterest,

                // LastVolume = (long)field.Volume - lastVolume,
                // TimeStamp = CTPMarketDataHelper.CalculateTimeStamp(field)
            };
        }

        public static void InitPreTradingDay()
        {
            if (DateTime.Now.Hour >= 16)
            {
                CTPMarketDataHelper.PreTradingDay = DateTime.Today;
            }
            else
            {
                CTPMarketDataHelper.PreTradingDay = DateTime.Today.AddDays(-1.0);
            }
        }

        private static double GetDoubleValue(double origValue)
        {
            double result;
            if (origValue >= 1.7976931348623157E+308)
            {
                result = 0.0;
            }
            else
            {
                result = origValue;
            }
            return result;
        }

        private static DateTime CalculateTimeStamp(ThostFtdcDepthMarketDataField field)
        {
            DateTime result = DateTime.MinValue;
            TimeSpan updateTime = CTPMarketDataHelper.GetUpdateTime(field.UpdateTime);
            if (updateTime > CTPMarketDataHelper.nightTimeSpan)
            {
                result = CTPMarketDataHelper.PreTradingDay.Add(updateTime).AddMilliseconds((double)field.UpdateMillisec);
            }
            else if (updateTime > CTPMarketDataHelper.dayTimeSpan)
            {
                result = CTPMarketDataHelper.GetDate(field.TradingDay).Add(updateTime).AddMilliseconds((double)field.UpdateMillisec);
            }
            else
            {
                result = CTPMarketDataHelper.PreTradingDay.AddDays(1.0).Add(updateTime).AddMilliseconds((double)field.UpdateMillisec);
            }
            return result;
        }

        private static TimeSpan GetUpdateTime(string updateTime)
        {
            return TimeSpan.ParseExact(updateTime, "g", CultureInfo.InvariantCulture);
        }

        private static DateTime GetDate(string preTradingDay)
        {
            DateTime result;
            try
            {
                result = DateTime.ParseExact(preTradingDay, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                result = DateTime.Now;
            }
            return result;
        }
    }
}
