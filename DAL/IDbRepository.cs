using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.Models;

namespace WebApplication5.DAL
{
    public interface IDbRepository
    {
        public int GetNoEndStatus(string a, string b);
        public int GetStoresCount(string a, string b);
        public int GetOrderCount(string a, string b);
        public int GetSoldOrdersCount(string a, string b);
        public int GetTimeOutCanceledCount(string a, string b);
        public int CustomerCanceledOrdersCount(string a, string b);
        public int GetCanceledOrdCount(string a, string b);
        public int GetNoReceiveStatusOrd(string a, string b);
       // public void PostNetSettings(UserSettings userSettings);
        public List<MarketNames> GetStoreNames();
        public List<MarketNames> GetEachStoreOrdersCount(string a, string b);
        public List<MarketNames> GetEachStoreCancelOrdersCount(string a, string b);
        public List<MarketNames> GetEachStoreSoldOrdersCount(string a, string b);
        public List<MarketNames> GetEachStoreNoEndStatus(string a, string b);
        public List<MarketNames> GetEachStoreTimeOutCanceledCount(string a, string b);
        public List<MarketNames> CustomerGetEachStoreCanceledOrdersCount(string a, string b);
        public List<MarketNames> GetEachStoreNoReceiveStatusOrd(string a, string b);




    }
}
