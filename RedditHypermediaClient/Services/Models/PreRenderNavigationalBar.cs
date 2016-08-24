using System.Collections.Generic;

namespace RedditHypermediaClient.Services.Models
{
    public class PreRenderNavigationalBar
    {
        public PreRenderLink Brand { get; set; }
        public IEnumerable<PreRenderLink> Links { get; set; }
        public IEnumerable<PreRenderAction> Actions { get; set; }
    }
}