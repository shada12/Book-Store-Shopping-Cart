using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BookStore : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //get all books in the catalog.
            List<Book> books = BookCatalogDataAccess.GetAllBooks();
            foreach (Book book in books)
            {
                //todo: Populate dropdown list selections 
                ListItem item = new ListItem(book.Title);
                drpBookSelection.Items.Add(item);

            }
        }

        ShoppingCart shoppingcart = null;
        if (Session["shoppingcart"] == null)
        {
            shoppingcart = new ShoppingCart();

            //todo: add cart to the session
                Session["shoppingcart"] = shoppingcart;
            
        }
        else
        {
            //todo: retrieve cart from the session
            shoppingcart = (ShoppingCart)Session["shoppingcart"];
               //or shoppingcart = Session["shoppingcart"] as ShoppingCart;

            foreach (BookOrder order in shoppingcart.BookOrders)
            {
                //todo: Remove the book in the order from the dropdown list

                drpBookSelection.Items.Remove(order.Book.Title);

            }
        }

        if (shoppingcart.NumOfItems == 0)
            lblNumItems.Text = "empty";
        else if (shoppingcart.NumOfItems == 1)
            lblNumItems.Text = "1 item";
        else
            lblNumItems.Text = shoppingcart.NumOfItems.ToString() + " items";
        
    }
    protected void drpBookSelection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpBookSelection.SelectedValue != "-1")
        {
            //string bookId = drpBookSelection.SelectedItem.Value;
              string bookTitle = drpBookSelection.SelectedItem.Value;


            // Book selectedBook = BookCatalogDataAccess.GetBookById(bookTitle);
             Book selectedBook = null;

            //
            foreach (Book book in BookCatalogDataAccess.GetAllBooks() ) {

                if (book.Title == bookTitle) {
                    selectedBook=book;
                }

            }
            //
            

            //todo: Add selected book to the session

            Session["selectedBook"] = selectedBook;

            //todo: Display the selected book's description and price 

            lblDescription.Text = selectedBook.Description;

            lblPrice.Text = selectedBook.Price.ToString();

            
        }
        else
        {
            //todo: Set description and price to blank
            lblDescription.Text = "";
            lblPrice.Text = "";

        }
    }
    protected void btnAddToCart_Click(object sender, EventArgs e)
    {
        if (drpBookSelection.SelectedValue != "-1" && Session["shoppingcart"] != null)
        {
            //todo: Retrieve selected book from the session
            Book selectedBook = (Book)Session["selectedBook"];

            //todo: get user entered quqntity

            int bookQuantity= Int32.Parse(txtQuantity.Text);

            //todo: Create a book order with selected book and quantity
            BookOrder bookOrder = new BookOrder(selectedBook, bookQuantity);

            //todo: Retrieve to cart from the session
            ShoppingCart shoppingcart = (ShoppingCart)Session["shoppingcart"];

            //todo: Add book order to the shopping cart
            shoppingcart.AddBookOrder(bookOrder);

            //todo: Remove the selected item from the dropdown list
            drpBookSelection.Items.Remove(selectedBook.Title);

            //todo: Set the dropdown list's selected value as "-1"
            drpBookSelection.SelectedValue = "-1";

            //todo: Set the description to show title and quantity of the book user added to the shopping cart

            lblDescription.Text = bookQuantity + ( bookQuantity == 1 ? " copy of ": " copies of ") + selectedBook.Title + " is added to the shopping cart";

            //todo: Update the number of items in shopping cart displayed next to the link to ShoppingCartView.aspx

            lblNumItems.Text= shoppingcart.NumOfItems.ToString();
        }
    }
}