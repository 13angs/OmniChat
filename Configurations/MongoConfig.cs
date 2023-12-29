namespace OmniChat.Configurations
{
    public class MongoConfig
    {
        public string? ConnectionString { get; set; }
        public string? DbName { get; set; }
        public MongoCollectionConfig? Collections { get; set; }
    }
    public class MongoCollectionConfig
    {
        public string? UserCols { get; set; }
        public string? ProviderCols { get; set; }
        public string? UserChannelCols { get; set; }
        public string? UserFriendCols { get; set; }
    }
}