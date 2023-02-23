using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Read_do.Models
{
    public class ListMethods
    {
        public ListMethods() { }

        public int insertEntry(ListDetails entry, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and add user to database
            String sqlsting = "INSERT INTO Tbl_List (List_Username, List_Isbn, List_Read) VALUES (@userName, @isbn, @isRead)";
            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);

            dbCommand.Parameters.Add("userName", SqlDbType.NVarChar, 30).Value = entry.username;
            dbCommand.Parameters.Add("isbn", SqlDbType.NVarChar, 300).Value = entry.isbn;
            dbCommand.Parameters.Add("isRead", SqlDbType.Bit).Value =entry.isRead;




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

                    errormsg = "Could not add book to personal list";
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
        
        //Använder istället GetBookEntryDetails för att hämta alla entryn i listan. Den funktionen hämtar även data om varje bok som databasen refererar till
        public List<ListDetails> GetList(string username, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();
            List<ListDetails> userList = new List<ListDetails>();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and users books from list from database
            String sqlsting = "SELECT * FROM Tbl_List WHERE List_Username=@userName";
            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);
            dbCommand.Parameters.Add("userName", SqlDbType.NVarChar, 30).Value = username;


            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet dataSet = new DataSet();

            int i = 0;
            int count = 0;
            errormsg = "";

            try
            {
                //start and read from adapter
                dBConnection.Open();
                myAdapter.Fill(dataSet, "Tbl_list");
                count = dataSet.Tables["Tbl_llist"].Rows.Count;
                if (count > 0)
                {
                    while (i < count)
                    {
                        ListDetails entry = new ListDetails();
                        entry.id = Convert.ToInt16(dataSet.Tables["Tbl_List"].Rows[i]["List_Id"]);
                        entry.username = (string)dataSet.Tables["Tbl_list"].Rows[i]["List_Username"];
                        entry.isbn = (string)dataSet.Tables["Tbl_list"].Rows[i]["List_Isbn"];
                        entry.isRead = Convert.ToBoolean(dataSet.Tables["Tbl_list"].Rows[i]["List_Read"]);
                        entry.score = Convert.ToInt16(dataSet.Tables["Tbl_list"].Rows[i]["List_Score"]);

                        userList.Add(entry);
                        i++;


                    }
                    errormsg = "";
                    return userList;
                }
                else
                {
                    errormsg = "Could not get personal bookslist";
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

        //DELETE FROM LIST
        public int DeleteEntry(int id, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and delete entry from database

            String sqlsting = "DELETE FROM Tbl_List WHERE List_Id=@id";

            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;



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
                    errormsg = "Could not delete entry from database";
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

        public int UpdateScore(int id, int score, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // update score -> assumes books is read
            String sqlsting = "UPDATE Tbl_List SET List_Score = @score, List_Read ='true' WHERE List_Id=@id";
            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);

            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;
            dbCommand.Parameters.Add("score", SqlDbType.Int).Value = score;

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
                    errormsg = "Could not update entry information";
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
