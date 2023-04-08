using OnlineStore.DAL.Enum;

namespace OnlineStore.BLL.ViewModels.Account
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public string Login { get; set; }

        public AccountStatus Status { get; set; } = AccountStatus.NotActivated;
    }
}
