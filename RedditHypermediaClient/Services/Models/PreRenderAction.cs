using System.Collections.Generic;

namespace RedditHypermediaClient.Services.Models
{
    public class PreRenderAction
    {
        public string Method { get; set; }
        public string Action { get; set; }
        public IEnumerable<PreRenderActionField> Fields { get; set; }
        public PreRenderActionButton Button { get; set; }
    }
}