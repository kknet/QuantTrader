using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using QuantTrader.Data;
using CTP;

namespace QuantTrader.DataFeeds
{
    public class CTPDataReceiver
    {
    }

    class TestMdUserApi
    {
        // Trade Front：180.168.146.187:10000，Market Front：180.168.146.187:10010

        CTPMDAdapter api = null;
        string FRONT_ADDR = "tcp://180.168.146.187:10010";  // 前置地址
        string BrokerID = "9999";                       // 经纪公司代码
        string UserID = "058207";                       // 投资者代码
        string Password = "160215";                     // 用户密码
        // 大连,上海代码为小写
        // 郑州,中金所代码为大写
        // 郑州品种年份为一位数
        string[] ppInstrumentID = { "cu1701", "ru1705", "ni1705", "rb1705", "y1705", "p1705", "", "i1705", "j1705", "sr705", "cf705" };	// 行情订阅列表
        int iRequestID = 0;

        public void Run()
        {
            api = new CTPMDAdapter();
            api.OnFrontConnected += new FrontConnected(OnFrontConnected);
            api.OnFrontDisconnected += new FrontDisconnected(OnFrontDisconnected);
            api.OnHeartBeatWarning += new HeartBeatWarning(OnHeartBeatWarning);
            api.OnRspError += new RspError(OnRspError);
            api.OnRspSubMarketData += new RspSubMarketData(OnRspSubMarketData);
            api.OnRspUnSubMarketData += new RspUnSubMarketData(OnRspUnSubMarketData);
            api.OnRspUserLogin += new RspUserLogin(OnRspUserLogin);
            api.OnRspUserLogout += new RspUserLogout(OnRspUserLogout);
            api.OnRtnDepthMarketData += new RtnDepthMarketData(OnRtnDepthMarketData);

            try
            {
                api.RegisterFront(FRONT_ADDR);
                api.Init();
                api.Join(); // 阻塞直到关闭或者CTRL+C
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                api.Release();
            }
        }

        void OnRspUserLogout(ThostFtdcUserLogoutField pUserLogout, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            DebugPrintFunc(new StackTrace());
        }

        void OnRspUserLogin(ThostFtdcRspUserLoginField pRspUserLogin, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            DebugPrintFunc(new StackTrace());
            if (bIsLast && !IsErrorRspInfo(pRspInfo))
            {
                ///获取当前交易日
                Console.WriteLine("--->>> 获取当前交易日 = " + api.GetTradingDay());
                // 请求订阅行情
                SubscribeMarketData();
            }
        }

        void OnRspUnSubMarketData(ThostFtdcSpecificInstrumentField pSpecificInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            DebugPrintFunc(new StackTrace());
        }

        void OnRspSubMarketData(ThostFtdcSpecificInstrumentField pSpecificInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            DebugPrintFunc(new StackTrace());
        }

        void OnRspError(ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            DebugPrintFunc(new StackTrace());
            IsErrorRspInfo(pRspInfo);
        }

        void OnHeartBeatWarning(int nTimeLapse)
        {
            DebugPrintFunc(new StackTrace());
            Console.WriteLine("--->>> nTimerLapse = " + nTimeLapse);
        }

        void OnFrontDisconnected(int nReason)
        {
            DebugPrintFunc(new StackTrace());
            Console.WriteLine("--->>> Reason = {0}", nReason);
        }

        void OnFrontConnected()
        {
            DebugPrintFunc(new StackTrace());
            ReqUserLogin();
        }

        bool IsErrorRspInfo(ThostFtdcRspInfoField pRspInfo)
        {
            // 如果ErrorID != 0, 说明收到了错误的响应
            bool bResult = ((pRspInfo != null) && (pRspInfo.ErrorID != 0));
            if (bResult)
                Console.WriteLine("--->>> ErrorID={0}, ErrorMsg={1}", pRspInfo.ErrorID, pRspInfo.ErrorMsg);
            return bResult;
        }

        void ReqUserLogin()
        {
            ThostFtdcReqUserLoginField req = new ThostFtdcReqUserLoginField();
            req.BrokerID = BrokerID;
            req.UserID = UserID;
            req.Password = Password;
            int iResult = api.ReqUserLogin(req);

            Console.WriteLine("--->>> 发送用户登录请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        void SubscribeMarketData()
        {

            int iResult = api.SubscribeMarketData(ppInstrumentID);
            Console.WriteLine("--->>> 发送行情订阅请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        void OnRtnDepthMarketData(ThostFtdcDepthMarketDataField pDepthMarketData)
        {
            //DebugPrintFunc(new StackTrace());

            //DateTime now = DateTime.Parse(pDepthMarketData.UpdateTime);
            //now.AddMilliseconds(pDepthMarketData.UpdateMillisec);
            string s = string.Format("{0,-6} : UpdateTime = {1}.{2:D3},  LasPrice = {3}", pDepthMarketData.InstrumentID, pDepthMarketData.UpdateTime, pDepthMarketData.UpdateMillisec, pDepthMarketData.LastPrice);
            Debug.WriteLine(s);
            Console.WriteLine(s);
        }

        void DebugPrintFunc(StackTrace stkTrace)
        {
            string s = stkTrace.GetFrame(0).ToString();
            s = s.Split(new char[] { ' ' })[0];
            Debug.WriteLine("--->>> " + s);
            Console.WriteLine("--->>> " + s);
        }

    }
}
