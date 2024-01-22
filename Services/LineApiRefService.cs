using Newtonsoft.Json;
using OmniChat.Models;

namespace OmniChat.Services
{
    public static class LineApiRefService
    {
        private const string ApiEndpoint = "https://api.line.me";
        public static async Task<LineBotInfo> GetLineBotInfo(string accessToken)
        {
            string botInfoEndpoint = $"{ApiEndpoint}/v2/bot/info";

            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            HttpResponseMessage response = await httpClient.GetAsync(botInfoEndpoint);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response from Line API:\n{responseBody}");

                return JsonConvert.DeserializeObject<LineBotInfo>(responseBody)!;
            }
            throw new BadHttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }
    }

    public partial class LineBotInfo
    {
        [JsonProperty("userId")]
        public string? UserId { get; set; }

        [JsonProperty("basicId")]
        public string? BasicId { get; set; }

        [JsonProperty("displayName")]
        public string? DisplayName { get; set; }

        [JsonProperty("pictureUrl")]
        public string? PictureUrl { get; set; }

        [JsonProperty("chatMode")]
        public string? ChatMode { get; set; }

        [JsonProperty("markAsReadMode")]
        public string? MarkAsReadMode { get; set; }
    }
}