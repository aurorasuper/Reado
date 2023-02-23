using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Read_do.Models;

namespace Read_do.Controllers
{
    public class UserTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.DeleteUser = TempData["DeletedUser"];
            return View();
        }

        [HttpPost]
        public IActionResult Login(TblUserDetails user)
        {
            TblUserMethods pm = new TblUserMethods();
            string error;
            Boolean check = pm.LogInCheck(user, out error);

            if (check)
            {
                //set username to session variable
                HttpContext.Session.SetString("isLoggedIn", user.userName);
                string errormsg;
                if (pm.IsAdmin(user.userName, out errormsg))
                {
                    HttpContext.Session.SetString("isAdmin", "true");
                    return RedirectToAction("BrowseAdmin");
                }
                else
                {
                    return RedirectToAction("MyBooks", "UserTest");
                }
            }
            else
            {
                ViewBag.LoginError = error;
                return View("Login");
            }
            
        }

        
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("isLoggedIn");
            HttpContext.Session.Remove("isAdmin");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(TblUserDetails user, string verifyPassword)
        {
            TblUserMethods pm = new TblUserMethods();
            int i;
            string error;

            if(user.password == verifyPassword)
            {
                i = pm.InsertUser(user, out error);
                // CREATE different constraint messages for email and for username 
                if (error.Contains("UN_Tbl_User"))
                {
                    ViewBag.NameError = "Username is already taken.";
                }

                if (error.Contains("UE_Tbl_User"))
                {
                    ViewBag.EmailError = "Email is already registered.";
                }


                if (i == 1)
                {
                    ViewBag.RegisteredUser = "Your account was successuflly registered!";
                    return RedirectToAction("Login");
                }
                else { return View("Register"); }
            }
            else
            {
                ViewBag.PasswordError = "Passwords do not match";
                return View("Register");
            }
            
           
        }

        [HttpGet]
        public IActionResult Profile()
        {
            string username = HttpContext.Session.GetString("isLoggedIn");
            string errormessage ="";
            TblUserMethods userMethods = new TblUserMethods();
            TblUserDetails user = userMethods.GetUser(username, out errormessage);
            ViewBag.getUser = errormessage;
           
            
            return View(user);
        }

        [HttpGet]
        public IActionResult EditProfile(string username)
        {
            
            string errormessage = "";
            TblUserMethods userMethods = new TblUserMethods();
            TblUserDetails user = userMethods.GetUser(username, out errormessage);
            ViewBag.Error = errormessage;
            

            return View(user);
        }
        [HttpPost]
        public IActionResult EditProfile(TblUserDetails newUserDetails)
        {
            string username = HttpContext.Session.GetString("isLoggedIn");
            string errormessage = "";
            TblUserMethods userMethods = new TblUserMethods();
            TblUserDetails user = userMethods.GetUser(username, out errormessage);
            ViewBag.getUser = errormessage;
            
            int i = 0;
            string updateErrorMessage;
            //Verify passwords
            if(newUserDetails.password == user.password)
            {
                
                user.userName = newUserDetails.userName;
                user.email = newUserDetails.email;
                i = userMethods.UpdateUser(username, user, out updateErrorMessage);
                if (i ==0)
                {
                    if (updateErrorMessage.Contains("UN_Tbl_User"))
                    {
                        ViewBag.UpdateUser = "Username is already taken.";
                    }else if (updateErrorMessage.Contains("UE_Tbl_User"))
                    {
                        ViewBag.UpdateUser = "Email is already registered.";
                    }
                    else
                    {
                        ViewBag.UpdateUser = updateErrorMessage;
                    }
                    
                    
                }
                else {
                    ViewBag.UpdateUser = "Successfully updated user";
                    HttpContext.Session.SetString("isLoggedIn", user.userName); 
                }
            }
            else
            {
                ViewBag.FailedPasswordCheck = "Incorrect password.";
            }
            return View("EditProfile", user);
        }

        [HttpPost]
        public IActionResult ChangePassword(IFormCollection collection)
        {
            // get all form data
            string currentPassword = collection["currentPassword"];
            string newPassword = collection["newPassword"];
            string verifyPassword = collection["verifyPassword"];

            // get userdata and print eventual errors
            string username = HttpContext.Session.GetString("isLoggedIn");
            string errormessage = "";
            TblUserMethods userMethods = new TblUserMethods();
            TblUserDetails user = userMethods.GetUser(username, out errormessage);

            string passwordErrorMessage;
            string passwordMessage ="";
            int i = 0;
            

            //verify current password: match -> verify new password. else display error message
            if (currentPassword == user.password)
            {
                if(newPassword == verifyPassword)
                {
                    user.password = newPassword;
                    i = userMethods.UpdateUser(username, user, out passwordErrorMessage);
                    if (i == 0)
                    {
                        if (passwordErrorMessage.Contains("PL_TBL_User"))
                        {
                            passwordMessage = "New password must contain atleast 6 characters";
                        }
                        else
                        {
                            passwordMessage = passwordErrorMessage;
                        }
                        

                    }
                    else
                    {
                        ViewBag.passwordSuccess = "Successfully updated password";
                        
                    }
                }
                else
                {
                    passwordMessage = "New password does not match";
                }
            }
            else
            {
                passwordMessage = "Current password was incorrect";
            }

            ViewBag.passwordError = passwordMessage;
            return View("EditProfile",user);
        }
        
        [HttpGet]
        public IActionResult DeleteProfile(string username)
        {
            
            string errormessage = "";
            TblUserMethods userMethods = new TblUserMethods();
            TblUserDetails user = userMethods.GetUser(username, out errormessage);
            ViewBag.getUser = errormessage;
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteProfile()
        {
            string username = HttpContext.Session.GetString("isLoggedIn");
            string errormessage = "";
            TblUserMethods userMethods = new TblUserMethods();
            
            ViewBag.getUser = errormessage;
            int i = 0;
            string deleteMessage;
            i = userMethods.DeleteUser(username, out deleteMessage);
            if(i == 1)
            {
                
                TempData["DeletedUser"] = "Your account was successfully deleted";
                HttpContext.Session.Remove("isLoggedIn");
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.DeleteUser = deleteMessage;
                return View("DeleteProfile");
            }
            
        }
        [HttpGet]
        public IActionResult BrowseBooks()
        {
            // check if user is admin -> redirect to admin page 
            TblUserMethods tblUserMethods = new TblUserMethods();
            string errormsg;
            if (tblUserMethods.IsAdmin(HttpContext.Session.GetString("isLoggedIn"),out errormsg)){
                return RedirectToAction("BrowseAdmin");
            }
            BooksMethods booksMethods = new BooksMethods();
            
            
            List<BookDetails> books;
            books = booksMethods.Select(out errormsg);
            ViewBag.Error = errormsg;
            //unfiltered, unsearched, un sorted

            ViewBag.Filter = "";
            ViewBag.Author = "";
            ViewBag.Sort = "";
            ViewBag.Book = TempData["CRUDmsg"];
            return View(books);
        }      

        [HttpPost]
        public IActionResult YearFilter(IFormCollection fc)
        {
            string option = fc["YearFilter"];
            int year = Convert.ToInt16(fc["Year"]);
            
            BooksMethods booksMethods = new BooksMethods();
            string errormsg;
            List<BookDetails> books = booksMethods.YearFilter(option,year, out errormsg);
            
            ViewBag.Author = "";
            ViewBag.Sort = "";
            if (errormsg == "")
            {
                ViewBag.Filter = "Showing books published " + option + " year " + year;
            }
            else
            {
                ViewBag.Filter = errormsg;
            }

            //If admin -> return admin view
            TblUserMethods tblUserMethods = new TblUserMethods();
            
            if (tblUserMethods.IsAdmin(HttpContext.Session.GetString("isLoggedIn"), out errormsg))
            {
                return View("BrowseAdmin",books);
            }
            else
            {
                return View("BrowseBooks", books);
            }
            
        }

        [HttpPost]
        public IActionResult Sort(IFormCollection fc)
        {
            BooksMethods booksMethods = new BooksMethods();
            string cat = fc["SortCat"];
            string dir = fc["SortDir"];
            ViewBag.Sort = "Sort books by category " + cat + " in" + dir + " direction";
            string errormsg;
            List<BookDetails> books = booksMethods.Sort(dir,cat,out errormsg);
            
            ViewBag.Filter = "";
            ViewBag.Author = "";
            if (errormsg == "")
            {
                ViewBag.Sort = "Sort books by category " + cat+ " in "+ dir + " direction";
            }
            else
            {
                ViewBag.Sort = errormsg;
            }

            //If admin -> return admin view
            TblUserMethods tblUserMethods = new TblUserMethods();

            if (tblUserMethods.IsAdmin(HttpContext.Session.GetString("isLoggedIn"), out errormsg))
            {
                return View("BrowseAdmin", books);
            }
            else
            {
                return View("BrowseBooks", books);
            }
        }


        public IActionResult Search(IFormCollection fc)
        {
            BooksMethods booksMethods = new BooksMethods();
            string author = fc["searchAuthor"];
            string errormsg;

            List<BookDetails> books = booksMethods.Search(author, out errormsg);
           
            if(errormsg == "")
            {
                ViewBag.Author = "Show books that contain " + author;
            }
            else
            {
                ViewBag.Author = errormsg;
            }

            ViewBag.Filter = "";
            ViewBag.Sort = "";

            //If admin -> return admin view
            TblUserMethods tblUserMethods = new TblUserMethods();

            if (tblUserMethods.IsAdmin(HttpContext.Session.GetString("isLoggedIn"), out errormsg))
            {
                ViewBag.Error = errormsg;
                return View("BrowseAdmin", books);
            }
            else
            {
                return View("BrowseBooks", books);
            }


        }

        [HttpPost]
        public IActionResult ConfirmAddToList(IFormCollection fc)
        {
            BooksMethods booksMethods = new BooksMethods();
            string errormsg;
            string isbn = fc["ISBN"];
            ViewBag.Isbn = isbn;
            // add isbn and username to bookslist table
            if (HttpContext.Session.GetString("isLoggedIn") == null)
            {
                ViewBag.Add = "Please log in to add this item to your list";
            }
            else
            {
                BookDetails book = new BookDetails();
                book = booksMethods.GetBook(isbn,out errormsg);
                if (book != null)
                {
                    ViewBag.Add = "Do you want to add: " + book.title +" to your list?";
                   
                    return View(book);
                }
                else
                {
                    ViewBag.Add = errormsg;
                    return View();
                }
            }
            


            // redirect back to browse books
            return View();
        }

        [HttpPost]
        public IActionResult AddToList(IFormCollection fc)
        {
            ListMethods listMethods = new ListMethods();
            string username = HttpContext.Session.GetString("isLoggedIn");
            string errormsg;
            string isbn = fc["ISBN"];
            ViewBag.Isbn = isbn;
            int i = 0;
            ListDetails entry = new ListDetails();
            entry.username = username;
            entry.isbn = isbn;
            entry.isRead = false;

            i = listMethods.insertEntry(entry, out errormsg);
            if(i == 1)
            {
                return RedirectToAction("BrowseBooks");
            }
            else
            {
                ViewBag.AddError = errormsg;
                return View("ConfirmAddToList");
            }

        }


        public IActionResult MyBooks()
        {
            string username = HttpContext.Session.GetString("isLoggedIn");
            string errormsg;
            if (username == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Score = HttpContext.Session.GetString("UpdateEntry");
            ViewBag.Delete = HttpContext.Session.GetString("DeleteEntry");
            //create BookEntryDetail item to get access to method
            BookEntryDetails bed = new BookEntryDetails();
            List<BookEntryDetails> list = bed.GetBookEntryDetails(username, out errormsg);
            if(list != null)
            {
                return View(list);
            }
            else
            {
                ViewBag.Error = errormsg;
                return View();
            }
            
        }

        [HttpPost]
        public IActionResult UpdateScore(IFormCollection fc)
        {
            int score = Convert.ToInt32(fc["score"]);
            int id = Convert.ToInt32(fc["id"]);
            int i = 0;
            string errormsg; 

           // to access methods
           ListMethods listMethods = new ListMethods();    

            //update and give feedback -> printed through viewbag in MyBooks
            i = listMethods.UpdateScore(id, score, out errormsg);
            if(i == 1)
            {
                HttpContext.Session.SetString("UpdateEntry", "Score was updated");

            }
            else
            {
                HttpContext.Session.SetString("UpdateEntry", errormsg);
            }

            return RedirectToAction("MyBooks");
        }


        public IActionResult BrowseAdmin()
        {
            BooksMethods booksMethods = new BooksMethods();


            List<BookDetails> books;
            string errormsg;
            books = booksMethods.Select(out errormsg);
            ViewBag.Error = errormsg;

            //unfiltered, unsearched, un sorted

            ViewBag.Filter = "";
            ViewBag.Author = "";
            ViewBag.Sort = "";
            ViewBag.Book = TempData["CRUDmsg"];
            return View(books);
            
        }
        public IActionResult ConfirmDeleteEntry(int id)
        {
           
            string errormsg =""; 
            BookEntryDetails entry = new BookEntryDetails();
            entry = entry.GetABookEntry(id, out errormsg);

            if(entry != null)
            {
                return View(entry);
            }
            else
            {
                ViewBag.GetEntry = errormsg;
                return View();
            }
            
        }

        [HttpPost]
        public IActionResult DeleteEntry(int id)
        {
            int i = 0;
            string errormsg = "";
            ListMethods listMethods = new ListMethods();

            i = listMethods.DeleteEntry(id,out errormsg);

            if (i ==1)
            {
                HttpContext.Session.SetString("DeleteEntry", "Deleted item from your list");
                return RedirectToAction("MyBooks");
            }
            else
            {
                ViewBag.DeleteEntry = errormsg;
                return View("ConfirmDeleteEntry");
            }

        }

        [HttpGet]
        public IActionResult AddBook()
        {
            string username = HttpContext.Session.GetString("isLoggedIn");
            if(username == null)
            {
                TempData["DeletedUser"] = "Please log in as admin to access this page";
                return RedirectToAction("Login");
            }
            string errormsg;
            TblUserMethods tblUserMethods = new TblUserMethods();
            
            if(tblUserMethods.IsAdmin(username,out errormsg))
            {
                return View();
            }
            else
            {
                if(errormsg == "")
                {
                    TempData["DeletedUser"] = "Please log in as admin to access this page";
                }
                else
                {
                    TempData["DeletedUser"] = errormsg;
                }
                
                return RedirectToAction("Login");
            }
            
        }

        [HttpPost]
        public IActionResult AddBook(BookDetails book)
        {
            BooksMethods booksMethods = new BooksMethods();
            string errormsg;
            int i = 0;
            i = booksMethods.InsertBook(book, out errormsg);
            if (i == 1)
            {
                TempData["CRUDmsg"] = "Succesfully added '" + book.title +"' by " + book.author + " to the database";
                return RedirectToAction("BrowseAdmin");
            }
            else
            {
                // Check if key is duplicate
                if(errormsg.Contains("Cannot insert duplicate key in object 'dbo.books_list'"))
                {
                    BookDetails bookDetails = new BookDetails();
                    string getBookError;
                    bookDetails = booksMethods.GetBook(book.isbn, out getBookError);
                    ViewBag.AddError = "A book with this Isbn already exists, with title '" + book.title + "' by " + book.author;
                }
                else
                {
                    ViewBag.AddError = errormsg;
                }
                
                return View("AddBook");
            }
        }

        [HttpGet]
        public IActionResult UpdateBook(string isbn)
        {
            BookDetails book;
            BooksMethods booksMethods = new BooksMethods();
            string errormsg;
            book = booksMethods.GetBook(isbn, out errormsg);
            ViewBag.Error = errormsg;
            return View(book);
        }

        [HttpPost]
        public IActionResult UpdateBook(BookDetails book)
        {
            BooksMethods booksMethods = new BooksMethods();
            string errormsg;
            int i = 0;
            i = booksMethods.UpdateBook(book, out errormsg);
            if (i == 1)
            {
                TempData["CRUDmsg"] = "Succesfully updated '" + book.title + "' by " + book.author + " to the database";
                return RedirectToAction("BrowseAdmin");
            }
            else
            {
                ViewBag.Error = errormsg;
                return View(book);
            }
            
        }
       
        [HttpGet]
        public IActionResult DeleteBook(string isbn)
        {
            BookDetails book;
            BooksMethods booksMethods = new BooksMethods();
            string errormsg;
            book = booksMethods.GetBook(isbn, out errormsg);
            ViewBag.Error = errormsg;
            
            return View(book);
        }

        [HttpPost]
        public IActionResult ConfirmDeleteBook(string isbn)
        {
            BooksMethods booksMethods = new BooksMethods();
            string errormsg;
            int i = 0;
            BookDetails book = booksMethods.GetBook(isbn,out errormsg);

            i = booksMethods.DeleteBook(isbn, out errormsg);
            if (i == 1)
            {
                TempData["CRUDmsg"] = "Succesfully deleted '" + book.title + "' by " + book.author + " to the database";
                return RedirectToAction("BrowseAdmin");
            }
            else
            {
                ViewBag.isbnError = isbn;
                ViewBag.Error = errormsg;
                return View("DeleteBook",book);
            }

        }
    }

        
}
