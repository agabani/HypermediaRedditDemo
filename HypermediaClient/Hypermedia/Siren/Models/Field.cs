using System.Collections.Generic;

namespace HypermediaClient.Hypermedia.Siren.Models
{
    public class Field
    {
        public string Name { get; set; }
        public IReadOnlyCollection<string> Class { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Title { get; set; }
    }
}