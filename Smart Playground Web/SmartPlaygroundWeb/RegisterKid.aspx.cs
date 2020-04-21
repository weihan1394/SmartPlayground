using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartPlaygroundWeb
{
    public partial class RegisterStudent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            updateTotalKids();
        }

        private void loadRfidDropDownList()
        {
            var ddlRfidListMap = new Dictionary<string, string>();

            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            SqlDataReader dataReader;
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "SELECT * FROM [rfidtag] WHERE status=0";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    ddlRfidListMap.Add(dataReader["Id"].ToString(), dataReader["name"].ToString());
                }

                dataReader.Close();
                command.Dispose();
                connection.Close();

                ddlRfidTag.DataSource = ddlRfidListMap;
                ddlRfidTag.DataTextField = "Value";
                ddlRfidTag.DataValueField = "Key";
                ddlRfidTag.DataBind();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            updateTotalKids();
            getZoneDetails();
        }

        private void getZoneDetails()
        {
            zone1Repeater.DataBind();
            zone2Repeater.DataBind();
            zone3Repeater.DataBind();
            zone4Repeater.DataBind();
        }

        private void updateTotalKids()
        {
            int zone1 = 0;
            int zone2 = 0;
            int zone3 = 0;
            int zone4 = 0;

            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            SqlDataReader dataReader;
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "SELECT * FROM [User] WHERE login=1";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    int zoneId = int.Parse(dataReader["zone"].ToString());

                    if (zoneId == 1)
                    {
                        zone1++;
                    }
                    else if (zoneId == 2)
                    {
                        zone2++;
                    }
                    else if (zoneId == 3)
                    {
                        zone3++;
                    }
                    else if (zoneId == 4)
                    {
                        zone4++;
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

            lblZone1.Text = zone1.ToString();
            lblZone2.Text = zone2.ToString();
            lblZone3.Text = zone3.ToString();
            lblZone4.Text = zone4.ToString();

            string red = "info-box-icon bg-red";
            string yellow = "info-box-icon bg-yellow";
            string blue = "info-box-icon bg-aqua";

            if (zone1 <= 1)
            {
                zone1Span.Attributes["class"] = blue;
            }
            else if (zone1 <= 2)
            {
                zone1Span.Attributes["class"] = yellow;
            }
            else
            {
                zone1Span.Attributes["class"] = red;
            }


            if (zone2 <= 1)
            {
                zone2Span.Attributes["class"] = blue;
            }
            else if (zone2 <= 2)
            {
                zone2Span.Attributes["class"] = yellow;
            }
            else
            {
                zone2Span.Attributes["class"] = red;
            }


            if (zone3 <= 1)
            {
                zone3Span.Attributes["class"] = blue;
            }
            else if (zone3 <= 2)
            {
                zone3Span.Attributes["class"] = yellow;
            }
            else
            {
                zone3Span.Attributes["class"] = red;
            }


            if (zone4 <= 1)
            {
                zone4Span.Attributes["class"] = blue;
            }
            else if (zone4 <= 2)
            {
                zone4Span.Attributes["class"] = yellow;
            }
            else
            {
                zone4Span.Attributes["class"] = red;
            }
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            string selectedTag = ddlRfidTag.SelectedItem.Value;
            string userId = Session["id"].ToString();

            DateTime currDateTimeNow = DateTime.Now;

            // update rfidtag table
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "UPDATE [rfidtag] SET [status]=1, [userid]=@userid WHERE [Id]=@id";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("userid", userId);
                command.Parameters.AddWithValue("Id", selectedTag);

                int rowsAffected = command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }

            // update user table
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "UPDATE [User] SET [login]=1, [rfidtagId]=@rfidtagId, [timestamp]=@timestamp, [timestampEnd]=@timestampEnd, [zone]=1 WHERE [Id]=@id";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("rfidtagId", selectedTag);
                command.Parameters.AddWithValue("Id", userId);
                command.Parameters.AddWithValue("timestamp", currDateTimeNow);
                command.Parameters.AddWithValue("timestampEnd", DBNull.Value);

                int rowsAffected = command.ExecuteNonQuery();
                imgProfilePic.Style.Add("border", "3px solid #605ca8");

                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }

            btnStart.Enabled = false;
            btnStart.Visible = false;
            btnEnd.Enabled = true;
            btnEnd.Visible = true;
            lblTag.Visible = false;
            lblTag.Enabled = false;
            ddlRfidTag.Visible = false;
            ddlRfidTag.Enabled = false;

            // update the last visit time for the user
            lblLastVisit.Text = currDateTimeNow.ToString("MM/dd/yyyy hh:mm tt");
            lblLastVisitEnd.Text = "";
        }

        protected void btnEnd_Click(object sender, EventArgs e)
        {
            string userId = Session["id"].ToString();

            DateTime currDateTimeNow = DateTime.Now;

            // update rfidtag table
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "UPDATE [rfidtag] SET [status]=0, [userid]=0 WHERE [userid]=@userid";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("userid", userId);

                int rowsAffected = command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }

            // update user table
            connetionString = ConfigurationManager.ConnectionStrings["SmartPlaygroundConnectionString"].ConnectionString;
            sql = "UPDATE [User] SET [login]=0, [rfidtagId]=0, [zone]=0, [timestampEnd]=@timestampEnd WHERE [Id]=@id";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("timestampEnd", currDateTimeNow);
                command.Parameters.AddWithValue("Id", userId);

                int rowsAffected = command.ExecuteNonQuery();
                imgProfilePic.Style.Add("border", "3px solid #ff851b");

                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }

            btnStart.Enabled = true;
            btnStart.Visible = true;
            btnEnd.Enabled = false;
            btnEnd.Visible = false;
            lblTag.Visible = true;
            lblTag.Enabled = true;
            ddlRfidTag.Visible = true;
            ddlRfidTag.Enabled = true;

            loadRfidDropDownList();

            lblLastVisitEnd.Text = currDateTimeNow.ToString("MM/dd/yyyy hh:mm tt");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string emailAddress = txtEmailAddress.Text;
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
                command.Parameters.AddWithValue("email", emailAddress);
                dataReader = command.ExecuteReader();
                bool found = false;
                if (dataReader.Read())
                {
                    imgProfilePic.ImageUrl = "~/Resource/image/kid/" + dataReader["email"] + ".jpg";
                    DateTime birthday = DateTime.Parse(dataReader["birthday"].ToString());
                    int day = birthday.Day;
                    int month = birthday.Month;
                    int year = birthday.Year;

                    int yearNow = DateTime.Now.Year;
                    int age = yearNow - year;

                    char gender = dataReader["gender"].ToString()[0];

                    if (gender.Equals('M'))
                    {
                        lblChildMoreInfo.Text = "Male  |  " + age + " years old  |  " + day + "/" + month;
                    }
                    else
                    {
                        lblChildMoreInfo.Text = "Female  |  " + age + " years old  |  " + day + "/" + month;
                    }



                    string id = dataReader["Id"].ToString();
                    lblID.Text = id;
                    string childName = dataReader["name"].ToString();
                    lblChildName.Text = childName;
                    string parentName = dataReader["contactName"].ToString();
                    lblParent.Text = parentName;
                    string timestamp = dataReader["timestamp"].ToString();
                    lblLastVisit.Text = timestamp;
                    string timestampEnd = dataReader["timestampEnd"].ToString();
                    lblLastVisitEnd.Text = timestampEnd;
                    string contactNumber = dataReader["contactNumber"].ToString();
                    lblParentContact.Text = contactNumber;
                    string contactRelationship = dataReader["contactRelationship"].ToString();
                    lblRelationship.Text = contactRelationship;
                    string registeredDate = dataReader["registeredDate"].ToString();
                    lblRegisteredDate.Text = registeredDate;
                    string note = dataReader["note"].ToString();
                    lblNote.Text = note;


                    int login = int.Parse(dataReader["login"].ToString());
                    if (login == 1)
                    {
                        // already login
                        imgProfilePic.Style.Add("border", "3px solid #605ca8");

                        btnStart.Enabled = false;
                        btnStart.Visible = false;
                        btnEnd.Enabled = true;
                        btnEnd.Visible = true;
                        lblTag.Visible = false;
                        lblTag.Enabled = false;
                        ddlRfidTag.Visible = false;
                        ddlRfidTag.Enabled = false;
                    }
                    else
                    {
                        imgProfilePic.Style.Add("border", "3px solid #ff851b");

                        btnStart.Enabled = true;
                        btnStart.Visible = true;
                        btnEnd.Enabled = false;
                        btnEnd.Visible = false;
                        lblTag.Visible = true;
                        lblTag.Enabled = true;
                        ddlRfidTag.Visible = true;
                        ddlRfidTag.Enabled = true;

                        loadRfidDropDownList();
                    }

                    found = true;

                    Session["id"] = id;
                }
                else
                {
                    // no such user
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
}