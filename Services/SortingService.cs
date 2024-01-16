using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

namespace OmniChat.Services
{
    public static class SortingService
    {
        public static SortDefinition<T> Sort<T>(string[] sortFields, string[] sortOrders)
        {

            if (sortFields == null || sortOrders == null)
            {
                sortFields = new string[] { "modified_timestamp" };
                sortOrders = new string[] { "desc" };
            }

            // Build sort definitions dynamically based on request parameters
            var sortDefinitionBuilder = Builders<T>.Sort;
            var sortDefinitions = new List<SortDefinition<T>>();

            for (int i = 0; i < Math.Min(sortFields.Length, sortOrders.Length); i++)
            {
                var sortOrder = sortOrders[i].Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? sortDefinitionBuilder.Ascending(sortFields[i])
                    : sortDefinitionBuilder.Descending(sortFields[i]);

                sortDefinitions.Add(sortOrder);
            }

            // Combine sort definitions into a single sort definition
            return sortDefinitionBuilder.Combine(sortDefinitions);
        }
    }
}