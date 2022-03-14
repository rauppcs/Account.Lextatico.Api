using System.Threading.Tasks;
using Account.Lextatico.Infra.Services.Models.EmailService;

namespace Account.Lextatico.Infra.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest emailRequest);
    }
}
