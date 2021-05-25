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
    public class ProposalsDB
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public ProposalsDB()
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
        public void Insert(Models.Proposal proposal)
        {
            string query = "INSERT INTO proposals (" +
                "jobID, " +
                "freelancerID, " +
                "propDescription, " +
                "propPrice, " +
                "clientAcceptance) VALUES(" +
                "\"" + proposal.jobID + "\"" + ", " +
                "\"" + proposal.freelancerID + "\"" + ", " +
                "\"" + proposal.propDescription + "\"" + ", " +
                "\"" + proposal.propPrice + "\"" + ", " +
                "\"" + proposal.clientAcceptance + "\"" +
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
        public void Update(Models.Proposal proposal)
        {
            string query = "UPDATE proposals SET " +
                "jobID=" + "\"" + proposal.jobID + "\"" + ", " +
                "freelancerID=" + "\"" + proposal.freelancerID + "\"" + ", " +
                "propDescription=" + "\"" + proposal.propDescription + "\"" + ", " +
                "propPrice=" + "\"" + proposal.propPrice + "\"" + ", " +
                "clientAcceptance=" + "\"" + proposal.clientAcceptance + "\"" +
                "WHERE propID=" + proposal.propID;

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
        public void Delete(string propID)
        {
            string query = "DELETE FROM proposals WHERE propID=" + "\"" + propID + "\"";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement with UserID
        public Models.Proposal SelectwithId(string propID)
        {
            string query = "SELECT * FROM proposals where propID = " + "\"" + propID + "\"";


            //Create a Object to store the result
            Models.Proposal proposal = new Models.Proposal();

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
                    proposal.propID = Int32.Parse(dataReader["propID"].ToString());
                    proposal.jobID = Int32.Parse(dataReader["jobID"].ToString());
                    proposal.freelancerID = Int32.Parse(dataReader["freelancerID"].ToString());
                    proposal.propDescription = dataReader["propDescription"].ToString();
                    proposal.propPrice = Int32.Parse(dataReader["propPrice"].ToString());
                    proposal.clientAcceptance = dataReader["clientAcceptance"].ToString();
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return proposal;
            }
            else
            {
                return proposal;
            }
        }
        
        //Select statement
        public List<Models.Proposal> SelectAll()
        {
            string query = "SELECT * FROM proposals";

            //Create a list to store the result
            List<Models.Proposal> list = new List<Models.Proposal>();

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
                    Models.Proposal proposal = new Models.Proposal();
                    proposal.propID = Int32.Parse(dataReader["propID"].ToString());
                    proposal.jobID = Int32.Parse(dataReader["jobID"].ToString());
                    proposal.freelancerID = Int32.Parse(dataReader["freelancerID"].ToString());
                    proposal.propDescription = dataReader["propDescription"].ToString();
                    proposal.propPrice = Int32.Parse(dataReader["propPrice"].ToString());
                    proposal.clientAcceptance = dataReader["clientAcceptance"].ToString();
                    list.Add(proposal);
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
            string query = "SELECT Count(*) FROM proposals";
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