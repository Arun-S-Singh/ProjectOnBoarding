using ProjectOnBoarding.Models;

namespace ProjectOnBoarding.Services.Email
{
    public interface IEmailService
    {
        void SendEmail(EmailDto email);
    }
}
