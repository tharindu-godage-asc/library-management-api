namespace Library.Api.Domain.Entities
{
    public class Member
    {
        public int Id { get; private set; }
        public string FullName { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PhoneNumber { get; private set; } = default!;
        public DateTime RegisteredDate { get; private set; }
        public bool IsActive { get; private set; }

        private Member() { }

        public Member(string fullName, string email, string phoneNumber)
        {
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;

            RegisteredDate = DateTime.UtcNow;
            IsActive = true;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Update(
            string fullName,
            string email,
            string phoneNumber,
            bool isActive)
                {
                    FullName = fullName;
                    Email = email;
                    PhoneNumber = phoneNumber;
                    IsActive = isActive;
                }
    }
}
