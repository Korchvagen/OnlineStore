using OnlineStore.DAL.Enum;

namespace OnlineStore.DAL.Models
{
    public class Accounts
    {
        public int Id { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public string Login { get; set; }

        public string Password { get; set; }

        public AccountStatus Status { get; set; } = AccountStatus.NotActivated;

        public virtual Cart Cart { get; set; }
    }
}
