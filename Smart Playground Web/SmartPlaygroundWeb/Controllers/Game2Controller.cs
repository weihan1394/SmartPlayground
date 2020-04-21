using SmartPlaygroundWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.NetworkInformation;
using SmartPlaygroundWeb.Util;
using System.IO;
using System.Web.Script.Serialization;

namespace SmartPlaygroundWeb.Controllers
{
    public class Game2Controller : ApiController
    {
        Helper helper = new Helper();

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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

                    // update the kid to be in zone 3
                    sql = "UPDATE [User] SET [zone]=3 WHERE [Id]=@id";
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
                // retrieve game 1 ip address
                string game1QueueIP = "";
                // TEST: MAC
                //sql = "SELECT * FROM [ConfigurationSetting] WHERE [Device] = 'MAC'";
                // LIVE
                sql = "SELECT * FROM [ConfigurationSetting] WHERE [Device] = 'RPI1'";
                connection = new SqlConnection(connetionString);
                try
                {
                    connection.Open();
                    command = new SqlCommand(sql, connection);
                    dataReader = command.ExecuteReader();

                    if (dataReader.Read())
                    {
                        game1QueueIP = dataReader["IP"].ToString();
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
                bool game1Available = helper.PingHost(game1QueueIP);

                if (game1Available)
                {
                    // game 1 queue is up (e.g.  http://192.168.1.12:5000/)
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + game1QueueIP + ":5000/checkKidExistInQueue");
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
                }
            }

            return kid;
        }

        public async System.Threading.Tasks.Task<string> PostAsync()
        {
            string returndata = "";
            string result = await Request.Content.ReadAsStringAsync();

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            Game2Record game2Record = Newtonsoft.Json.JsonConvert.DeserializeObject<Game2Record>(result);

            // insert to database record
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "INSERT INTO [Game2Record] (kidId, zoneId, score, missHit, power, timestamp) " +
                "VALUES (@kidId, @zoneId, @score, @missHit, @power, @timestamp)";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@kidId", game2Record.KidId);
                command.Parameters.AddWithValue("@zoneId", 3);
                command.Parameters.AddWithValue("@score", game2Record.Score);
                command.Parameters.AddWithValue("@missHit", game2Record.MissHit);
                command.Parameters.AddWithValue("@power", game2Record.Power);
                DateTime recordDT = DateTime.ParseExact(game2Record.Timestamp, "dd/MM/yyyy H:mm:ss", null);
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