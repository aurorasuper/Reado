using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Read_do.Models
{
    public class BooksMethods
    {
        public BooksMethods() { }

        public List<BookDetails> Select(out string errormsg)
        {
            
            List<BookDetails> bookDetails = new List<BookDetails>();

            // create connection
            SqlConnection dBConnection = new SqlConnection();
            TblUserDetails user = new TblUserDetails();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and get user from database
            String sqlString = "SELECT TOP 100 * FROM books_list";
            SqlCommand dbCommand = new SqlCommand(sqlString, dBConnection);
           
            SqlDataAdapter myAdapter= new SqlDataAdapter(dbCommand);

            
            DataSet dataSet = new DataSet();

            int i = 0;
            int count = 0;
            errormsg = "";
            try
            {
                dBConnection.Open();
                myAdapter.Fill(dataSet, "books_list");
                count = dataSet.Tables["books_list"].Rows.Count;
                if (count > 0)
                {
                    while (i < count)
                    {
                        BookDetails book = new BookDetails();
                        book.isbn = (string)dataSet.Tables["books_list"].Rows[i]["ISBN"];
                        book.title = (string)dataSet.Tables["books_list"].Rows[i]["Book_Title"];
                        book.author = (string)dataSet.Tables["books_list"].Rows[i]["Book_Author"];

                        // fix error where "Object cannot be cast from DBNull to other types."
                        if (dataSet.Tables["books_list"].Rows[i]["Year_Of_Publication"] != DBNull.Value)
                        {
                            book.publishedYear = Convert.ToInt32(dataSet.Tables["books_list"].Rows[i]["Year_Of_Publication"]);
                        }

                        book.publisher = (string)dataSet.Tables["books_list"].Rows[i]["Publisher"];

                        // fix error where "Object cannot be cast from DBNull to other types."
                        if (dataSet.Tables["books_list"].Rows[i]["Image_URL_L"] != DBNull.Value)
                        {
                            book.imageUrl = (string)dataSet.Tables["books_list"].Rows[i]["Image_URL_L"];
                        }
                        else { book.imageUrl = "broken"; }

                        // dont add to list if DBNull 
                        if (dataSet.Tables["books_list"].Rows[i]["Year_Of_Publication"] != DBNull.Value)
                        {
                            bookDetails.Add(book);
                        }
                        i++;
                    }
                    errormsg = "";
                    return bookDetails;
                }
                else
                {
                    errormsg = "Could not get books";
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

        public List<BookDetails> YearFilter( string option, int year, out string errormsg)
        {
            List<BookDetails> bookDetails = new List<BookDetails>();

            // create connection
            SqlConnection dBConnection = new SqlConnection();
            TblUserDetails user = new TblUserDetails();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";
            

            String sqlString;
            // sql string and get user from database based on user option
            if(option == "before")
            {
                sqlString = "SELECT TOP 1000 * FROM books_list WHERE Year_Of_Publication < @year";
            }else if (option == "after")
            {
                sqlString = "SELECT TOP 1000* FROM books_list WHERE Year_Of_Publication > @year";
            }
            else
            {
                sqlString = "SELECT TOP 1000* FROM books_list WHERE Year_Of_Publication = @year";
            }
            
            SqlCommand dbCommand = new SqlCommand(sqlString, dBConnection);
            dbCommand.Parameters.Add("year", SqlDbType.SmallInt).Value = year;
            

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);


            DataSet dataSet = new DataSet();

            int i = 0;
            int count = 0;
            errormsg = "";
            try
            {
                dBConnection.Open();
                myAdapter.Fill(dataSet, "books_list");
                count = dataSet.Tables["books_list"].Rows.Count;
                if (count > 0)
                {
                    while (i < count)
                    {
                        BookDetails book = new BookDetails();
                        book.isbn = (string)dataSet.Tables["books_list"].Rows[i]["ISBN"];
                        book.title = (string)dataSet.Tables["books_list"].Rows[i]["Book_Title"];
                        book.author = (string)dataSet.Tables["books_list"].Rows[i]["Book_Author"];

                        // fix error where "Object cannot be cast from DBNull to other types."
                        if (dataSet.Tables["books_list"].Rows[i]["Year_Of_Publication"] != DBNull.Value)
                        {
                            book.publishedYear = Convert.ToInt32(dataSet.Tables["books_list"].Rows[i]["Year_Of_Publication"]);
                        }

                        book.publisher = (string)dataSet.Tables["books_list"].Rows[i]["Publisher"];

                        // fix error where "Object cannot be cast from DBNull to other types."
                        if (dataSet.Tables["books_list"].Rows[i]["Image_URL_L"] != DBNull.Value)
                        {
                            book.imageUrl = (string)dataSet.Tables["books_list"].Rows[i]["Image_URL_L"];
                        }
                        else { book.imageUrl = "broken"; }

                        // dont add to list if DBNull 
                        if (dataSet.Tables["books_list"].Rows[i]["Year_Of_Publication"] != DBNull.Value)
                        {
                            bookDetails.Add(book);
                        }
                        i++;
                        
                        
                    }
                    errormsg = "";
                    return bookDetails;
                }
                else
                {
                    errormsg = "Could not get books";
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

        public List<BookDetails> Search(string search, out string errormsg)
        {
            List<BookDetails> bookDetails = new List<BookDetails>();

            // create connection
            SqlConnection dBConnection = new SqlConnection();
            TblUserDetails user = new TblUserDetails();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";


            String sqlString = "SELECT TOP 100 * FROM books_list WHERE Book_Author LIKE @searchword OR Book_Title LIKE @searchword OR Publisher LIKE @searchword";
          
            SqlCommand dbCommand = new SqlCommand(sqlString, dBConnection);
            dbCommand.Parameters.Add("searchword", SqlDbType.NVarChar,300).Value = "%"+search+"%";
            

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet dataSet = new DataSet();

            int i = 0;
            int count = 0;
            errormsg = "";
            try
            {
                dBConnection.Open();
                myAdapter.Fill(dataSet, "books_list");
                count = dataSet.Tables["books_list"].Rows.Count;
                if (count > 0)
                {
                    while (i < count)
                    {
                        BookDetails book = new BookDetails();
                        book.isbn = (string)dataSet.Tables["books_list"].Rows[i]["ISBN"];
                        book.title = (string)dataSet.Tables["books_list"].Rows[i]["Book_Title"];
                        book.author = (string)dataSet.Tables["books_list"].Rows[i]["Book_Author"];
                        book.publishedYear = Convert.ToInt32(dataSet.Tables["books_list"].Rows[i]["Year_Of_Publication"]);
                        book.publisher = (string)dataSet.Tables["books_list"].Rows[i]["Publisher"];
                        book.imageUrl = (string)dataSet.Tables["books_list"].Rows[i]["Image_URL_L"];
                        i++;
                        bookDetails.Add(book);

                    }
                    errormsg = "";
                    return bookDetails;
                }
                else
                {
                    errormsg = "Could not find anything containing name " + search;
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

       public List<BookDetails> Sort(string direction, string category, out string errormsg)
        {
            List<BookDetails> bookDetails = new List<BookDetails>();

            // create connection
            SqlConnection dBConnection = new SqlConnection();
            TblUserDetails user = new TblUserDetails();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";



            // sql string and get user from database based on user option
           
            String sqlString;

            
           
            /*The following erromessage was printed when: sqlString = "SELECT TOP 1000 * FROM books_list ORDER BY @Column DESC"; was used with: dbCommand.Parameters.Add("Column", SqlDbType.NVarChar).Value = category;
            * The SELECT item identified by the ORDER BY number 1 contains a variable as part of the expression identifying a column position.Variables are only allowed when ordering by an expression referencing a column name.*/
            // Try bypassing issue with if statements
            if (category == "Book_Title")
             {
                sqlString = "SELECT TOP 1000 * FROM books_list ORDER BY [Book_Title]";
             }else if(category == "Book_Author")
            {
                sqlString = "SELECT TOP 1000 * FROM books_list ORDER BY [Book_Author]";
            }else if(category == "Year_of_Publication")
            {
                sqlString = "SELECT TOP 1000 * FROM books_list ORDER BY [Year_Of_Publication]";
            }
            else
            {
                sqlString = "SELECT TOP 1000 * FROM books_list ORDER BY [Publisher]";
            }

            if (direction == "asc")
            {
                sqlString = sqlString + " ASC";
            }
            else
            {
                sqlString = sqlString + " DESC"; 
            }
            SqlCommand dbCommand = new SqlCommand(sqlString, dBConnection);

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);


            DataSet dataSet = new DataSet();

            int i = 0;
            int count = 0;
            errormsg = "";
            try
            {
                dBConnection.Open();
                myAdapter.Fill(dataSet, "books_list");
                count = dataSet.Tables["books_list"].Rows.Count;
                if (count > 0)
                {
                    while (i < count)
                    {
                        BookDetails book = new BookDetails();
                        book.isbn = (string)dataSet.Tables["books_list"].Rows[i]["ISBN"];
                        book.title = (string)dataSet.Tables["books_list"].Rows[i]["Book_Title"];
                        book.author = (string)dataSet.Tables["books_list"].Rows[i]["Book_Author"];

                        // fix error where "Object cannot be cast from DBNull to other types."
                        if (dataSet.Tables["books_list"].Rows[i]["Year_Of_Publication"] != DBNull.Value)
                        {
                            book.publishedYear = Convert.ToInt32(dataSet.Tables["books_list"].Rows[i]["Year_Of_Publication"]);
                        }

                        book.publisher = (string)dataSet.Tables["books_list"].Rows[i]["Publisher"];

                        // fix error where "Object cannot be cast from DBNull to other types."
                        if (dataSet.Tables["books_list"].Rows[i]["Image_URL_L"] != DBNull.Value)
                        {
                            book.imageUrl = (string)dataSet.Tables["books_list"].Rows[i]["Image_URL_L"];
                        }
                        else { book.imageUrl = "broken"; }

                        // dont add to list if DBNull 
                        if (dataSet.Tables["books_list"].Rows[i]["Year_Of_Publication"]!= DBNull.Value)
                        {
                            bookDetails.Add(book);
                        }

                        i++;


                    }
                    errormsg = "";
                    return bookDetails;
                }
                else
                {
                    errormsg = "Could not get books";
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

        public BookDetails GetBook(string isbn, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();
            BookDetails book = new BookDetails();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and get specific book from database from database
            String sqlsting = "SELECT * FROM books_list WHERE ISBN=@isbn";
            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);
            dbCommand.Parameters.Add("isbn", SqlDbType.NVarChar, 300).Value = isbn;
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
                        book.isbn = (string)reader["ISBN"];
                        book.title = (string)reader["Book_Title"];
                        book.author = (string)reader["Book_Author"];
                        book.publishedYear = Convert.ToInt32(reader["Year_Of_Publication"]);
                        book.publisher = (string)reader["Publisher"];
                        book.imageUrl = (string)reader["Image_URL_L"];
                    }
                    reader.Close();
                    return book;
                }
                else
                {
                    errormsg = "Could not get book from database";
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

        public int InsertBook(BookDetails book, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and add user to database
            String sqlsting = "INSERT INTO books_list (ISBN,Book_Title,Book_Author,Year_Of_Publication,Publisher,Image_URL_L) VALUES (@isbn, @title, @author, @year, @publisher, @url)";
            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);

            dbCommand.Parameters.Add("isbn", SqlDbType.NVarChar, 300).Value = book.isbn;
            dbCommand.Parameters.Add("title", SqlDbType.NVarChar, 300).Value = book.title;
            dbCommand.Parameters.Add("author", SqlDbType.NVarChar, 300).Value = book.author;
            dbCommand.Parameters.Add("year", SqlDbType.Int).Value = book.publishedYear;
            dbCommand.Parameters.Add("publisher", SqlDbType.NVarChar, 150).Value = book.publisher;
            dbCommand.Parameters.Add("url", SqlDbType.NVarChar, 150).Value = book.imageUrl;


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

                    errormsg = "Could not add book to database";
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

        public int UpdateBook(BookDetails book, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";




            // update all values - under the assumption that a books isbn is not updated
            String sqlsting = "UPDATE books_list SET Book_Title = @title, Book_Author = @author, Year_Of_Publication = @year, Publisher = @publisher, Image_URL_L = @url WHERE ISBN=@isbn";
            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);

            
            dbCommand.Parameters.Add("title", SqlDbType.NVarChar, 300).Value = book.title;
            dbCommand.Parameters.Add("author", SqlDbType.NVarChar, 300).Value = book.author;
            dbCommand.Parameters.Add("year", SqlDbType.Int).Value = book.publishedYear;
            dbCommand.Parameters.Add("publisher", SqlDbType.NVarChar, 150).Value = book.publisher;
            dbCommand.Parameters.Add("url", SqlDbType.NVarChar, 150).Value = book.imageUrl;
            dbCommand.Parameters.Add("isbn", SqlDbType.NVarChar, 300).Value = book.isbn;



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
                    errormsg = "Could not update book information";
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

        public int DeleteBook(string isbn, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();

            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and add user to database

            String sqlsting = "DELETE FROM books_list WHERE ISBN=@isbn";

            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);
            dbCommand.Parameters.Add("isbn", SqlDbType.NVarChar, 300).Value = isbn;



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
                    errormsg = "Could not delete book from database";
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
