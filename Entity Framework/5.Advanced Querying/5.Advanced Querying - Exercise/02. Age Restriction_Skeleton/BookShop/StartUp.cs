namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Net.Http.Headers;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using BookShopContext db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //string command = "miNor";
            //2.Age Restriction
            //Console.WriteLine(GetBooksByAgeRestriction(db, command));

            //3.Golden books
            //Console.WriteLine(GetGoldenBooks(db));

            //4.Get books by price
            //Console.WriteLine(GetBooksByPrice(db));

            //5.Not Released In
            //int year = 1998;
            //Console.WriteLine(GetBooksNotReleasedIn(db,year));

            //6.Book Titles by Category
            //string input = "";
            //Console.WriteLine(GetBooksByCategory(db, input));

            //7.Released Before Date
            //string date = "12-04-1992";
            //Console.WriteLine(GetBooksReleasedBefore(db, date));

            //8.Author Search
            //string input = "dy";
            //Console.WriteLine(GetAuthorNamesEndingIn(db,input));

            //9.Book Search
            //string input = "WOR";
            //Console.WriteLine(GetBookTitlesContaining(db,input));

            //10.Book Search by Author
            //string input = "po";
            //Console.WriteLine(GetBooksByAuthor(db,input));

            //11.Count Books
            //int lengthCheck = 40;
            //Console.WriteLine(CountBooks(db, lengthCheck));

            //12.Total Book Copies
            //Console.WriteLine(CountCopiesByAuthor(db));

            //13.Profit by Category
            //Console.WriteLine(GetTotalProfitByCategory(db));

            //14.Most Recent Books
            //Console.WriteLine(GetMostRecentBooks(db));

            //15.Increase Prices
            //IncreasePrices(db);

            //16.Remove Books
            Console.WriteLine(RemoveBooks(db));
        }

        //2.Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {

            //TryParse ot enumeration of type AgeRestriction
            if (!Enum.TryParse(command, true, out AgeRestriction ageRestriction))
            {
                return string.Empty;
            }

            var bookTitle = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();


            return string.Join(Environment.NewLine, bookTitle);
        }

        //3.Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .OrderBy(b => b.BookId)
                .ToList();

            return string.Join(Environment.NewLine, goldenBooks.Select(b => b.Title));
        }

        //4.Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var booksByPrice = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToArray();



            return string.Join(Environment.NewLine, booksByPrice.Select(b => $"{b.Title} - ${b.Price:F2}"));
        }


        //5.Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksNotInYear = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .OrderBy(b => b.BookId)
                .ToArray();


            return string.Join(Environment.NewLine, booksNotInYear.Select(b => b.Title));
        }

        //6.Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.ToLower().Split(" ",StringSplitOptions.RemoveEmptyEntries);

            var booksFromCategory = context.BooksCategories
            .Where(bc=>categories.Contains(bc.Category.Name.ToLower()))
            .Select(bc => bc.Book.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine,booksFromCategory);
        }

        //7.Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {

            DateTime dateTocCompare = DateTime.ParseExact(date, "dd-MM-yyyy",CultureInfo.InvariantCulture);
            var booksBeforeDate = context.Books
                .Where(b => b.ReleaseDate.Value.Date < dateTocCompare)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToArray();

            return string.Join(Environment.NewLine, booksBeforeDate.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}"));
        }

        //8.Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(n=> n.FirstName.EndsWith(input))
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName
                })
                .OrderBy(a => a.FirstName)
                .ThenBy(a=>a.LastName)
                .ToArray();

            return string.Join(Environment.NewLine, authors.Select(a => $"{a.FirstName} {a.LastName}"));
        }

        //9.Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(t=>t.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //10.Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b=>b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(b => new
                {
                    b.Title,
                    b.Author.FirstName,
                    b.Author.LastName,
                    b.BookId
                })
                .OrderBy(b=>b.BookId)
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} ({b.FirstName} {b.LastName})"));
        }

        //11.Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var numberOfBooks = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToArray();
            return numberOfBooks.Length;
        }

        //12.Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    Copies = a.Books.Sum(a => a.Copies)
                })
                .OrderByDescending(a => a.Copies)
                .ToArray();


            return string.Join(Environment.NewLine, authors.Select(a => $"{a.FirstName} {a.LastName} - {a.Copies}"));
        }

        //13.Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Profit = c.CategoryBooks.Sum(p => p.Book.Price * (decimal)(p.Book.Copies))
                })
                .OrderByDescending(p=>p.Profit)
                .ThenBy(p=>p.Name)
                .ToArray();

            return string.Join(Environment.NewLine, categories.Select(a => $"{a.Name} ${a.Profit:F2}"));
        }

        //14.Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var booksByCategory = context.Categories
                .Select(b => new
                {
                    b.Name,
                    MostRecent = b.CategoryBooks
                    .OrderByDescending(b=>b.Book.ReleaseDate)
                    .Take(3)
                    .Select(b=> $"{b.Book.Title} ({b.Book.ReleaseDate.Value.Year})").ToArray()
                })
                .OrderBy(c=>c.Name)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var category in booksByCategory)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.MostRecent)
                {
                    sb.AppendLine($"{book}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //15.Increase Prices
        public static void IncreasePrices(BookShopContext context) 
        {
            //Get books in memory
            var booksToUpdate = context.Books
                .Where(b=>b.ReleaseDate.Value.Year < 2010)
                .ToArray();
            decimal increase = 5M;
            //Update books
            foreach (var book in booksToUpdate)
            {
                book.Price += increase;
            }

            //Save changes
            context.SaveChanges();
            
        }

        //16.Remove Books
        public static int RemoveBooks(BookShopContext context)
        {

            int numberOfCopies = 4200;

            var booksToRemove = context.Books
                .Where(b => b.Copies < numberOfCopies)
                .ToArray();

            context.RemoveRange(booksToRemove);

            context.SaveChanges();

            return booksToRemove.Length;
        }
    }


}


