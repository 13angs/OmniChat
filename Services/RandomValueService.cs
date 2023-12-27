namespace OmniChat.Services
{
    public static class RandomValueService
    {
        public static string GenerateRandomString(Random random, int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}