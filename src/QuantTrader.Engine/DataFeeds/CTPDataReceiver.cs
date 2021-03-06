﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using NLog;
using CTP;

using QuantTrader.Entities;
using System.IO;

namespace QuantTrader.DataFeeds
{
    public class CTPDataReceiver : IDataReceiver
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private CTPAccountInfo _accountInfo = null;
        private CTPMDAdapter api = new CTPMDAdapter();
        private readonly static ConcurrentQueue<ThostFtdcDepthMarketDataField> _queue = new ConcurrentQueue<ThostFtdcDepthMarketDataField>();
        
        public ConcurrentQueue<ThostFtdcDepthMarketDataField> MarketDataQueue
        {
            get { return _queue; }
        }

        public CTPDataReceiver(CTPAccountInfo accountInfo)
        {
            _accountInfo = accountInfo;
        }

        public void Initialize()
        {
            api.OnFrontConnected += new FrontConnected(OnFrontConnected);
            api.OnFrontDisconnected += new FrontDisconnected(OnFrontDisconnected);
            api.OnHeartBeatWarning += new HeartBeatWarning(OnHeartBeatWarning);
            api.OnRspError += new RspError(OnRspError);
            api.OnRspSubMarketData += new RspSubMarketData(OnRspSubMarketData);
            api.OnRspUnSubMarketData += new RspUnSubMarketData(OnRspUnSubMarketData);
            api.OnRspUserLogin += new RspUserLogin(OnRspUserLogin);
            api.OnRspUserLogout += new RspUserLogout(OnRspUserLogout);
            api.OnRtnDepthMarketData += new RtnDepthMarketData(OnRtnDepthMarketData);
        }

        bool IsErrorRspInfo(ThostFtdcRspInfoField pRspInfo)
        {
            // 如果ErrorID != 0, 说明收到了错误的响应
            bool bResult = ((pRspInfo != null) && (pRspInfo.ErrorID != 0));
            if (bResult)
            {
                _logger.Info("--->>> ErrorID={0}, ErrorMsg={1}", pRspInfo.ErrorID, pRspInfo.ErrorMsg);
            }
                
            return bResult;
        }

        private void ReqUserLogin()
        {
            ThostFtdcReqUserLoginField req = new ThostFtdcReqUserLoginField();
            req.BrokerID = _accountInfo.BrokerID;
            req.UserID = _accountInfo.UserID;
            req.Password = _accountInfo.Password;

            int iResult = api.ReqUserLogin(req);

            _logger.Info("--->>> 发送用户登录请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        private void SubscribeMarketData()
        {
            int iResult = api.SubscribeMarketData(QuantTraderGlobals.GetInstance().QuantTraderConfig.SubscribeMarketDatas.ToArray());
            _logger.Info("--->>> 发送行情订阅请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        private void OnRtnDepthMarketData(ThostFtdcDepthMarketDataField pDepthMarketData)
        {
            // 行情数据添加到队列
            _queue.Enqueue(pDepthMarketData);
        }

        private void OnRspUserLogout(ThostFtdcUserLogoutField pUserLogout, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            // throw new NotImplementedException();
        }

        private void OnRspUserLogin(ThostFtdcRspUserLoginField pRspUserLogin, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && !IsErrorRspInfo(pRspInfo))
            {
                ///获取当前交易日
                _logger.Info("--->>> 获取当前交易日 = " + api.GetTradingDay());
                // 请求订阅行情
                SubscribeMarketData();
            }
        }

        private void OnHeartBeatWarning(int nTimeLapse)
        {
            // throw new NotImplementedException();
        }

        private void OnRspUnSubMarketData(ThostFtdcSpecificInstrumentField pSpecificInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            // throw new NotImplementedException();
        }

        private void OnRspSubMarketData(ThostFtdcSpecificInstrumentField pSpecificInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            // throw new NotImplementedException();
        }

        private void OnRspError(ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            IsErrorRspInfo(pRspInfo);
        }

        private void OnFrontDisconnected(int nReason)
        {
            // throw new NotImplementedException();
        }

        private void OnFrontConnected()
        {
            // throw new NotImplementedException();
            ReqUserLogin();
        }

        public void Run()
        {
            try
            {
                api.RegisterFront(_accountInfo.QuoteServerAddress);
                api.Init();
                // api.Join();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
