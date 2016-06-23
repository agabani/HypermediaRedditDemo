using System.Collections.Generic;

namespace HypermediaClient.Hypermedia.Siren.Models
{
    public class Entity
    {
        public IReadOnlyCollection<string> Class { get; set; }
        public IReadOnlyCollection<string> Rel { get; set; }
        public IReadOnlyDictionary<string, dynamic> Properties { get; set; }
        public IReadOnlyCollection<Entity> Entities { get; set; }
        public IReadOnlyCollection<Link> Links { get; set; }
        public IReadOnlyCollection<Action> Actions { get; set; }
        public string Href { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
    }
}