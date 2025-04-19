using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;

namespace XCourse.Services.Implementations.VideoServices
{
    public class YouTubeUploaderService : IYouTubeUploaderService
    {
        private readonly IConfiguration _configuration;

        public YouTubeUploaderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadVideoAsync(Stream videoStream, string title, string description)
        {
            var refreshToken = _configuration["YouTube:RefreshToken"];
            var clientId = _configuration["GmailSettings:ClientId"];
            var clientSecret = _configuration["GmailSettings:ClientSecret"];

            var token = new TokenResponse
            {
                RefreshToken = refreshToken
            };

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                Scopes = new[] { YouTubeService.Scope.YoutubeUpload }
            });

            var credentialWithToken = new UserCredential(flow, "user", token);
            await credentialWithToken.RefreshTokenAsync(CancellationToken.None);

            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentialWithToken,
                ApplicationName = "YourApp"
            });

            var video = new Video
            {
                Snippet = new VideoSnippet
                {
                    Title = title,
                    Description = description,
                    CategoryId = "27" // Education
                },
                Status = new VideoStatus
                {
                    PrivacyStatus = "unlisted"
                }
            };

            var videoInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", videoStream, "video/*");

            await videoInsertRequest.UploadAsync();

            if (videoInsertRequest.ResponseBody == null)
            {
                return null;
            }
            else
            {
                var videoId = videoInsertRequest.ResponseBody.Id;
                return $"https://www.youtube.com/embed/{videoId}";
            }
        }
    }
}
