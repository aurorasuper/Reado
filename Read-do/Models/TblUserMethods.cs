using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Read_do.Models
{
    public class TblUserMethods
    {
        public TblUserMethods() { }

        public int InsertUser(TblUserDetails usr, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and add user to database
            String sqlsting = "INSERT INTO Tbl_User (Usr_Name, Usr_Email, Usr_Password, Usr_IsAdmin) VALUES (@userName, @email, @password, @bool)";
            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);

            dbCommand.Parameters.Add("userName", SqlDbType.NVarChar, 30).Value = usr.userName;
            dbCommand.Parameters.Add("email", SqlDbType.NVarChar, 150).Value = usr.email;
            dbCommand.Parameters.Add("password", SqlDbType.NVarChar, 150).Value = usr.password;
            dbCommand.Parameters.Add("bool", SqlDbType.Bit).Value = false;



            try
            {
                dBConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if(i == 1)
                {
                    errormsg = "";
                }
                else
                {

                    errormsg = "Could not add user to database";
                }
                return i;

            }catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dBConnection.Close();
            }


        }


        public Boolean LogInCheck(TblUserDetails user, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string to get password corresponding to user 
            String sqlsting = "SELECT Usr_Password FROM Tbl_User WHERE Usr_Name=@userdata";
            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);
            dbCommand.Parameters.Add("userdata", SqlDbType.NVarChar, 30).Value = user.userName;


            SqlDataReader reader;
            errormsg = "";


            try
            {
                dBConnection.Open();
                Boolean correctPassword = false;
                string storedPassword = "";

                // start and read from reader
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    storedPassword = (string)reader["Usr_Password"];
                }

                // check if password from database with given username corresponds to given password
                if (storedPassword == user.password)
                {
                    correctPassword = true;
                }
                else // either stored password did not correspond with given password OR username could not be found in database
                {
                    errormsg = "Incorrect username or password.";
                    correctPassword = false;
                }

                reader.Close();
                return correctPassword;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return false;
            }
            finally
            {
                dBConnection.Close();
            }
        }

        public Boolean IsAdmin(string username, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string to get users admin status
            String sqlsting = "SELECT Usr_IsAdmin FROM Tbl_User WHERE Usr_Name=@userdata";
            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);
            dbCommand.Parameters.Add("userdata", SqlDbType.NVarChar, 30).Value =username;


            SqlDataReader reader;

            try
            {
                dBConnection.Open();
                Boolean isAdmin = false;
                

                // start and read from reader
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    isAdmin = Convert.ToBoolean(reader["Usr_IsAdmin"]);
                }
               
                
                errormsg = "";
                reader.Close();
                return isAdmin;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return false;
            }
            finally
            {
                dBConnection.Close();
            }
        }

        public TblUserDetails GetUser(string username, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();
            TblUserDetails user = new TblUserDetails();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and get user from database
            String sqlsting = "SELECT * FROM Tbl_User WHERE Usr_Name=@userName";
            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);
            dbCommand.Parameters.Add("userName", SqlDbType.NVarChar, 30).Value = username;
            SqlDataReader reader;
            errormsg = "";
            try
            {
                dBConnection.Open();
                // start and read from reader
                reader = dbCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user.Id = Convert.ToInt32(reader["Usr_Id"]);
                        user.userName = (string)reader["Usr_Name"];
                        user.email = (string)reader["Usr_Email"];
                        user.password = (string)reader["Usr_Password"];
                        user.isAdmin = Convert.ToBoolean(reader["Usr_IsAdmin"]);
                    }
                    reader.Close();
                    return user;
                }
                else
                {
                    errormsg = "Could not get user from database";
                    return null;
                }
               
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dBConnection.Close();
            }
        }
        public int DeleteUser(string username, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and add user to database

            String sqlsting = "DELETE FROM Tbl_User WHERE Usr_Name=@userName";

            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);
            dbCommand.Parameters.Add("userName", SqlDbType.NVarChar, 30).Value = username;

            try
            {
                dBConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1)
                {
                    errormsg = "";
                }
                else
                {
                    errormsg = "Could not delete user from database";
                }
                return i;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dBConnection.Close();
            }
        }

        public int UpdateUser(String username, TblUserDetails usr, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            


            // update all values 
            String sqlsting = "UPDATE Tbl_User SET Usr_Name = @newUserName, Usr_Email = @email, Usr_Password = @password WHERE Usr_Name=@userName";
            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);

            dbCommand.Parameters.Add("userName", SqlDbType.NVarChar, 30).Value = username;
            dbCommand.Parameters.Add("newUserName", SqlDbType.NVarChar, 30).Value = usr.userName;
            dbCommand.Parameters.Add("email", SqlDbType.NVarChar, 150).Value = usr.email;
            dbCommand.Parameters.Add("password", SqlDbType.NVarChar, 150).Value = usr.password;
            


            try
            {
                dBConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1)
                {
                    errormsg = "";
                }
                else
                {
                    errormsg = "Could not update user information";
                }
                return i;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dBConnection.Close();
            }
        }

       
    }
}
