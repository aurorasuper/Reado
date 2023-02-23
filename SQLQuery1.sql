SELECT books_list.Image_URL_L, books_list.Book_Title, books_list.Book_Author, books_list.Year_Of_Publication, books_list.Publisher, Tbl_List.List_Read, Tbl_List.List_Score
FROM Tbl_List 
INNER JOIN books_list ON Tbl_list.List_isbn = books_list.ISBN AND Tbl_List.List_Username='test'; 