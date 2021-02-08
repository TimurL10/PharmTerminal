using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
//using Dapper;
using WebApplication5.DAL;
using WebApplication5.Models;
using System.IO;
using System.Collections;

namespace WebApplication5.DAL
{
    public class DbRepository : IDbRepository
    {
        private string _configuration;
        private string _userConfiguration;
        private string _shardConfiguration;
        public DbRepository(IConfiguration configuration)
        {
           _configuration = configuration.GetValue<string>("DbInfo:ConnectionString");
           _userConfiguration = configuration.GetValue<string>("DbInfo:ConnectionStringUser");
            _shardConfiguration = configuration.GetValue<string>("DbInfo:ConnectionStringShard");
        }

        internal IDbConnection dbConnection { get { return new SqlConnection(_configuration); } }
        internal IDbConnection dbConnectionUser { get { return new SqlConnection(_userConfiguration); } }

        internal IDbConnection dbConnectionShard { get { return new SqlConnection(_shardConfiguration); } }

        public int GetNoEndStatus(string startDate, string endDate)
        {
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            int result = 0;

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number)
                            from[Documents].[OrderHeaders] h
                            inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
                            where h.storeid in
                            (SELECT[TableRowGUID]
                            FROM[References].PN_PharmacySync p
                            JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
                            WHERE n.Real_Net_Guid = {netGuid}
                            AND p.Actual = 1)
                            and h.date between {startDate} and ({endDate}) and s.status = 100
                            and h.orderid not in (select orderid from[Documents].[OrderStatuses] where[status] in (210, 205, 202, 212, 211))
                            HAVING COUNT(h.number) > 0");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = (Int32)reader[0];
                            Debug.WriteLine(result);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return result;
        }
        public int GetStoresCount(string startDate, string endDate)
        {
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            int result = 0;

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"SELECT COUNT(DISTINCT s.StoreId)
                                    FROM [Documents].[OrderStatuses] s 
                                    JOIN [Documents].[OrderHeaders] h ON h.OrderId = s.OrderId
                                    INNER JOIN [References].PN_PharmacySync p ON p.TableRowGUID = h.StoreId
                                    INNER JOIN [Documents].[OrderRows] r ON r.OrderId = s.OrderId 
                                    JOIN [References].[ProductsSync] pr ON pr.id_ISS = r.Nnt
                                    JOIN [References].[UnionNetSync] n ON n.id = p.id_pn_unionnet 
                                    WHERE n.Real_Net_Guid = {netGuid}
                                    AND s.TimeStamp > {startDate}
                                    AND s.TimeStamp < {endDate}
                                    HAVING COUNT(DISTINCT s.StoreId) > 0");
                Debug.WriteLine(sqlCommand);
                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                command.CommandTimeout = 200;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = (Int32)reader[0];
                            Debug.WriteLine(result);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return result;            
        }
        public int GetOrderCount(string start, string end)
        {
            string startDate = "'2021-01-01'";
            string endDate = "'2021-01-04'";
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            int result = 0;

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"SELECT Count(OrderId) AS 'Количество заказов'
                                        FROM [Documents].[OrderRows]
                                        WHERE StoreId in
                                        (SELECT [TableRowGUID]
                                        FROM [References].PN_PharmacySync p 
                                        JOIN [References].[UnionNetSync] n ON n.id = p.id_pn_unionnet 
                                        WHERE n.Real_Net_Guid = {netGuid}
                                        AND p.Actual = 1)
                                        AND Timestamp > {startDate}
                                        AND Timestamp < {endDate}
                                        HAVING Count(OrderId) > 0");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = (Int32)reader[0];
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return result;
        }
        public int GetSoldOrdersCount(string start, string end)
        {
            string startDate = "'2021-01-01'";
            string endDate = "'2021-01-04'";
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            int result = 0;

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number)
                            from[Documents].[OrderHeaders] h
                            inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
                            where h.storeid in
                            (SELECT[TableRowGUID]
                            FROM[References].PN_PharmacySync p
                            JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
                            WHERE n.Real_Net_Guid = {netGuid}
                            AND p.Actual = 1)
                            and h.date between {startDate} and ({endDate}) and s.status = 100
                            and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (210))
                            HAVING COUNT(h.number) > 0");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = (Int32)reader[0];
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return result;
        }
        public int GetTimeOutCanceledCount(string start, string end)
        {
            string startDate = "'2021-01-01'";
            string endDate = "'2021-01-04'";
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            int result = 0;

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number)
                            from[Documents].[OrderHeaders] h
                            inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
                            where h.storeid in
                            (SELECT[TableRowGUID]
                            FROM[References].PN_PharmacySync p
                            JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
                            WHERE n.Real_Net_Guid = {netGuid}
                            AND p.Actual = 1)
                            and h.date between {startDate} and ({endDate}) and s.status = 100
                            and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (205))
                            HAVING COUNT(h.number) > 0");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = (Int32)reader[0];
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return result;
        }
        public int CustomerCanceledOrdersCount(string start, string end)
        {
            string startDate = "'2021-01-01'";
            string endDate = "'2021-01-04'";
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            int result = 0;

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number)
                            from[Documents].[OrderHeaders] h
                            inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
                            where h.storeid in
                            (SELECT[TableRowGUID]
                            FROM[References].PN_PharmacySync p
                            JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
                            WHERE n.Real_Net_Guid = {netGuid}
                            AND p.Actual = 1)
                            and h.date between {startDate} and ({endDate}) and s.status = 100
                            and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (211))
                            HAVING COUNT(h.number) > 0");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = (Int32)reader[0];
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return result;
        }
        public int GetCanceledOrdCount(string start, string end)
        {
            string startDate = "'2021-01-01'";
            string endDate = "'2021-01-04'";
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            int result = 0;

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number)
                            from[Documents].[OrderHeaders] h
                            inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
                            where h.storeid in
                            (SELECT[TableRowGUID]
                            FROM[References].PN_PharmacySync p
                            JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
                            WHERE n.Real_Net_Guid = {netGuid}
                            AND p.Actual = 1)
                            and h.date between {startDate} and ({endDate}) and s.status = 100
                            and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (212,202))
                            HAVING COUNT(h.number) > 0");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = (Int32)reader[0];
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return result;
        }
        public int GetNoReceiveStatusOrd(string start, string end)
        {
            string startDate = "'2021-01-01'";
            string endDate = "'2021-01-04'";
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            int result = 0;

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number)
                            from[Documents].[OrderHeaders] h
                            inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
                            where h.storeid in
                            (SELECT[TableRowGUID]
                            FROM[References].PN_PharmacySync p
                            JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
                            WHERE n.Real_Net_Guid = {netGuid}
                            AND p.Actual = 1)
                            and h.date between {startDate} and ({endDate}) and s.status = 100
                            and h.orderid not in (select orderid from[Documents].[OrderStatuses] where[status] in (200,211,202,201))
                            HAVING COUNT(h.number) > 0");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = (Int32)reader[0];
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return result;
        }
        //public void PostNetSettings(UserSettings userSettings)
        //{
        //    using (IDbConnection connection = dbConnectionUser)
        //    {
        //        connection.Execute("");
        //    } 
        //}

        public List<MarketNames> GetStoreNames()
        {
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            List<MarketNames> marketNames = new List<MarketNames>();
            using (IDbConnection connection = dbConnectionShard)
            {
                string SqlCommand = ($@"SELECT p.TableRowGUID, NameFull 
                                        FROM[References].PN_PharmacySync p
                                        JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
                                        WHERE n.Real_Net_Guid = {netGuid}
                                        AND p.Actual = 1");


                SqlCommand command = new SqlCommand(SqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.Text;
                SqlParameter parameter = new SqlParameter();
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var market = new MarketNames((Guid)reader[0], (String)reader[1]);
                            marketNames.Add(market);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return marketNames;
        }       

        public List<MarketNames> GetEachStoreOrdersCount(string start, string end)
        {
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            List<MarketNames> marketNames = new List<MarketNames>();
            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"SELECT COUNT(h.number), p.NameFull
                                        FROM [Documents].[OrderHeaders] h
                                        INNER JOIN [References].PN_PharmacySync p ON p.TableRowGUID = h.StoreId
                                        JOIN [References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
                                        WHERE p.TableRowGUID in (SELECT [TableRowGUID]
                                        FROM [References].PN_PharmacySync p 
                                        JOIN [References].[UnionNetSync] n ON n.id = p.id_pn_unionnet 
                                        WHERE n.Real_Net_Guid = {netGuid}
                                        AND p.Actual = 1)
                                        AND h.timestamp  between {start} and ({end})
                                        GROUP BY p.NameFull");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var market = new MarketNames((Int32)reader[0], (String)reader[1]);
                            marketNames.Add(market);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return marketNames;
        }
        public List<MarketNames> GetEachStoreCancelOrdersCount(string start, string end)
        {
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            List<MarketNames> MarketNames = new List<MarketNames>();

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number), h.StoreId
                                        from [Documents].[OrderHeaders] h
                                        inner join [Documents].[OrderStatuses] s on h.orderid = s.orderid
                                        where h.storeid in 
                                        (SELECT [TableRowGUID]
                                        FROM [References].PN_PharmacySync p 
                                        JOIN [References].[UnionNetSync] n ON n.id = p.id_pn_unionnet 
                                        WHERE n.Real_Net_Guid = {netGuid}
                                        AND p.Actual = 1)

                                        and h.date between {start} and {end} and s.status = 100
                                        and h.orderid in (select orderid from [Documents].[OrderStatuses] where [status] in (202,212) )
                                        GROUP BY h.StoreId");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var market = new MarketNames((Int32)reader[0], (Guid)reader[1]);
                            MarketNames.Add(market);

                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return MarketNames;
        }
        public List<MarketNames> GetEachStoreSoldOrdersCount(string start, string end)
        {
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            List<MarketNames> MarketNames = new List<MarketNames>();

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number), h.StoreId
                                        from [Documents].[OrderHeaders] h
                                        inner join [Documents].[OrderStatuses] s on h.orderid = s.orderid
                                        where h.storeid in 
                                        (SELECT [TableRowGUID]
                                        FROM [References].PN_PharmacySync p 
                                        JOIN [References].[UnionNetSync] n ON n.id = p.id_pn_unionnet 
                                        WHERE n.Real_Net_Guid = {netGuid}
                                        AND p.Actual = 1)

                                        and h.date between {start} and {end} and s.status = 100
                                        and h.orderid in (select orderid from [Documents].[OrderStatuses] where [status] in (210) )
                                        GROUP BY h.StoreId");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var market = new MarketNames((Int32)reader[0], (Guid)reader[1]);
                            MarketNames.Add(market);

                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return MarketNames;
        }
        public List<MarketNames> GetEachStoreNoEndStatus(string start, string end)
        {
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            List<MarketNames> MarketNames = new List<MarketNames>();

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number), h.StoreId
                                        from [Documents].[OrderHeaders] h
                                        inner join [Documents].[OrderStatuses] s on h.orderid = s.orderid
                                        where h.storeid in 
                                        (SELECT [TableRowGUID]
                                        FROM [References].PN_PharmacySync p 
                                        JOIN [References].[UnionNetSync] n ON n.id = p.id_pn_unionnet 
                                        WHERE n.Real_Net_Guid = {netGuid}
                                        AND p.Actual = 1)

                                        and h.date between {start} and {end} and s.status = 100
                                        and h.orderid not in (select orderid from [Documents].[OrderStatuses] where [status] in (202,212,205,211,210) )
                                        GROUP BY h.StoreId");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var market = new MarketNames((Int32)reader[0], (Guid)reader[1]);
                            MarketNames.Add(market);

                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return MarketNames;
        }
        public List<MarketNames> GetEachStoreTimeOutCanceledCount(string start, string end)
        {
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            List<MarketNames> MarketNames = new List<MarketNames>();

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number), h.StoreId
                                        from [Documents].[OrderHeaders] h
                                        inner join [Documents].[OrderStatuses] s on h.orderid = s.orderid
                                        where h.storeid in 
                                        (SELECT [TableRowGUID]
                                        FROM [References].PN_PharmacySync p 
                                        JOIN [References].[UnionNetSync] n ON n.id = p.id_pn_unionnet 
                                        WHERE n.Real_Net_Guid = {netGuid}
                                        AND p.Actual = 1)

                                        and h.date between {start} and {end} and s.status = 100
                                        and h.orderid in (select orderid from [Documents].[OrderStatuses] where [status] in (205) )
                                        GROUP BY h.StoreId");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var market = new MarketNames((Int32)reader[0], (Guid)reader[1]);
                            MarketNames.Add(market);

                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return MarketNames;
        }
        public List<MarketNames> CustomerGetEachStoreCanceledOrdersCount(string start, string end)
        {
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            List<MarketNames> MarketNames = new List<MarketNames>();

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number), h.StoreId
                                        from [Documents].[OrderHeaders] h
                                        inner join [Documents].[OrderStatuses] s on h.orderid = s.orderid
                                        where h.storeid in 
                                        (SELECT [TableRowGUID]
                                        FROM [References].PN_PharmacySync p 
                                        JOIN [References].[UnionNetSync] n ON n.id = p.id_pn_unionnet 
                                        WHERE n.Real_Net_Guid = {netGuid}
                                        AND p.Actual = 1)

                                        and h.date between {start} and {end} and s.status = 100
                                        and h.orderid in (select orderid from [Documents].[OrderStatuses] where [status] in (211) )
                                        GROUP BY h.StoreId");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var market = new MarketNames((Int32)reader[0], (Guid)reader[1]);
                            MarketNames.Add(market);

                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return MarketNames;
        }
        public List<MarketNames> GetEachStoreNoReceiveStatusOrd(string start, string end)
        {
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            List<MarketNames> MarketNames = new List<MarketNames>();

            using (IDbConnection connection = dbConnection)
            {
                string spName = "[Monitoring].[ExecQueryShards]";
                string sqlCommand = ($@"select COUNT(h.number), h.StoreId
                                        from [Documents].[OrderHeaders] h
                                        inner join [Documents].[OrderStatuses] s on h.orderid = s.orderid
                                        where h.storeid in 
                                        (SELECT [TableRowGUID]
                                        FROM [References].PN_PharmacySync p 
                                        JOIN [References].[UnionNetSync] n ON n.id = p.id_pn_unionnet 
                                        WHERE n.Real_Net_Guid = {netGuid}
                                        AND p.Actual = 1)
                                        and h.date between {start} and {end} and s.status = 100
                                        and h.orderid not in (select orderid from [Documents].[OrderStatuses] where [status] in (200,201,202,211) )
                                        GROUP BY h.StoreId");

                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var market = new MarketNames((Int32)reader[0], (Guid)reader[1]);
                            MarketNames.Add(market);

                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return MarketNames;        
        }
        public ArrayList GetThreeMonthSoldOrd()
        {
            ArrayList listPoints = new ArrayList();
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";
            
            using (IDbConnection connection = dbConnection)
            {
                string sqlCommand = File.ReadAllText(@"C:\Users\t.lumelsky\source\repos\PharmTerminal\getThreeMonthsSold.sql");
                string spName = "[Monitoring].[ExecQueryShards]"; 
                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 500;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            listPoints.AddRange(new int[] {(Int32)reader[0], (Int32)reader[1], (Int32)reader[2], (Int32)reader[3],
                                (Int32)reader[4], (Int32)reader[5], (Int32)reader[6], (Int32)reader[7], (Int32)reader[8], (Int32)reader[9],
                                (Int32)reader[10], (Int32)reader[11] });                          
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return listPoints;
        }

        public ArrayList GetThreeMonthCanceled()
        {
            ArrayList listPoints = new ArrayList();
            string netGuid = "'efb05410-ba92-4a73-a37f-f05f9a499ded'";

            using (IDbConnection connection = dbConnection)
            {
                string sqlCommand = File.ReadAllText(@"C:\Users\t.lumelsky\source\repos\PharmTerminal\getThreeMonthsCanceled.sql");
                string spName = "[Monitoring].[ExecQueryShards]";
                SqlCommand command = new SqlCommand(sqlCommand, (SqlConnection)connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 500;
                command.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@text";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = sqlCommand;
                command.Parameters.AddWithValue("@text", sqlCommand);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            listPoints.AddRange(new int[] {(Int32)reader[0], (Int32)reader[1], (Int32)reader[2], (Int32)reader[3],
                                (Int32)reader[4], (Int32)reader[5], (Int32)reader[6], (Int32)reader[7], (Int32)reader[8], (Int32)reader[9],
                                (Int32)reader[10], (Int32)reader[11] });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return listPoints;
        }
    }
}
