namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
            string input = Console.ReadLine();
            Console.WriteLine(GetBooksByAuthor(db, input));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            int ageRestriction;
            if (command.ToLower() == "minor")
            {
                ageRestriction = 0;
            }
            else if (command.ToLower() == "teen")
            {
                ageRestriction = 1;
            }
            else
            {
                ageRestriction = 2;
            }

            var books = context.Books
                .Where(a => ((int)a.AgeRestriction) == ageRestriction)
                .Select(e => new
                {
                    e.Title
                })
                .OrderBy(t => t.Title)
                .ToArray();
            StringBuilder output = new();

            foreach (var book in books)
            {
                output.AppendLine($"{book.Title}");
            }

            return output.ToString().Trim();
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                .Where(i => i.EditionType == EditionType.Gold && i.Copies < 5000)
                .Select(e => new
                {
                    e.BookId,
                    e.Title
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            string result = string.Join(Environment.NewLine, goldenBooks.Select(e => e.Title));
            return result;
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            var expensiveBooks = context.Books
                .Where(p => p.Price > 40)
                .Select(e => new
                {
                    e.Title,
                    e.Price
                })
                .OrderByDescending(p => p.Price)
                .ToArray();

            StringBuilder sb = new();

            foreach (var b in expensiveBooks)
            {
                sb.AppendLine($"{b.Title} - ${b.Price:f2}");
            }

            return sb.ToString().Trim();
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(y => y.ReleaseDate.Value.Year != year)
                .Select(e => new
                {
                    e.BookId,
                    e.Title
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            string result = string.Join(Environment.NewLine, books.Select(e => e.Title));
            return result;
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] catagories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c=>c.ToLower())
                .ToArray();

            var books = context.Books
                .Where(x => x.BookCategories.Any(bc => catagories.Contains(bc.Category.Name.ToLower())))
                .Select(e => new
                {
                    e.Title
                })
                .OrderBy(t => t.Title)
                .ToArray();

            string result = string.Join(Environment.NewLine, books.Select(e => e.Title));
            return result;
                
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime.TryParse(date, out DateTime beforeDate);

            var books = context.Books
                .Where(rd => rd.ReleaseDate < beforeDate)
                .Select(e => new
                {
                    e.Title,
                    e.EditionType,
                    e.Price,
                    e.ReleaseDate

                })
                .OrderByDescending(r => r.ReleaseDate)
                .ToArray();

            StringBuilder output = new();

            foreach (var b in books)
            {
                output.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:f2}");
            }

            return output.ToString().Trim();
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(fn => fn.FirstName.EndsWith(input))
                .Select(e => new
                {
                    FullName = e.FirstName + ' ' + e.LastName
                })
                .OrderBy(fn => fn.FullName)
                .ToArray();

            string result = string.Join(Environment.NewLine, authors.Select(e => e.FullName));
            return result;
        }
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(t => t.Title.ToLower().Contains(input.ToLower()))
                .Select(e => new
                {
                    e.Title
                })
                .OrderBy(t => t.Title)
                .ToArray();

            string result = string.Join(Environment.NewLine, books.Select(e => e.Title));
            return result;
        }
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(a => a.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(e => new
                {
                    e.Title,
                    AuthorName = e.Author.FirstName + ' ' + e.Author.LastName,
                    e.BookId
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            StringBuilder output = new();
            foreach (var b in books)
            {
                output.AppendLine($"{b.Title} ({b.AuthorName})");
            }
            return output.ToString().Trim();
        }
    }
}


