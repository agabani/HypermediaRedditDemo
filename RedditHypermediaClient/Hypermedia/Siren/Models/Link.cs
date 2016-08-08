using System.Collections.Generic;

namespace RedditHypermediaClient.Hypermedia.Siren.Models
{
    public class Link
    {
        public IReadOnlyCollection<string> Rel { get; set; }
        public IReadOnlyCollection<string> Class { get; set; }
        public string Href { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
    }
}