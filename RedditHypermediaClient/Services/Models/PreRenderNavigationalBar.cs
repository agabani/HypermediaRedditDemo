using System.Collections.Generic;

namespace RedditHypermediaClient.Services.Models
{
    public class PreRenderNavigationalBar
    {
        public PreRenderLink Brand { get; set; }
        public List<PreRenderLink> Links { get; set; }
        public List<PreRenderAction> Actions { get; set; }
    }
}