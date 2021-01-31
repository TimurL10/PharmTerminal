using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.DAL;

namespace WebApplication5.Models
{
    
    public class ReportByNet
    {
        public string StoreName { get; set; }

        public int StoresCount { get; set; }

        public int OrdersCount { get; set; }

        public int SoldOrders { get; set; }

        public int TimeOutCanceled { get; set; }

        public int CustomerCanceledOrders { get; set; }

        public int NoEndStatusOrd { get; set; }

        public int CanceledOrd { get; set; }

        public int NoReceiveStatusOrd { get; set; }

        public ReportByNet(int storesCount, int ordersCount, int soldOrders, int timeOutCanceled, int customerCanceledOrders, int noEndStatusOrd, int canceledOrd, int noReceiveStatusOrd)
        {
            StoreName = String.Empty;
            StoresCount = storesCount;
            OrdersCount = ordersCount;
            SoldOrders = soldOrders;
            TimeOutCanceled = timeOutCanceled;
            CustomerCanceledOrders = customerCanceledOrders;
            NoEndStatusOrd = noEndStatusOrd;
            CanceledOrd = canceledOrd;
            NoReceiveStatusOrd = noReceiveStatusOrd;
        }

        public ReportByNet(string storeName, int ordersCount, int soldOrders, int timeOutCanceled, int customerCanceledOrders, int noEndStatusOrd, int canceledOrd, int noReceiveStatusOrd)
        {
            StoreName = storeName;
            OrdersCount = ordersCount;
            SoldOrders = soldOrders;
            TimeOutCanceled = timeOutCanceled;
            CustomerCanceledOrders = customerCanceledOrders;
            NoEndStatusOrd = noEndStatusOrd;
            CanceledOrd = canceledOrd;
            NoReceiveStatusOrd = noReceiveStatusOrd;
        }
    }
}
