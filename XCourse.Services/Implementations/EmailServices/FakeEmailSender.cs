using Microsoft.AspNetCore.Identity.UI.Services;

namespace XCourse.Services.Implementations.EmailServices
{
    public class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
