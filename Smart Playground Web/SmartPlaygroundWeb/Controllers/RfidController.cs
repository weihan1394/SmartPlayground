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
    public class RfidController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public Boolean Get(string rfid)
        {
            int userId = -1;

            // get the user id from rfidtag
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            SqlDataReader dataReader;
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "SELECT * FROM [rfidtag] WHERE barcode = @barcode AND status = '1'";
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
                return false;
            }
            else
            {
                return true;
            }
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