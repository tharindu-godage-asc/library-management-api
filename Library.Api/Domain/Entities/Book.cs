namespace Library.Api.Domain.Entities
{
    public class Book
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = default!;
        public string Author { get; private set; } = default!;
        public string Isbn { get; private set; } = default!;
        public int PublishedYear { get; private set; }
        public int TotalCopies { get; private set; }
        public int AvailableCopies { get; private set; }

        private Book() { }

        public Book(string title, string author, string isbn, int publishedYear, int totalCopies)
        {
            if (publishedYear > DateTime.UtcNow.Year)
                throw new ArgumentException("Published year cannot be in the future.");
            if (totalCopies <= 0)
                throw new ArgumentException("Total Copies must be greater than 0");

            Title = title;
            Author = author;
            Isbn = isbn;
            PublishedYear = publishedYear;
            TotalCopies = totalCopies;
            AvailableCopies = totalCopies;
        }

        public void BorrowCopy()
        {
            if (AvailableCopies <= 0)
                throw new InvalidOperationException("No available copies.");
            AvailableCopies--;
        }

        public void ReturnCopy()
        {
            if (AvailableCopies >= TotalCopies)
                throw new InvalidOperationException("All copies already accounted for.");
            AvailableCopies++;
        }

        public void Update(
            string title,
            string author,
            string isbn,
            int publishedYear,
            int totalCopies)
        {
            if (publishedYear > DateTime.UtcNow.Year)
            {
                throw new ArgumentException(
                    "Published year cannot be in the future.");
            }

            if (totalCopies <= 0)
            {
                throw new ArgumentException(
                    "Total copies must be greater than 0.");
            }

            Title = title;
            Author = author;
            Isbn = isbn;
            PublishedYear = publishedYear;
            TotalCopies = totalCopies;

            if (AvailableCopies > totalCopies)
            {
                AvailableCopies = totalCopies;
            }
        }
    }
}
