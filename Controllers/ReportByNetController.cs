using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.DAL;
using WebApplication5.Models;
using System.Collections;


namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReportByNetController : Controller
    {
        private IDbRepository _dbRepository;
        public ReportByNetController(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ReportByNet Get(string start, string end)
        {

            start = InsertQuotes(ParseDate(start));
            end = InsertQuotes(ParseDate(end));

            ReportByNet report = new ReportByNet(
                _dbRepository.GetStoresCount(start, end),
                _dbRepository.GetOrderCount(start, end),
                _dbRepository.GetSoldOrdersCount(start, end),
                _dbRepository.GetTimeOutCanceledCount(start, end),
                _dbRepository.CustomerCanceledOrdersCount(start, end),
                _dbRepository.GetNoEndStatus(start, end),
                _dbRepository.GetCanceledOrdCount(start, end),
                _dbRepository.GetNoReceiveStatusOrd(start, end));
            return report;
        }

        public string ParseDate(string date)
        {
            var ParseDate = DateTime.Parse(date.Remove(15));
            ParseDate = new DateTime(ParseDate.Year, ParseDate.Month, ParseDate.Day);
            var formattedDate = ParseDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return formattedDate;
        }

        public string InsertQuotes(string stringToShift)
        {
            if (stringToShift != null)
            {
                stringToShift = stringToShift.Insert(0, "'");
                stringToShift = stringToShift.Insert(stringToShift.Length, "'");
                return stringToShift;
            }
            return null;
        }

        [HttpGet]
        public List<ReportByNet> GetReportForStores(string start, string end)
        {

            //start = "Fri Jan 01 2021 00:00:00 GMT 0300(Москва, стандартное время)";
            //end = "Fri Jan 08 2021 00:00:00 GMT 0300(Москва, стандартное время)";


            List<ReportByNet> reportByNets = new List<ReportByNet>();
            start = InsertQuotes(ParseDate(start));
            end = InsertQuotes(ParseDate(end));
            var stores = _dbRepository.GetStoreNames();
            var orders = _dbRepository.GetEachStoreOrdersCount(start, end);
            var canceledOrderd = _dbRepository.GetEachStoreCancelOrdersCount(start, end);
            var soldOrders = _dbRepository.GetEachStoreSoldOrdersCount(start, end);
            var noEndStatus = _dbRepository.GetEachStoreNoEndStatus(start, end);
            var timeoutCanceledOrders = _dbRepository.GetEachStoreTimeOutCanceledCount(start, end);
            var customerCancelesOrders = _dbRepository.CustomerGetEachStoreCanceledOrdersCount(start, end);
            var noReceiveStatusOrd = _dbRepository.GetEachStoreNoReceiveStatusOrd(start, end);


            var result = (from t in stores
                          join o in orders on t.NameFull equals o.NameFull into one
                          from suborders in one.DefaultIfEmpty()
                          join c in canceledOrderd on t.TableRowGUID equals c.TableRowGUID into two
                          from subcancaledorder in two.DefaultIfEmpty()
                          join s in soldOrders on t.TableRowGUID equals s.TableRowGUID into three
                          from subsoldorders in three.DefaultIfEmpty()
                          join n in noEndStatus on t.TableRowGUID equals n.TableRowGUID into four
                          from subnoendstatuses in four.DefaultIfEmpty()
                          join ti in timeoutCanceledOrders on t.TableRowGUID equals ti.TableRowGUID into five
                          from subtimeoutcanceledord in five.DefaultIfEmpty()
                          join cu in customerCancelesOrders on t.TableRowGUID equals cu.TableRowGUID into six
                          from subcuscanceledord in six.DefaultIfEmpty()
                          join no in noReceiveStatusOrd on t.TableRowGUID equals no.TableRowGUID into seven
                          from subnoreseivestord in seven.DefaultIfEmpty()
                          select new
                          {
                              NameFull = t?.NameFull ?? String.Empty,
                              OrdersCount = suborders?.Count ?? 0,
                              CanceledOrders = subcancaledorder?.Count ?? 0,
                              SoldOrders = subsoldorders?.Count ?? 0,
                              NoEndStatus = subnoendstatuses?.Count ?? 0,
                              TimeOutCanceledOrders = subtimeoutcanceledord?.Count ?? 0,
                              CusCanceledOrd = subcuscanceledord?.Count ?? 0,
                              noRecieveOrdStatus = subnoreseivestord?.Count ?? 0
                          });

            foreach (var r in result)
            {
                ReportByNet reportByNet = new ReportByNet(r.NameFull, r.OrdersCount, r.SoldOrders,
                    r.TimeOutCanceledOrders, r.CusCanceledOrd, r.NoEndStatus, r.CanceledOrders, r.noRecieveOrdStatus);
                reportByNets.Add(reportByNet);
                Console.WriteLine("This: " + "" + reportByNet.StoreName);
            }
            return reportByNets;
        }

        public Hashtable GetDateForAreaChart()
        {
            Hashtable sendHashList = new Hashtable();
            //Hashtable chart = new Hashtable()
            //{
            //    {"chart",new Dictionary<string,string>(){ { "type","area"} } }
            //};            
            //Hashtable title = new Hashtable()
            //{
            //    {"title",new Dictionary<string,string>(){ { "text", "Данные по аптечной сети" } } }
            //};
            //Hashtable xAxis = new Hashtable()
            //{
            //    {"xAxis",new Dictionary<string,string[]>(){ { "text", new string[] { "Декабрь", "Январь", "Февраль" } } } }
            //};
            //Hashtable credits = new Hashtable()
            //{
            //    {"credits",new Dictionary<string,bool>(){ { "enabled", false} } }
            //};
            //Hashtable exporting = new Hashtable()
            //{
            //    {"exporting",new Dictionary<string,bool>(){ { "enabled", false} } }
            //};
            Hashtable series2 = new Hashtable()
            {
                { "data", _dbRepository.GetThreeMonthPoints() }
            };
            
            series2.Add( "name", "выкупленные заказы" );

            sendHashList.Add("chart", new Dictionary<string, string>() { { "type", "area" } });
            sendHashList.Add("title", new Dictionary<string, string>() { { "text", "Данные по аптечной сети" } });
            sendHashList.Add("xAxis", new Dictionary<string, string[]>() { { "text", new string[] { "Декабрь", "Январь", "Февраль" } } });
            sendHashList.Add("credits", new Dictionary<string, bool>() { { "enabled", false } });
            sendHashList.Add("exporting", new Dictionary<string, bool>() { { "enabled", false } });
            sendHashList.Add("series", new ArrayList() { series2 });

            return sendHashList;            
        }        
        
    }
    
    
}
