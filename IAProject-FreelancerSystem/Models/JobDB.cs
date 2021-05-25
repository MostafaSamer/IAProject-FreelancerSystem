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
    public class JobDB
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public JobDB()
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
        public void Insert(Models.Job job)
        {
            string query = "INSERT INTO jobs (" +
                "freelancerID, " +
                "clientID, " +
                "jobTitle, " +
                "jobBudget, " +
                "jobType, " +
                "creationDate, " +
                "jobDescription, " +
                "jobAVGRate, " +
                "jobStatus, " +
                "jobAdminAcceptance, " +
                "propCount) VALUES(" +
                "\"" + job.freelancerID + "\"" + ", " +
                "\"" + job.clientID + "\"" + ", " +
                "\"" + job.jobTitle + "\"" + ", " +
                "\"" + job.jobBudget + "\"" + ", " +
                "\"" + job.jobType + "\"" + ", " +
                "\"" + job.creationDate + "\"" + ", " +
                "\"" + job.jobDescription + "\"" + ", " +
                "\"" + job.jobAVGRate + "\"" + ", " +
                "\"" + job.jobStatus + "\"" + ", " +
                "\"" + job.jobAdminAcceptance + "\"" + ", " +
                "\"" + job.propCount + "\"" +
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
        public void Update(Models.Job job)
        {
            string query = "UPDATE jobs SET " +
                "freelancerID=" + "\"" + job.freelancerID + "\", " + 
                "clientID=" + "\"" + job.clientID + "\", " +
                "jobTitle=" + "\"" + job.jobTitle + "\", " +
                "jobBudget=" + "\"" + job.jobBudget + "\", " +
                "jobType=" + "\"" + job.jobType + "\", " +
                "jobDescription=" + "\"" + job.jobDescription + "\", " +
                "jobAVGRate=" + "\"" + job.jobAVGRate + "\", " +
                "jobStatus=" + "\"" + job.jobStatus + "\", " +
                "jobAdminAcceptance=" + "\"" + job.jobAdminAcceptance + "\", " +
                "propCount=" + "\"" + job.propCount + "\"" +
                "WHERE jobID=" + "\"" + job.jobID + "\"";

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
        public void Delete(string jobID)
        {
            string query = "DELETE FROM jobs WHERE jobID=" + "\"" + jobID + "\"";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement with UserID
        public Models.Job SelectwithId(string jobID)
        {
            string query = "SELECT * FROM jobs where jobID = " + "\"" + jobID + "\"";

            //Create a Object to store the result
            Models.Job job = new Models.Job();

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
                    job.jobID = Int32.Parse(dataReader["jobID"].ToString());
                    job.freelancerID = Int32.Parse(dataReader["freelancerID"].ToString());
                    job.clientID = Int32.Parse(dataReader["clientID"].ToString());
                    job.jobTitle = dataReader["jobTitle"].ToString();
                    job.jobBudget = Int32.Parse(dataReader["jobBudget"].ToString());
                    job.jobType = dataReader["jobType"].ToString();
                    job.creationDate = dataReader["creationDate"].ToString();
                    job.jobDescription = dataReader["jobDescription"].ToString();
                    job.jobAVGRate = Int32.Parse(dataReader["jobAVGRate"].ToString());
                    job.jobStatus = dataReader["jobStatus"].ToString();
                    job.jobAdminAcceptance = dataReader["jobAdminAcceptance"].ToString();
                    job.propCount = Int32.Parse(dataReader["propCount"].ToString());
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return job;
            }
            else
            {
                return job;
            }
        }
        
        //Select statement
        public List<Models.Job> SelectAll()
        {
            string query = "SELECT * FROM jobs";

            //Create a list to store the result
            List<Models.Job> list = new List<Models.Job>();

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
                    Models.Job job = new Models.Job();
                    job.jobID = Int32.Parse(dataReader["jobID"].ToString());
                    job.freelancerID = Int32.Parse(dataReader["freelancerID"].ToString());
                    job.clientID = Int32.Parse(dataReader["clientID"].ToString());
                    job.jobTitle = dataReader["jobTitle"].ToString();
                    job.jobBudget = Int32.Parse(dataReader["jobBudget"].ToString());
                    job.jobType = dataReader["jobType"].ToString();
                    job.creationDate = dataReader["creationDate"].ToString();
                    job.jobDescription = dataReader["jobDescription"].ToString();
                    job.jobAVGRate = Int32.Parse(dataReader["jobAVGRate"].ToString());
                    job.jobStatus = dataReader["jobStatus"].ToString();
                    job.jobAdminAcceptance = dataReader["jobAdminAcceptance"].ToString();
                    job.propCount = Int32.Parse(dataReader["propCount"].ToString());
                    list.Add(job);
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
            string query = "SELECT Count(*) FROM jobs";
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