using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;

namespace Read_do.Models
{
    public class BookEntryDetails
    {
        public BookEntryDetails() { }

        public int id { get; set; }

        [Display(Name = "ISBN")]
        public string isbn { get; set; }
        public string imageUrl { get; set; }
        
        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "Author")]
        public string author { get; set; }

        [Display(Name = "Year")]
        public int publishedYear { get; set; }

        [Display(Name = "Publisher")]
        public string publisher { get; set; }

        [Display(Name = "Read")]
        public Boolean isRead { get; set; }

        [Display(Name = "Score")]

        [Range(1, 5, ErrorMessage = "Score must be between $1 and $5")]
        public int score { get; set; }

       public List<BookEntryDetails> GetBookEntryDetails(string username, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();
            
            
            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and list of books related to user with bookdata from bookdatabase
            String sqlsting = "SELECT books_list.ISBN, books_list.Image_URL_L, books_list.Book_Title, books_list.Book_Author, books_list.Year_Of_Publication, books_list.Publisher, Tbl_List.List_Read, Tbl_List.List_Score, Tbl_List.List_Id "
                                + "FROM Tbl_List "
                                +"INNER JOIN books_list ON Tbl_list.List_isbn = books_list.ISBN AND Tbl_List.List_Username =@username";

            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);
            dbCommand.Parameters.Add("username", SqlDbType.NVarChar, 30).Value = username;

            //list of entries to return and prepare error message for return.
            List<BookEntryDetails> bookDetails = new List<BookEntryDetails>();
            errormsg = "";

            try
            {
                dBConnection.Open();
                // read from reader
                SqlDataReader reader;
                reader = dbCommand.ExecuteReader();

                //check reader has recieved anything 
                if (reader.HasRows)
                {
                    
                    while (reader.Read())
                    {
                        BookEntryDetails book = new BookEntryDetails();
                        book.isbn = (string)reader["ISBN"];
                        //some images are missing, check if null before converting
                        if(reader["Image_URL_L"] != DBNull.Value)
                        {
                            book.imageUrl = (string)reader["Image_URL_L"];
                        }
                        
                        book.title = (string)reader["Book_Title"];
                        book.author = (string)reader["Book_Author"];

                        //some years are missing, check if null before converting
                        if(reader["Year_Of_Publication"] != DBNull.Value)
                        {
                            book.publishedYear = Convert.ToInt32(reader["Year_Of_Publication"]);
                        }
                        
                        book.publisher = (string)reader["Publisher"];
                        book.isRead = Convert.ToBoolean(reader["List_Read"]);

                        //check if null
                        if(reader["List_Score"] != DBNull.Value)
                        {
                            book.score = Convert.ToInt16(reader["List_Score"]);
                        }
                        book.id = Convert.ToInt16(reader["List_Id"]);
                        bookDetails.Add(book);
                    }
                    reader.Close();
                    return bookDetails;
                }
                else
                {
                    errormsg = "Could not get Read list from database";
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


        public BookEntryDetails GetABookEntry(int id, out string errormsg)
        {
            // create connection
            SqlConnection dBConnection = new SqlConnection();


            //establish connection to SQL server
            dBConnection.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Books;Integrated Security=True;";

            // sql string and list of books related to user with bookdata from bookdatabase
            String sqlsting = "SELECT books_list.ISBN, books_list.Image_URL_L, books_list.Book_Title, books_list.Book_Author, books_list.Year_Of_Publication, books_list.Publisher, Tbl_List.List_Read, Tbl_List.List_Score, Tbl_List.List_Id "
                                + "FROM Tbl_List "
                                + "INNER JOIN books_list ON Tbl_list.List_isbn = books_list.ISBN AND Tbl_List.List_Id =@id";

            SqlCommand dbCommand = new SqlCommand(sqlsting, dBConnection);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;

            
            errormsg = "";

            try
            {
                dBConnection.Open();
                // read from reader
                SqlDataReader reader;
                reader = dbCommand.ExecuteReader();
                BookEntryDetails book = new BookEntryDetails();
                //check reader has recieved anything 
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        
                        book.isbn = (string)reader["ISBN"];
                        //some images are missing, check if null before converting
                        if (reader["Image_URL_L"] != DBNull.Value)
                        {
                            book.imageUrl = (string)reader["Image_URL_L"];
                        }

                        book.title = (string)reader["Book_Title"];
                        book.author = (string)reader["Book_Author"];

                        //some years are missing, check if null before converting
                        if (reader["Year_Of_Publication"] != DBNull.Value)
                        {
                            book.publishedYear = Convert.ToInt32(reader["Year_Of_Publication"]);
                        }

                        book.publisher = (string)reader["Publisher"];
                        book.isRead = Convert.ToBoolean(reader["List_Read"]);

                        //check if null
                        if (reader["List_Score"] != DBNull.Value)
                        {
                            book.score = Convert.ToInt16(reader["List_Score"]);
                        }
                        book.id = Convert.ToInt16(reader["List_Id"]);
                        
                    }
                    reader.Close();
                    return book;
                }
                else
                {
                    errormsg = "Could not get entry from list database";
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
    }

    
}
