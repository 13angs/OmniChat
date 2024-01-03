namespace OmniChat.Handlers
{
    public static class RequiredPropsHandler
    {
        public static bool HandleRequiredProps(string propName, string? propValue)
        {
            if(!string.IsNullOrEmpty(propValue))
            {
                return true;
            }
            throw new BadHttpRequestException($"{propName} is required!");
        }
    }
}