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
    public class KidController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public Kid Get(string rfid)
        {
            int userid = 0;
            Kid kid = new Kid();

            // get the user id from rfidtag table
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            SqlDataReader dataReader;
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "SELECT * FROM [rfidtag] WHERE status=1 AND [barcode] = @barcode";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@barcode", rfid);
                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    userid = int.Parse(dataReader["userid"].ToString());
                }

                dataReader.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }

            if (userid != 0)
            {
                sql = "SELECT * FROM [User] WHERE [Id] = @userId";
                connection = new SqlConnection(connetionString);
                try
                {
                    connection.Open();
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@userId", userid);
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
                        kid.Name = dataReader["contactName"].ToString();
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

            }

            return kid;
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