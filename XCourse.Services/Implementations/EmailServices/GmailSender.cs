using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using XCourse.Core.DTOs;

namespace XCourse.Services.Implementations.EmailServices
{
    public class GmailSender : IEmailSender
    {
        private readonly GmailSettings _gmailSettings;

        public GmailSender(IOptions<GmailSettings> gmailSettings)
        {
            _gmailSettings = gmailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var accessToken = await GetAccessTokenAsync();

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("XCourse Platform", _gmailSettings.EmailFrom));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = $"<html><body>{htmlMessage}</body></html>" };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(new SaslMechanismOAuth2(_gmailSettings.EmailFrom, accessToken));
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var credential = new UserCredential(
                new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = _gmailSettings.ClientId,
                        ClientSecret = _gmailSettings.ClientSecret
                    }
                }),
                "user",
                new TokenResponse { RefreshToken = _gmailSettings.RefreshToken });

            await credential.RefreshTokenAsync(CancellationToken.None);
            return credential.Token.AccessToken;
        }
    }
}
