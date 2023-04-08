using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Models;
using OnlineStore.BLL.ViewModels.Account;
using System.Security.Claims;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using OnlineStore.DAL.Enum;
using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Response;
using Microsoft.Extensions.Logging;
using OnlineStore.BLL.Interfaces;
using AutoMapper;

namespace OnlineStore.BLL.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IBaseRepository<Accounts> _baseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountsService> _logger;

        public AccountsService(IBaseRepository<Accounts> baseRepository, IMapper mapper, ILogger<AccountsService> logger)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<IEnumerable<AccountViewModel>>> GetAccounts()
        {
            _logger.LogInformation("[GetAccounts]");

            try
            {
                var accounts = await _baseRepository.GetAll<Accounts>().ToListAsync();

                if (accounts == null)
                {
                    return new BaseResponse<IEnumerable<AccountViewModel>>()
                    {
                        Description = "Found 0 items",
                        StatusCode = StatusCode.NoAccounts
                    };
                }

                IEnumerable<AccountViewModel> accountViewModels = _mapper.Map<IEnumerable<Accounts>, IEnumerable<AccountViewModel>>(accounts);

                return new BaseResponse<IEnumerable<AccountViewModel>>()
                {
                    Data = accountViewModels,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetAccounts]: {ex.Message}");

                return new BaseResponse<IEnumerable<AccountViewModel>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Accounts>> GetAccountById(int id)
        {
            _logger.LogInformation("[GetAccountById]");

            try
            {
                var account = await _baseRepository.GetAll<Accounts>().FirstOrDefaultAsync(a => a.Id == id);

                if (account == null)
                {
                    return new BaseResponse<Accounts>()
                    {
                        Description = "Account not found",
                        StatusCode = StatusCode.AccountNotFound
                    };
                }

                return new BaseResponse<Accounts>()
                {
                    Data = account,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetAccountById]: {ex.Message}");

                return new BaseResponse<Accounts>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteAccount(int id)
        {
            _logger.LogInformation("[DeleteAccount]");

            try
            {
                var account = await _baseRepository.GetAll<Accounts>().FirstOrDefaultAsync(a => a.Id == id);

                if (account == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "Account not found",
                        StatusCode = StatusCode.AccountNotFound
                    };
                }

                await _baseRepository.Delete(account);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteAccount]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Accounts>> GetAccountByLogin(string login)
        {
            _logger.LogInformation("[GetAccountByLogin]");

            try
            {
                var account = await _baseRepository.GetAll<Accounts>().FirstOrDefaultAsync(a => a.Login == login);

                if (account == null)
                {
                    return new BaseResponse<Accounts>()
                    {
                        Description = "Account not found",
                        StatusCode = StatusCode.AccountNotFound
                    };
                }

                return new BaseResponse<Accounts>()
                {
                    Data = account,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetAccountByLogin]: {ex.Message}");

                return new BaseResponse<Accounts>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Accounts>> Register(RegisterViewModel model)
        {
            _logger.LogInformation("[Register]");

            try
            {
                var loginCheckResult = await _baseRepository.GetAll<Accounts>().FirstOrDefaultAsync(a => a.Login == model.Login);

                if (loginCheckResult != null)
                {
                    return new BaseResponse<Accounts>()
                    {
                        Description = "User with this login already exists",
                        StatusCode = StatusCode.AccountAlreadyExist
                    };
                }

                var hashedPasswordResponse = await PasswordEncryption(model.Password);

                if (hashedPasswordResponse.StatusCode != StatusCode.OK)
                {
                    return new BaseResponse<Accounts>()
                    {
                        Description = "Password hash error",
                        StatusCode = StatusCode.InternalServerError
                    };
                }

                var account = new Accounts()
                {
                    Login = model.Login,
                    Password = hashedPasswordResponse.Data
                };
                await _baseRepository.Create(account);

                return new BaseResponse<Accounts>()
                {
                    Data = account,
                    Description = "Registration completed successfully",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Register]: {ex.Message}");

                return new BaseResponse<Accounts>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }

        }

        public async Task<BaseResponse<Accounts>> LogIn(LoginViewModel model)
        {
            _logger.LogInformation("[LogIn]");

            try
            {
                var account = await _baseRepository.GetAll<Accounts>().FirstOrDefaultAsync(a => a.Login == model.Login);

                if (account == null)
                {
                    return new BaseResponse<Accounts>()
                    {
                        Description = "Account not found",
                        StatusCode = StatusCode.AccountNotFound
                    };
                }

                var isPasswordsMatch = await PasswordComparission(account.Password, model.Password);

                if (isPasswordsMatch.Data != true)
                {
                    return new BaseResponse<Accounts>()
                    {
                        Description = "Wrong password",
                        StatusCode = StatusCode.WrongPassword
                    };
                }

                if (account.Status == AccountStatus.NotActivated)
                {
                    return new BaseResponse<Accounts>()
                    {
                        Data = account,
                        StatusCode = StatusCode.AccountNotActivated
                    };
                }

                return new BaseResponse<Accounts>()
                {
                    Data = account,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[LogIn]: {ex.Message}");

                return new BaseResponse<Accounts>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> SendEmail(string login, string mess, string subject)
        {
            _logger.LogInformation("[SendEmail]");

            try
            {
                MailAddress sender = new MailAddress("Korchvagen007@gmail.com", "Korchikov Konstantin");
                MailAddress receiver = new MailAddress(login, "Dear User");
                MailMessage message = new MailMessage(sender, receiver);
                SmtpClient smtpClient = new SmtpClient();

                message.Body = mess;
                message.IsBodyHtml = true;
                message.Subject = subject;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(sender.Address, "amqcctbeqabytjkb");
                smtpClient.Send(message);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[SendEmail]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<int>> CreateConfirmationCode()
        {
            _logger.LogInformation("[CreateConfirmationCode]");

            try
            {
                int code = new Random().Next(100000, 999999);

                return new BaseResponse<int>()
                {
                    Data = code,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateConfirmationCode]: {ex.Message}");

                return new BaseResponse<int>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Authentication(Accounts account)
        {
            _logger.LogInformation("[Authentication]");

            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, account.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role.ToString())
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = claimsIdentity,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Authentication]: {ex.Message}");

                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> AccountActivation(string login)
        {
            _logger.LogInformation("[AccountActivation]");

            try
            {
                var account = await _baseRepository.GetAll<Accounts>().FirstOrDefaultAsync(a => a.Login == login);

                if (account == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "Account not found",
                        StatusCode = StatusCode.AccountNotFound
                    };
                }

                account.Status = AccountStatus.Activated;
                await _baseRepository.Update(account);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AccountActivation]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> NewPassword(int id, string newPassword)
        {
            _logger.LogInformation("[NewPassword]");

            try
            {
                var account = await _baseRepository.GetAll<Accounts>().FirstOrDefaultAsync(a => a.Id == id);

                if (account == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "Account not found",
                        StatusCode = StatusCode.AccountNotFound
                    };
                }

                var hashedPassword = await PasswordEncryption(newPassword);

                if (hashedPassword.StatusCode != StatusCode.OK)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "Failed to hash password",
                        StatusCode = StatusCode.InternalServerError
                    };
                }

                account.Password = hashedPassword.Data;
                await _baseRepository.Update(account);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[AccountActivation]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> ChangeRole(int accountId, UserRole selectedRole)
        {
            _logger.LogInformation("[ChangeRole]");

            try
            {
                var account = await _baseRepository.GetAll<Accounts>().FirstOrDefaultAsync(a => a.Id == accountId);

                if (account == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.AccountNotFound,
                        Description = "Account not found"
                    };
                }

                account.Role = selectedRole;
                await _baseRepository.Update(account);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ChangeRole]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private async Task<BaseResponse<string>> PasswordEncryption(string password)
        {
            _logger.LogInformation("[PasswordEncryption]");

            try
            {
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                    return new BaseResponse<string>()
                    {
                        Data = hash,
                        StatusCode = StatusCode.OK
                    };
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[PasswordEncryption]: {ex.Message}");

                return new BaseResponse<string>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        private async Task<BaseResponse<bool>> PasswordComparission(string accountPassword, string logInPassword)
        {
            _logger.LogInformation("[PasswordComparission]");

            try
            {
                var hashedLogInPassword = await PasswordEncryption(logInPassword);

                if (hashedLogInPassword.StatusCode != StatusCode.OK)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "Failed to hash password",
                        StatusCode = StatusCode.InternalServerError
                    };
                }

                if (accountPassword != hashedLogInPassword.Data)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "Passwords don't match",
                        StatusCode = StatusCode.WrongPassword
                    };
                }

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[PasswordComparission]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }
    }
}
