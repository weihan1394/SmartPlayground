using SmartPlaygroundWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartPlaygroundWeb.Controllers
{
    public class HighestScoreController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public HighestRecord Get(int zone)
        {
            DateTime today = DateTime.Now;
            DateTime tomorrow = today.AddDays(1);

            int gameHighestScore = 0;
            bool found = false;

            HighestRecord highestRecord = new HighestRecord();
            highestRecord.Name = "---";
            highestRecord.Score = 0;
            highestRecord.Timestamp = today;

            if ((zone == 1) || (zone == 2))
            {
                // basketball game
                string connetionString = null;
                SqlConnection connection;
                SqlCommand command;
                string sql = null;
                SqlDataReader dataReader;
                connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
                // sql = "SELECT * FROM [rfidtag] WHERE barcode = @barcode AND status = '1'";
                if (zone == 1)
                {
                    sql = "SELECT * FROM [Game1Record]";
                }
                else if (zone == 2)
                {
                    sql = "SELECT * FROM [Game2Record]";
                }
                connection = new SqlConnection(connetionString);
                try
                {
                    connection.Open();
                    command = new SqlCommand(sql, connection);
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        int currScore = int.Parse(dataReader["score"].ToString());
                        if (currScore > gameHighestScore)
                        {
                            found = true;
                            // found higher score
                            gameHighestScore = currScore;
                            highestRecord.Name = dataReader["kidId"].ToString();
                            highestRecord.Score = currScore;
                            highestRecord.Timestamp = DateTime.Parse(dataReader["timestamp"].ToString());
                        }
                    }

                    dataReader.Close();
                    command.Dispose();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    string err = ex.Message.ToString();
                }

                // get the name
                if (found)
                {
                    // means there is someone
                    // find the name
                    sql = "SELECT * FROM [User] WHERE Id = @kidId";
                    connection = new SqlConnection(connetionString);
                    try
                    {
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@kidId", highestRecord.Name);
                        dataReader = command.ExecuteReader();

                        if (dataReader.Read())
                        {
                            highestRecord.Name = dataReader["name"].ToString();
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
            }

            return highestRecord;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
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