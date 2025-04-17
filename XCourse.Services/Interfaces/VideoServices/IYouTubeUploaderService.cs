namespace XCourse.Services.Implementations.VideoServices
{
    public interface IYouTubeUploaderService
    {
        Task<string> UploadVideoAsync(Stream videoStream, string title, string description);
    }
}
