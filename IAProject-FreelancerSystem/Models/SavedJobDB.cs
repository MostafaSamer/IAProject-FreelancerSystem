using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows;
using MySql.Data.MySqlClient;

namespace IAProject_FreelancerSystem.Models
{
    public class SavedJobDB
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public SavedJobDB()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "freelance";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert(Models.SavedJob savedJobs)
        {
            string query = "INSERT INTO savedjobs (" +
                "freelancerID, " +
                "jobID, " +
                "jobTitle, " +
                "jobDate, " +
                "clientName) VALUES(" +
                savedJobs.freelancerID +
                savedJobs.jobID +
                savedJobs.jobTitle +
                savedJobs.jobDate +
                savedJobs.clientName +
                ")";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update(Models.SavedJob savedJobs)
        {
            string query = "UPDATE savedjobs SET " +
                "freelancerID=" + savedJobs.freelancerID +
                "jobID=" + savedJobs.jobID +
                "jobTitle=" + savedJobs.jobTitle +
                "jobDate=" + savedJobs.jobDate +
                "clientName=" + savedJobs.clientName +
                "WHERE savedID=" + savedJobs.savedID;

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete(string savedID)
        {
            string query = "DELETE FROM savedjobs WHERE savedID=" + savedID;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement with UserID
        public Models.SavedJob SelectwithId(string savedID)
        {
            string query = "SELECT * FROM savedjobs where savedID = " + savedID;

            //Create a Object to store the result
            Models.SavedJob savedJobs = new Models.SavedJob();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    savedJobs.savedID = Int32.Parse(dataReader["savedID"].ToString());
                    savedJobs.freelancerID = Int32.Parse(dataReader["freelancerID"].ToString());
                    savedJobs.jobID = Int32.Parse(dataReader["jobID"].ToString());
                    savedJobs.jobTitle = dataReader["jobTitle"].ToString();
                    savedJobs.jobDate = dataReader["jobDate"].ToString();
                    savedJobs.clientName = dataReader["clientName"].ToString();
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return savedJobs;
            }
            else
            {
                return savedJobs;
            }
        }
        
        //Select statement
        public List<Models.SavedJob> SelectAll()
        {
            string query = "SELECT * FROM savedjobs";

            //Create a list to store the result
            List<Models.SavedJob> list = new List<Models.SavedJob>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    Models.SavedJob savedJobs = new Models.SavedJob();
                    savedJobs.savedID = Int32.Parse(dataReader["savedID"].ToString());
                    savedJobs.freelancerID = Int32.Parse(dataReader["freelancerID"].ToString());
                    savedJobs.jobID = Int32.Parse(dataReader["jobID"].ToString());
                    savedJobs.jobTitle = dataReader["jobTitle"].ToString();
                    savedJobs.jobDate = dataReader["jobDate"].ToString();
                    savedJobs.clientName = dataReader["clientName"].ToString();
                    list.Add(savedJobs);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Count statement
        public int Count()
        {
            string query = "SELECT Count(*) FROM savedjobs";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }
    }
}