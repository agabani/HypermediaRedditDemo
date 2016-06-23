using System.Collections.Generic;

namespace HypermediaClient.Hypermedia.Siren.Models
{
    public class Action
    {
        public string Name { get; set; }
        public IReadOnlyCollection<string> Class { get; set; }
        public string Method { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public IReadOnlyCollection<Field> Fields { get; set; }
    }
}