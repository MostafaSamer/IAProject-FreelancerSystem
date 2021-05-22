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
    public class UserDB
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public UserDB()
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
        public void Insert(BL.User user)
        {
            string query = "INSERT INTO users (" +
                "userPassword, " +
                "userName, " +
                "fName, " +
                "lName, " +
                "email, " +
                "phoneNum, " +
                "userPhoto, " +
                "role) VALUES(" +
                user.userPassword +
                user.userName +
                user.fName +
                user.lName +
                user.email +
                user.phoneNum +
                user.userPhoto +
                user.role + 
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
        public void Update(BL.User user)
        {
            string query = "UPDATE users SET " +
                "userPassword=" + user.userPassword +
                "userName=" + user.userName +
                "fName=" + user.fName +
                "lName=" + user.lName +
                "email=" + user.email +
                "phoneNum=" + user.phoneNum +
                "userPhoto=" + user.userPhoto +
                "role=" + user.role +
                "WHERE userID=" + user.userID;

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
        public void Delete(string userID)
        {
            string query = "DELETE FROM users WHERE userID=" + userID;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement with UserID
        public BL.User SelectwithId(string userID)
        {
            string query = "SELECT * FROM users where userID = " + userID;

            //Create a Object to store the result
            BL.User user = new BL.User();

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
                    user.userID = Int32.Parse(dataReader["userID"].ToString());
                    user.userPassword = dataReader["userPassword"].ToString();
                    user.userName = dataReader["userName"].ToString();
                    user.fName = dataReader["fName"].ToString();
                    user.lName = dataReader["lName"].ToString();
                    user.email = dataReader["email"].ToString();
                    user.phoneNum = dataReader["phoneNum"].ToString();
                    user.userPhoto = dataReader["userPhoto"].ToString();
                    user.role = dataReader["role"].ToString();
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return user;
            }
            else
            {
                return user;
            }
        }
        
        //Select statement
        public List<BL.User> SelectAll()
        {
            string query = "SELECT * FROM users";

            //Create a list to store the result
            List<BL.User> list = new List<BL.User>();

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
                    BL.User user = new BL.User();
                    user.userID = Int32.Parse(dataReader["userID"].ToString());
                    user.userPassword = dataReader["userPassword"].ToString();
                    user.userName = dataReader["userName"].ToString();
                    user.fName = dataReader["fName"].ToString();
                    user.lName = dataReader["lName"].ToString();
                    user.email = dataReader["email"].ToString();
                    user.phoneNum = dataReader["phoneNum"].ToString();
                    user.userPhoto = dataReader["userPhoto"].ToString();
                    user.role = dataReader["role"].ToString();
                    list.Add(user);
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
            string query = "SELECT Count(*) FROM users";
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