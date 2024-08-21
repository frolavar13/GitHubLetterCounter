using GitHubLetterCounter.Interfaces;

namespace GitHubLetterCounter.Services
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubLetterCounter/1.0");
        }

        // Asynchronously sends an HTTP GET request to the specified URL and returns the response content as a string.
        // Re-throws the exception.
        public async Task<string> GetStringAsync(string url)
        {
            try
            {
                // Sends the HTTP GET request and returns the response content as a string.
                return await _httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException ex)
            {
                // Handles HTTP request-related errors.
                Logger.Error($"Error making HTTP request: {ex.Message}");
                throw;
            }
            catch (TaskCanceledException ex)
            {
                // Handles cases where the request was canceled or timed out.
                Logger.Error($"HTTP request timed out or was canceled: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handles any other unexpected errors.
                Logger.Error($"Unexpected error: {ex.Message}");
                throw;
            }
        }
    }


}
