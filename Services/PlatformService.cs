using OmniChat.Models;

namespace OmniChat.Services
{
    public class PlatformService
    {
        public PlatformResponse GetPlatforms()
        {
            PlatformResponse response = new()
            {
                Platforms = Enum.GetNames(typeof(Platform)).ToList(),
                PlatformNames = Enum.GetNames(typeof(Platform)).ToDictionary(p => p, p => p)
            };
            return response;
        }
    }
}