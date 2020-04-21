using SmartPlaygroundWeb.Models;
using SmartPlaygroundWeb.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace SmartPlaygroundWeb.Controllers
{
    public class Game1Controller : ApiController
    {
        Helper helper = new Helper();

        

        // GET api/<controller>
        public IEnumerable<Game1Record> Get()
        {
            List<Game1Record> lsGameRecord = new List<Game1Record>();

            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            SqlDataReader dataReader;
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "SELECT * FROM [Game1Record]";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Game1Record newGame1Record = new Game1Record();
                    newGame1Record.KidId = int.Parse(dataReader["kidId"].ToString());
                    newGame1Record.Score = int.Parse(dataReader["score"].ToString());
                    newGame1Record.WrongColor = int.Parse(dataReader["wrongColor"].ToString());
                    newGame1Record.BoardHit = int.Parse(dataReader["boardHit"].ToString());
                    newGame1Record.Timestamp = (dataReader["timeStamp"].ToString());

                    lsGameRecord.Add(newGame1Record);
                }

                dataReader.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }

            return lsGameRecord;
        }

        // GET api/<controller>/5
        public Kid Get(string rfid)
        {
            int userId = -1;
            Kid kid = new Kid();

            // get user id
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            SqlDataReader dataReader;
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            // sql = "SELECT * FROM [rfidtag] WHERE barcode = @barcode AND status = '1'";
            sql = "SELECT * FROM [rfidtag] WHERE barcode = @barcode";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@barcode", rfid);
                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    userId = int.Parse(dataReader["userid"].ToString());
                }

                dataReader.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }

            if (userId == -1)
            {
                // rfid not found
                kid.KidId = -1;
            }
            else if (userId == 0)
            {
                // rfid found but no kid tagged into it
                kid.KidId = 0;
            }
            else if (userId >= 1)
            {
                // found user
                if (userId != 0)
                {
                    sql = "SELECT * FROM [User] WHERE [Id] = @userId";
                    connection = new SqlConnection(connetionString);
                    try
                    {
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@userId", userId);
                        dataReader = command.ExecuteReader();

                        if (dataReader.Read())
                        {
                            kid.KidId = int.Parse(dataReader["Id"].ToString());
                            kid.Email = dataReader["email"].ToString();
                            kid.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());
                            kid.TimestampEnd = dataReader["timestampEnd"].ToString();
                            kid.ContactNumber = dataReader["contactNumber"].ToString();
                            kid.ContactRelationship = dataReader["contactRelationship"].ToString();
                            kid.RegistereDate = DateTime.Parse(dataReader["registeredDate"].ToString());
                            kid.Note = dataReader["note"].ToString();
                            kid.CurrentStation = dataReader["currentStation"].ToString();
                            kid.Name = dataReader["name"].ToString();
                            kid.Zone = dataReader["zone"].ToString();
                            kid.Gender = dataReader["gender"].ToString();
                        }

                        dataReader.Close();
                        command.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message.ToString();
                    }

                    // update the kid to be in zone 2
                    sql = "UPDATE [User] SET [zone]=2 WHERE [Id]=@id";
                    connection = new SqlConnection(connetionString);
                    try
                    {
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("Id", userId);

                        int rowsAffected = command.ExecuteNonQuery();

                        command.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message.ToString();
                    }
                }

                // call other queue to check
                // retrieve game 2 ip address
                string game2QueueIP = "";
                // TEST: MAC
                // sql = "SELECT * FROM [ConfigurationSetting] WHERE [Device] = 'MAC'";
                // LIVE
                sql = "SELECT * FROM [ConfigurationSetting] WHERE [Device] = 'RPI2'";
                connection = new SqlConnection(connetionString);
                try
                {
                    connection.Open();
                    command = new SqlCommand(sql, connection);
                    dataReader = command.ExecuteReader();

                    if (dataReader.Read())
                    {
                        game2QueueIP = dataReader["IP"].ToString();
                    }

                    dataReader.Close();
                    command.Dispose();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    string err = ex.Message.ToString();
                }

                // try to ping game 1 queue
                bool game1Available = helper.PingHost(game2QueueIP);

                if (game1Available)
                {
                    // game 1 queue is up (e.g.  http://192.168.1.12:5000/)
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + game2QueueIP + ":5000/checkKidExistInQueue");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            rfid
                        });

                        Console.WriteLine(json);
                        streamWriter.Write(json);
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                }
                else
                {
                    // game 1 queue is not up
                    Console.WriteLine("Game 1 queue is not up");
                }
            }

            return kid;
        }

        // POST api/<controller>
        public async System.Threading.Tasks.Task<string> PostAsync()
        {
            string returndata = "";
            string result = await Request.Content.ReadAsStringAsync();

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            Game1Record game1Record = Newtonsoft.Json.JsonConvert.DeserializeObject<Game1Record>(result);

            // insert to database record
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "INSERT INTO [Game1Record] (kidId, zoneId, score, wrongColor, boardHit, timestamp) " +
                "VALUES (@kidId, @zoneId, @score, @wrongColor, @boardHit, @timestamp)";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@kidId", game1Record.KidId);
                command.Parameters.AddWithValue("@zoneId", 1);
                command.Parameters.AddWithValue("@score", game1Record.Score);
                command.Parameters.AddWithValue("@wrongColor", game1Record.WrongColor);
                command.Parameters.AddWithValue("@boardHit", game1Record.BoardHit);
                DateTime recordDT = DateTime.ParseExact(game1Record.Timestamp, "dd/MM/yyyy H:mm:ss", null);
                command.Parameters.AddWithValue("@timestamp", recordDT);

                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
                returndata = "yes";
            }
            catch (Exception ex)
            {
                returndata = ex.Message.ToString();
            }

            //{
            //    "kidId": 1,
            //    "score": 5,
            //    "takeCorrect": 2,
            //    "takeWrong": 3,
            //    "throwIn": "5",
            //    "throwOut": "6"
            //    "timestamp": "14.04.2019 08:36:14"
            //}

            return returndata;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}