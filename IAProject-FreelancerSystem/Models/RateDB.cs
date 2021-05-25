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
    public class RateDB
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public RateDB()
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
        public void Insert(Models.Rate rate)
        {
            string query = "INSERT INTO rates (" +
                "jobID, " +
                "freelancerID, " +
                "rate) VALUES(" +
                "\"" + rate.jobID + "\"" + ", " +
                "\"" + rate.freelancerID + "\"" + ", " +
                "\"" + rate.rate + "\"" +
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
        public void Update(Models.Rate rate)
        {
            string query = "UPDATE rates SET " +
                "jobID=" + "\"" + rate.jobID + "\"" +
                "userName=" + "\"" + rate.freelancerID + "\"" +
                "rate=" + "\"" + rate.rate + "\"" +
                "WHERE rateID =" + rate.rateID;

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
        public void Delete(string rateID)
        {
            string query = "DELETE FROM rates WHERE rateID=" + "\"" + rateID + "\"";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement with UserID
        public Models.Rate SelectwithId(string rateID)
        {
            string query = "SELECT * FROM rates where rateID  = " + "\"" + rateID + "\"";

            //Create a Object to store the result
            Models.Rate rate = new Models.Rate();

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
                    rate.rateID = Int32.Parse(dataReader["rateID"].ToString());
                    rate.jobID = Int32.Parse(dataReader["jobID"].ToString());
                    rate.freelancerID = Int32.Parse(dataReader["freelancerID"].ToString());
                    rate.rate = Int32.Parse(dataReader["rate"].ToString());
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return rate;
            }
            else
            {
                return rate;
            }
        }
        
        //Select statement
        public List<Models.Rate> SelectAll()
        {
            string query = "SELECT * FROM rates";

            //Create a list to store the result
            List<Models.Rate> list = new List<Models.Rate>();

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
                    Models.Rate rate = new Models.Rate();
                    rate.rateID = Int32.Parse(dataReader["rateID"].ToString());
                    rate.jobID = Int32.Parse(dataReader["jobID"].ToString());
                    rate.freelancerID = Int32.Parse(dataReader["freelancerID"].ToString());
                    rate.rate = Int32.Parse(dataReader["rate"].ToString());
                    list.Add(rate);
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
        public int Count(string _query)
        {
            string query = "SELECT Count(*) FROM rates";
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