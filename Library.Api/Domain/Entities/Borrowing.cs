using Library.Api.Domain.Enums;
using System.Net.NetworkInformation;

namespace Library.Api.Domain.Entities
{
    public class Borrowing
    {
        public int Id { get; private set; }

        public int BookId { get; private set; }
        public int MemberId { get; private set; }
        public DateTime BorrowedDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? ReturnedDate {  get; private set; }
        public BorrowingStatus Status { get; private set; }

        //Check whether book is over due or not
        public bool IsOverdue =>
             Status == BorrowingStatus.Borrowed &&
             DateTime.UtcNow > DueDate;

        private Borrowing() { }

        public Borrowing(int bookId, int memberId)
        {
            BookId = bookId;
            MemberId = memberId;

            BorrowedDate = DateTime.UtcNow;
            DueDate = BorrowedDate.AddDays(14);

            Status = BorrowingStatus.Borrowed;
        }

        public void MarkReturned()
        {
            if (Status == BorrowingStatus.Returned)
            
                throw new InvalidOperationException("Book has already been returned");

            ReturnedDate = DateTime.UtcNow;
            Status = BorrowingStatus.Returned;
            
        }


        public void MarkOverDue()
        {
            if (Status == BorrowingStatus.Borrowed && DateTime.UtcNow > DueDate)
            {
                Status = BorrowingStatus.Overdue;
            }
        }
    }
}
