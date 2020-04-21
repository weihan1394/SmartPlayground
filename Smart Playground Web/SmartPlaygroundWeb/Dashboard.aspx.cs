using SmartPlaygroundWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartPlaygroundWeb
{
    public partial class Dashboard : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static List<GraphData> GetGame1ScoreChartData(string selectedKidId, string selectedDate)
        {
            if ((selectedDate != "") && (selectedDate != ""))
            {


                Console.WriteLine(selectedKidId + selectedDate);

                List<Game1RecordDB> lsGame1Record = new List<Game1RecordDB>();
                List<Game2RecordDB> lsGame2Record = new List<Game2RecordDB>();

                int kidId = int.Parse(selectedKidId);
                DateTime date = DateTime.Parse(selectedDate);
                DateTime datePlus = date.AddDays(1);

                string connetionString = null;
                SqlConnection connection;
                SqlCommand command;
                string sql = null;
                SqlDataReader dataReader;
                connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;

                if (kidId > 0)
                {
                    // createdDate >= '2013-06-01' and createdDate < '2013-06-02' 
                    // GET game 1 record
                    // SELECT * FROM  [dbo].[Game1Record] WHERE [timestamp] >= '2019-04-15' AND [timestamp] < '2019-04-16';
                    sql = "SELECT * FROM [Game1Record] WHERE kidId=@kidId AND timestamp >= @dateStart AND timestamp < @dateEnd";
                    connection = new SqlConnection(connetionString);
                    try
                    {
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@kidId", kidId);
                        command.Parameters.AddWithValue("@dateStart", date);
                        command.Parameters.AddWithValue("@dateEnd", datePlus);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            Game1RecordDB currGame1RecordDB = new Game1RecordDB();
                            currGame1RecordDB.KidId = int.Parse(dataReader["kidId"].ToString());
                            currGame1RecordDB.Score = int.Parse(dataReader["score"].ToString());
                            currGame1RecordDB.WrongColor = int.Parse(dataReader["wrongColor"].ToString());
                            currGame1RecordDB.BoardHit = int.Parse(dataReader["boardHit"].ToString());
                            currGame1RecordDB.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());

                            DateTime currGameDateTime = DateTime.Parse(dataReader["timestamp"].ToString());
                            lsGame1Record.Add(currGame1RecordDB);
                        }
                        dataReader.Close();
                        command.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message.ToString();
                    }


                    // GET game 2 record
                    sql = "SELECT * FROM [Game2Record] WHERE kidId=@kidId AND timestamp >= @dateStart AND timestamp < @dateEnd";
                    connection = new SqlConnection(connetionString);
                    try
                    {
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@kidId", kidId);
                        command.Parameters.AddWithValue("@dateStart", date);
                        command.Parameters.AddWithValue("@dateEnd", datePlus);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            //                private int kidId;
                            //private int score;
                            //private int wrongColor;
                            //private int boardHit;
                            //private string timestamp;

                            Game2RecordDB currGame2RecordDB = new Game2RecordDB();
                            currGame2RecordDB.KidId = int.Parse(dataReader["kidId"].ToString());
                            currGame2RecordDB.Score = int.Parse(dataReader["score"].ToString());
                            currGame2RecordDB.MissHit = int.Parse(dataReader["missHit"].ToString());
                            currGame2RecordDB.Power = int.Parse(dataReader["power"].ToString());
                            currGame2RecordDB.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());

                            DateTime currGameDateTime = DateTime.Parse(dataReader["timestamp"].ToString());
                            lsGame2Record.Add(currGame2RecordDB);
                        }
                        dataReader.Close();
                        command.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message.ToString();
                    }
                }


                // sort the list by time
                lsGame1Record.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                lsGame2Record.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));

                // prepare game1 data
                DataTable dtGame1 = new DataTable();
                dtGame1.Columns.AddRange(new DataColumn[] {
                        new DataColumn("w", typeof(string)),
                        new DataColumn("x", typeof(int)),
                        new DataColumn("y",typeof(int)),
                        new DataColumn("z",typeof(int))});

                foreach (Game1RecordDB currGame1RecordDB in lsGame1Record)
                {
                    // 2012-02-24 15:00:00

                    dtGame1.Rows.Add(currGame1RecordDB.Timestamp.ToString("yyyy-mm-dd hh:mm:ss"), currGame1RecordDB.Score, currGame1RecordDB.Score, currGame1RecordDB.Score);
                }

                List<GraphData> chartData = new List<GraphData>();
                foreach (DataRow dr in dtGame1.Rows)
                {
                    chartData.Add(new GraphData
                    {
                        w = Convert.ToString(dr["w"]),
                        x = (Convert.ToInt32(dr["x"])),
                        y = (Convert.ToInt32(dr["y"])),
                        z = (Convert.ToInt32(dr["z"]))
                    });
                }

                return chartData;
            }

            return null;

            //// Get Data from Database.
            //DataTable dt = new DataTable();
            //dt.Columns.AddRange(new DataColumn[] {
            //            new DataColumn("w", typeof(string)),
            //            new DataColumn("x", typeof(int)),
            //            new DataColumn("y",typeof(int)),
            //            new DataColumn("z",typeof(int))});



            //dt.Rows.Add("2011 Q1", 2, 0, 0);
            //dt.Rows.Add("2011 Q2", 50, 15, 5);
            //dt.Rows.Add("2011 Q3", 15, 50, 23);
            //dt.Rows.Add("2011 Q4", 45, 12, 7);
            //dt.Rows.Add("2011 Q5", 20, 32, 55);
            //dt.Rows.Add("2011 Q6", 39, 67, 20);
            //dt.Rows.Add("2011 Q7", 20, 9, 5);
            //List<GraphData> chartData = new List<GraphData>();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    chartData.Add(new GraphData
            //    {
            //        w = Convert.ToString(dr["w"]),
            //        x = (Convert.ToInt32(dr["x"])),
            //        y = (Convert.ToInt32(dr["y"])),
            //        z = (Convert.ToInt32(dr["z"]))
            //    });
            //}

            //return chartData;
        }

        [WebMethod]
        public static List<GraphData> GetGame1WrongColorChartData(string selectedKidId, string selectedDate)
        {
            if ((selectedDate != "") && (selectedDate != ""))
            {
                Console.WriteLine(selectedKidId + selectedDate);

                List<Game1RecordDB> lsGame1Record = new List<Game1RecordDB>();
                List<Game2RecordDB> lsGame2Record = new List<Game2RecordDB>();

                int kidId = int.Parse(selectedKidId);
                DateTime date = DateTime.Parse(selectedDate);
                DateTime datePlus = date.AddDays(1);

                string connetionString = null;
                SqlConnection connection;
                SqlCommand command;
                string sql = null;
                SqlDataReader dataReader;
                connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;

                if (kidId > 0)
                {
                    // createdDate >= '2013-06-01' and createdDate < '2013-06-02' 
                    // GET game 1 record
                    // SELECT * FROM  [dbo].[Game1Record] WHERE [timestamp] >= '2019-04-15' AND [timestamp] < '2019-04-16';
                    sql = "SELECT * FROM [Game1Record] WHERE kidId=@kidId AND timestamp >= @dateStart AND timestamp < @dateEnd";
                    connection = new SqlConnection(connetionString);
                    try
                    {
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@kidId", kidId);
                        command.Parameters.AddWithValue("@dateStart", date);
                        command.Parameters.AddWithValue("@dateEnd", datePlus);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            Game1RecordDB currGame1RecordDB = new Game1RecordDB();
                            currGame1RecordDB.KidId = int.Parse(dataReader["kidId"].ToString());
                            currGame1RecordDB.Score = int.Parse(dataReader["score"].ToString());
                            currGame1RecordDB.WrongColor = int.Parse(dataReader["wrongColor"].ToString());
                            currGame1RecordDB.BoardHit = int.Parse(dataReader["boardHit"].ToString());
                            currGame1RecordDB.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());

                            DateTime currGameDateTime = DateTime.Parse(dataReader["timestamp"].ToString());
                            lsGame1Record.Add(currGame1RecordDB);
                        }
                        dataReader.Close();
                        command.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message.ToString();
                    }
                }

                // sort the list by time
                lsGame1Record.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                lsGame2Record.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));

                // prepare game1 data
                DataTable dtGame1 = new DataTable();
                dtGame1.Columns.AddRange(new DataColumn[] {
                        new DataColumn("w", typeof(string)),
                        new DataColumn("x", typeof(int)),
                        new DataColumn("y",typeof(int)),
                        new DataColumn("z",typeof(int))});

                foreach (Game1RecordDB currGame1RecordDB in lsGame1Record)
                {
                    dtGame1.Rows.Add(currGame1RecordDB.Timestamp, currGame1RecordDB.WrongColor, currGame1RecordDB.WrongColor, currGame1RecordDB.WrongColor);
                }

                List<GraphData> chartData = new List<GraphData>();
                foreach (DataRow dr in dtGame1.Rows)
                {
                    chartData.Add(new GraphData
                    {
                        w = Convert.ToString(dr["w"]),
                        x = (Convert.ToInt32(dr["x"])),
                        y = (Convert.ToInt32(dr["y"])),
                        z = (Convert.ToInt32(dr["z"]))
                    });
                }

                return chartData;
            }

            return null;
        }

        [WebMethod]
        public static List<GraphData> GetGame1BoardHitChartData(string selectedKidId, string selectedDate)
        {
            if ((selectedDate != "") && (selectedDate != ""))
            {
                Console.WriteLine(selectedKidId + selectedDate);

                List<Game1RecordDB> lsGame1Record = new List<Game1RecordDB>();
                List<Game2RecordDB> lsGame2Record = new List<Game2RecordDB>();

                int kidId = int.Parse(selectedKidId);
                DateTime date = DateTime.Parse(selectedDate);
                DateTime datePlus = date.AddDays(1);

                string connetionString = null;
                SqlConnection connection;
                SqlCommand command;
                string sql = null;
                SqlDataReader dataReader;
                connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;

                if (kidId > 0)
                {
                    // createdDate >= '2013-06-01' and createdDate < '2013-06-02' 
                    // GET game 1 record
                    // SELECT * FROM  [dbo].[Game1Record] WHERE [timestamp] >= '2019-04-15' AND [timestamp] < '2019-04-16';
                    sql = "SELECT * FROM [Game1Record] WHERE kidId=@kidId AND timestamp >= @dateStart AND timestamp < @dateEnd";
                    connection = new SqlConnection(connetionString);
                    try
                    {
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@kidId", kidId);
                        command.Parameters.AddWithValue("@dateStart", date);
                        command.Parameters.AddWithValue("@dateEnd", datePlus);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            Game1RecordDB currGame1RecordDB = new Game1RecordDB();
                            currGame1RecordDB.KidId = int.Parse(dataReader["kidId"].ToString());
                            currGame1RecordDB.Score = int.Parse(dataReader["score"].ToString());
                            currGame1RecordDB.WrongColor = int.Parse(dataReader["wrongColor"].ToString());
                            currGame1RecordDB.BoardHit = int.Parse(dataReader["boardHit"].ToString());
                            currGame1RecordDB.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());

                            DateTime currGameDateTime = DateTime.Parse(dataReader["timestamp"].ToString());
                            lsGame1Record.Add(currGame1RecordDB);
                        }
                        dataReader.Close();
                        command.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message.ToString();
                    }
                }

                // sort the list by time
                lsGame1Record.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                lsGame2Record.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));

                // prepare game1 data
                DataTable dtGame1 = new DataTable();
                dtGame1.Columns.AddRange(new DataColumn[] {
                        new DataColumn("w", typeof(string)),
                        new DataColumn("x", typeof(int)),
                        new DataColumn("y",typeof(int)),
                        new DataColumn("z",typeof(int))});

                foreach (Game1RecordDB currGame1RecordDB in lsGame1Record)
                {
                    dtGame1.Rows.Add(currGame1RecordDB.Timestamp, currGame1RecordDB.BoardHit, currGame1RecordDB.BoardHit, currGame1RecordDB.BoardHit);
                }

                List<GraphData> chartData = new List<GraphData>();
                foreach (DataRow dr in dtGame1.Rows)
                {
                    chartData.Add(new GraphData
                    {
                        w = Convert.ToString(dr["w"]),
                        x = (Convert.ToInt32(dr["x"])),
                        y = (Convert.ToInt32(dr["y"])),
                        z = (Convert.ToInt32(dr["z"]))
                    });
                }

                return chartData;
            }

            return null;
        }

        [WebMethod]
        public static List<GraphData> GetGame2ScoreChartData(string selectedKidId, string selectedDate)
        {
            if ((selectedDate != "") && (selectedDate != ""))
            {
                Console.WriteLine(selectedKidId + selectedDate);

                List<Game2RecordDB> lsGame2Record = new List<Game2RecordDB>();

                int kidId = int.Parse(selectedKidId);
                DateTime date = DateTime.Parse(selectedDate);
                DateTime datePlus = date.AddDays(1);

                string connetionString = null;
                SqlConnection connection;
                SqlCommand command;
                string sql = null;
                SqlDataReader dataReader;
                connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;

                if (kidId > 0)
                {
                    // createdDate >= '2013-06-01' and createdDate < '2013-06-02' 
                    // GET game 1 record
                    // SELECT * FROM  [dbo].[Game1Record] WHERE [timestamp] >= '2019-04-15' AND [timestamp] < '2019-04-16';
                    sql = "SELECT * FROM [Game2Record] WHERE kidId=@kidId AND timestamp >= @dateStart AND timestamp < @dateEnd";
                    connection = new SqlConnection(connetionString);
                    try
                    {
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@kidId", kidId);
                        command.Parameters.AddWithValue("@dateStart", date);
                        command.Parameters.AddWithValue("@dateEnd", datePlus);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            Game2RecordDB currGame2RecordDB = new Game2RecordDB();
                            currGame2RecordDB.KidId = int.Parse(dataReader["kidId"].ToString());
                            currGame2RecordDB.Score = int.Parse(dataReader["score"].ToString());
                            currGame2RecordDB.MissHit = int.Parse(dataReader["missHit"].ToString());
                            currGame2RecordDB.Power = int.Parse(dataReader["power"].ToString());
                            currGame2RecordDB.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());

                            DateTime currGameDateTime = DateTime.Parse(dataReader["timestamp"].ToString());
                            lsGame2Record.Add(currGame2RecordDB);
                        }
                        dataReader.Close();
                        command.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message.ToString();
                    }
                }

                // sort the list by time
                lsGame2Record.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));

                // prepare game1 data
                DataTable dtGame2 = new DataTable();
                dtGame2.Columns.AddRange(new DataColumn[] {
                        new DataColumn("w", typeof(string)),
                        new DataColumn("x", typeof(int)),
                        new DataColumn("y",typeof(int)),
                        new DataColumn("z",typeof(int))});

                foreach (Game2RecordDB currGame2RecordDB in lsGame2Record)
                {
                    dtGame2.Rows.Add(currGame2RecordDB.Timestamp.ToString("yyyy-mm-dd hh:mm:ss"), currGame2RecordDB.Score, currGame2RecordDB.Score, currGame2RecordDB.Score);
                }

                List<GraphData> chartData = new List<GraphData>();
                foreach (DataRow dr in dtGame2.Rows)
                {
                    chartData.Add(new GraphData
                    {
                        w = Convert.ToString(dr["w"]),
                        x = (Convert.ToInt32(dr["x"])),
                        y = (Convert.ToInt32(dr["y"])),
                        z = (Convert.ToInt32(dr["z"]))
                    });
                }

                return chartData;
            }

            return null;
        }

        [WebMethod]
        public static List<GraphData> GetGame2MissHitChartData(string selectedKidId, string selectedDate)
        {
            if ((selectedDate != "") && (selectedDate != ""))
            {
                Console.WriteLine(selectedKidId + selectedDate);

                List<Game2RecordDB> lsGame2Record = new List<Game2RecordDB>();

                int kidId = int.Parse(selectedKidId);
                DateTime date = DateTime.Parse(selectedDate);
                DateTime datePlus = date.AddDays(1);

                string connetionString = null;
                SqlConnection connection;
                SqlCommand command;
                string sql = null;
                SqlDataReader dataReader;
                connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;

                if (kidId > 0)
                {
                    // createdDate >= '2013-06-01' and createdDate < '2013-06-02' 
                    // GET game 1 record
                    // SELECT * FROM  [dbo].[Game1Record] WHERE [timestamp] >= '2019-04-15' AND [timestamp] < '2019-04-16';
                    sql = "SELECT * FROM [Game2Record] WHERE kidId=@kidId AND timestamp >= @dateStart AND timestamp < @dateEnd";
                    connection = new SqlConnection(connetionString);
                    try
                    {
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@kidId", kidId);
                        command.Parameters.AddWithValue("@dateStart", date);
                        command.Parameters.AddWithValue("@dateEnd", datePlus);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            Game2RecordDB currGame2RecordDB = new Game2RecordDB();
                            currGame2RecordDB.KidId = int.Parse(dataReader["kidId"].ToString());
                            currGame2RecordDB.Score = int.Parse(dataReader["score"].ToString());
                            currGame2RecordDB.MissHit = int.Parse(dataReader["missHit"].ToString());
                            currGame2RecordDB.Power = int.Parse(dataReader["power"].ToString());
                            currGame2RecordDB.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());

                            DateTime currGameDateTime = DateTime.Parse(dataReader["timestamp"].ToString());
                            lsGame2Record.Add(currGame2RecordDB);
                        }
                        dataReader.Close();
                        command.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message.ToString();
                    }
                }

                // sort the list by time
                lsGame2Record.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));

                // prepare game1 data
                DataTable dtGame2 = new DataTable();
                dtGame2.Columns.AddRange(new DataColumn[] {
                        new DataColumn("w", typeof(string)),
                        new DataColumn("x", typeof(int)),
                        new DataColumn("y",typeof(int)),
                        new DataColumn("z",typeof(int))});

                foreach (Game2RecordDB currGame2RecordDB in lsGame2Record)
                {
                    dtGame2.Rows.Add(currGame2RecordDB.Timestamp, currGame2RecordDB.MissHit, currGame2RecordDB.MissHit, currGame2RecordDB.MissHit);
                }

                List<GraphData> chartData = new List<GraphData>();
                foreach (DataRow dr in dtGame2.Rows)
                {
                    chartData.Add(new GraphData
                    {
                        w = Convert.ToString(dr["w"]),
                        x = (Convert.ToInt32(dr["x"])),
                        y = (Convert.ToInt32(dr["y"])),
                        z = (Convert.ToInt32(dr["z"]))
                    });
                }

                return chartData;
            }

            return null;
        }

        [WebMethod]
        public static List<GraphData> GetGame2PowerChartData(string selectedKidId, string selectedDate)
        {
            if ((selectedDate != "") && (selectedDate != ""))
            {
                Console.WriteLine(selectedKidId + selectedDate);

                List<Game2RecordDB> lsGame2Record = new List<Game2RecordDB>();

                int kidId = int.Parse(selectedKidId);
                DateTime date = DateTime.Parse(selectedDate);
                DateTime datePlus = date.AddDays(1);

                string connetionString = null;
                SqlConnection connection;
                SqlCommand command;
                string sql = null;
                SqlDataReader dataReader;
                connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;

                if (kidId > 0)
                {
                    // createdDate >= '2013-06-01' and createdDate < '2013-06-02' 
                    // GET game 1 record
                    // SELECT * FROM  [dbo].[Game1Record] WHERE [timestamp] >= '2019-04-15' AND [timestamp] < '2019-04-16';
                    sql = "SELECT * FROM [Game2Record] WHERE kidId=@kidId AND timestamp >= @dateStart AND timestamp < @dateEnd";
                    connection = new SqlConnection(connetionString);
                    try
                    {
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@kidId", kidId);
                        command.Parameters.AddWithValue("@dateStart", date);
                        command.Parameters.AddWithValue("@dateEnd", datePlus);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            Game2RecordDB currGame2RecordDB = new Game2RecordDB();
                            currGame2RecordDB.KidId = int.Parse(dataReader["kidId"].ToString());
                            currGame2RecordDB.Score = int.Parse(dataReader["score"].ToString());
                            currGame2RecordDB.MissHit = int.Parse(dataReader["missHit"].ToString());
                            currGame2RecordDB.Power = int.Parse(dataReader["power"].ToString());
                            currGame2RecordDB.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());

                            DateTime currGameDateTime = DateTime.Parse(dataReader["timestamp"].ToString());
                            lsGame2Record.Add(currGame2RecordDB);
                        }
                        dataReader.Close();
                        command.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message.ToString();
                    }
                }

                // sort the list by time
                lsGame2Record.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));

                // prepare game1 data
                DataTable dtGame2 = new DataTable();
                dtGame2.Columns.AddRange(new DataColumn[] {
                        new DataColumn("w", typeof(string)),
                        new DataColumn("x", typeof(int)),
                        new DataColumn("y",typeof(int)),
                        new DataColumn("z",typeof(int))});

                foreach (Game2RecordDB currGame2RecordDB in lsGame2Record)
                {
                    dtGame2.Rows.Add(currGame2RecordDB.Timestamp, currGame2RecordDB.Power, currGame2RecordDB.Power, currGame2RecordDB.Power);
                }

                List<GraphData> chartData = new List<GraphData>();
                foreach (DataRow dr in dtGame2.Rows)
                {
                    chartData.Add(new GraphData
                    {
                        w = Convert.ToString(dr["w"]),
                        x = (Convert.ToInt32(dr["x"])),
                        y = (Convert.ToInt32(dr["y"])),
                        z = (Convert.ToInt32(dr["z"]))
                    });
                }

                return chartData;
            }

            return null;
        }

        public class GraphData
        {
            public string w { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int kidId = -1;

            List<DateTime> lsDate = new List<DateTime>();
            List<Game1RecordDB> lsGame1Record = new List<Game1RecordDB>();
            List<Game2RecordDB> lsGame2Record = new List<Game2RecordDB>();

            string kidEmail = txtEmailAddress.Text;

            // get kid id
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            SqlDataReader dataReader;
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "SELECT * FROM [User] WHERE email=@email";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@email", kidEmail);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    kidId = int.Parse(dataReader["Id"].ToString());
                }
                dataReader.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }

            // if is less than 0 means not found
            if (kidId > 0)
            {
                // GET game 1 record
                sql = "SELECT * FROM [Game1Record] WHERE kidId=@kidId";
                connection = new SqlConnection(connetionString);
                try
                {
                    connection.Open();
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@kidId", kidId);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        //                private int kidId;
                        //private int score;
                        //private int wrongColor;
                        //private int boardHit;
                        //private string timestamp;

                        Game1RecordDB currGame1RecordDB = new Game1RecordDB();
                        currGame1RecordDB.KidId = int.Parse(dataReader["Id"].ToString());
                        currGame1RecordDB.Score = int.Parse(dataReader["score"].ToString());
                        currGame1RecordDB.WrongColor = int.Parse(dataReader["wrongColor"].ToString());
                        currGame1RecordDB.BoardHit = int.Parse(dataReader["boardHit"].ToString());
                        currGame1RecordDB.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());

                        DateTime currGameDateTime = DateTime.Parse(dataReader["timestamp"].ToString());
                        lsDate.Add(currGameDateTime.Date);
                        lsGame1Record.Add(currGame1RecordDB);
                    }
                    dataReader.Close();
                    command.Dispose();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    string err = ex.Message.ToString();
                }


                // GET game 2 record
                sql = "SELECT * FROM [Game2Record] WHERE kidId=@kidId";
                connection = new SqlConnection(connetionString);
                try
                {
                    connection.Open();
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@kidId", kidId);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        //                private int kidId;
                        //private int score;
                        //private int wrongColor;
                        //private int boardHit;
                        //private string timestamp;

                        Game2RecordDB currGame2RecordDB = new Game2RecordDB();
                        currGame2RecordDB.KidId = int.Parse(dataReader["Id"].ToString());
                        currGame2RecordDB.Score = int.Parse(dataReader["score"].ToString());
                        currGame2RecordDB.MissHit = int.Parse(dataReader["missHit"].ToString());
                        currGame2RecordDB.Power = int.Parse(dataReader["power"].ToString());
                        currGame2RecordDB.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());

                        DateTime currGameDateTime = DateTime.Parse(dataReader["timestamp"].ToString());
                        lsDate.Add(currGameDateTime.Date);
                        lsGame2Record.Add(currGame2RecordDB);
                    }
                    dataReader.Close();
                    command.Dispose();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    string err = ex.Message.ToString();
                }

                // get distinct date
                lsDate = lsDate.Select(x => x.Date).Distinct().ToList();
                ddlDates.DataSource = lsDate.Select(x => new { Text = x.Date, Value = x.Date });
                ddlDates.DataBind();

                // check if there's any record
                if (lsDate.Count > 1)
                {
                    ddlDates.Enabled = true;
                }
                else
                {
                    ddlDates.Enabled = false;
                }

                lblHiddenKidId.Text = kidId.ToString();
            }
        }

        protected void ddlDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("fired");

            int game1TotalScore = 0;
            int game1HighestScore = 0;
            int game2TotalScore = 0;
            int game2HighestScore = 0;

            int kidId = int.Parse(lblHiddenKidId.Text);
            lblHiddenKidId.Text = kidId.ToString();
            string selectedDate = ddlDates.SelectedValue.ToString();
            lblHiddenSelectedDate.Text = selectedDate;

            if ((kidId != 0) && (selectedDate != ""))
            {
                List<Game1RecordDB> lsGame1Record = new List<Game1RecordDB>();
                List<Game2RecordDB> lsGame2Record = new List<Game2RecordDB>();

                DateTime date = DateTime.Parse(selectedDate);
                DateTime datePlus = date.AddDays(1);

                string connetionString = null;
                SqlConnection connection;
                SqlCommand command;
                string sql = null;
                SqlDataReader dataReader;
                connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;

                if (kidId > 0)
                {
                    // createdDate >= '2013-06-01' and createdDate < '2013-06-02' 
                    // GET game 1 record
                    // SELECT * FROM  [dbo].[Game1Record] WHERE [timestamp] >= '2019-04-15' AND [timestamp] < '2019-04-16';
                    sql = "SELECT * FROM [Game1Record] WHERE kidId=@kidId AND timestamp >= @dateStart AND timestamp < @dateEnd";
                    connection = new SqlConnection(connetionString);
                    try
                    {
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@kidId", kidId);
                        command.Parameters.AddWithValue("@dateStart", date);
                        command.Parameters.AddWithValue("@dateEnd", datePlus);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            Game1RecordDB currGame1RecordDB = new Game1RecordDB();
                            currGame1RecordDB.KidId = int.Parse(dataReader["kidId"].ToString());
                            currGame1RecordDB.Score = int.Parse(dataReader["score"].ToString());
                            currGame1RecordDB.WrongColor = int.Parse(dataReader["wrongColor"].ToString());
                            currGame1RecordDB.BoardHit = int.Parse(dataReader["boardHit"].ToString());
                            currGame1RecordDB.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());

                            DateTime currGameDateTime = DateTime.Parse(dataReader["timestamp"].ToString());

                            // add score
                            game1TotalScore += currGame1RecordDB.Score;
                            if (currGame1RecordDB.Score > game1HighestScore)
                            {
                                game1HighestScore = currGame1RecordDB.Score;
                            }

                            lsGame1Record.Add(currGame1RecordDB);
                        }
                        dataReader.Close();
                        command.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message.ToString();
                    }
                }

                int game1AverageScore;
                if (lsGame1Record.Count > 0)
                {
                    game1AverageScore = game1TotalScore / lsGame1Record.Count;
                }
                else
                {
                    game1AverageScore = 0;
                }
                lblGame1AverageScore.Text = game1AverageScore.ToString();
                lblGame1HighestScore.Text = game1HighestScore.ToString();


                sql = "SELECT * FROM [Game2Record] WHERE kidId=@kidId AND timestamp >= @dateStart AND timestamp < @dateEnd";
                connection = new SqlConnection(connetionString);
                try
                {
                    connection.Open();
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@kidId", kidId);
                    command.Parameters.AddWithValue("@dateStart", date);
                    command.Parameters.AddWithValue("@dateEnd", datePlus);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Game2RecordDB currGame2RecordDB = new Game2RecordDB();
                        currGame2RecordDB.KidId = int.Parse(dataReader["kidId"].ToString());
                        currGame2RecordDB.Score = int.Parse(dataReader["score"].ToString());
                        currGame2RecordDB.MissHit = int.Parse(dataReader["missHit"].ToString());
                        currGame2RecordDB.Power = int.Parse(dataReader["power"].ToString());
                        currGame2RecordDB.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());

                        DateTime currGameDateTime = DateTime.Parse(dataReader["timestamp"].ToString());

                        // add score
                        game2TotalScore += currGame2RecordDB.Score;
                        if (currGame2RecordDB.Score > game1HighestScore)
                        {
                            game2HighestScore = currGame2RecordDB.Score;
                        }

                        lsGame2Record.Add(currGame2RecordDB);
                    }
                    dataReader.Close();
                    command.Dispose();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    string err = ex.Message.ToString();
                }

                int game2AverageScore;
                if (lsGame2Record.Count > 0)
                {
                    game2AverageScore = game2TotalScore / lsGame2Record.Count;
                }
                else
                {
                    game2AverageScore = 0;
                }
                lblGame2AverageScore.Text = game2AverageScore.ToString();
                lblGame2HighestScore.Text = game2HighestScore.ToString();
            }
        }
    }
}

