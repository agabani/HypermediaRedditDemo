using System.Collections.Generic;

namespace RedditHypermediaClient.Extensions
{
    public static class ReadOnlyDictionaryExtensions
    {
        public static dynamic Get(this IReadOnlyDictionary<string, dynamic> dictionary, string key)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : null;
        }
    }
}