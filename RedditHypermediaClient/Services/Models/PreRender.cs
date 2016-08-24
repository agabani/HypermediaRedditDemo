using RedditHypermediaClient.Clients.Siren.Models;

namespace RedditHypermediaClient.Services.Models
{
    public class PreRender
    {
        public PreRenderNavigationalBar NavigationalBar { get; set; }
        public PreRenderPagination Pagination { get; set; }
        public Entity Entity { get; set; }
    }
}