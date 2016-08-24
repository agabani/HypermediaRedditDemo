using System.Linq;
using RedditHypermediaClient.Clients.Siren.Models;
using RedditHypermediaClient.Services.Models;

namespace RedditHypermediaClient.Services
{
    public class PreRenderService
    {
        public PreRender PreRender(Entity entity)
        {
            var preRender = new PreRender
            {
                NavigationalBar = PreRenderNavigationalBar(entity),
                Pagination = PreRenderPagination(entity),
                Entity = entity
            };

            return preRender;
        }

        private static PreRenderNavigationalBar PreRenderNavigationalBar(Entity entity)
        {
            var navigationalBrand = entity.Links.First(l => l.Class.Contains("navigation") && l.Class.Contains("brand"));
            var navigationalLinks = entity.Links.Where(l => l.Class.Contains("navigation") && !l.Class.Contains("brand"));
            var navigationalActions = entity.Actions.Where(a => a.Class.Contains("navigation"));

            return new PreRenderNavigationalBar
            {
                Brand = new PreRenderLink
                {
                    Text = navigationalBrand.Title,
                    Href = navigationalBrand.Href
                },
                Links = navigationalLinks.Select(l => new PreRenderLink
                {
                    Text = l.Title,
                    Href = l.Href
                }),
                Actions = navigationalActions.Select(a => new PreRenderAction
                {
                    Method = a.Method,
                    Action = a.Href,
                    Fields = a.Fields.Select(f => new PreRenderActionField
                    {
                        Id = f.Name,
                        Name = f.Name,
                        Value = f.Value,
                        Type = f.Type
                    }),
                    Button = new PreRenderActionButton
                    {
                        Type = "submit",
                        Text = a.Title
                    }
                })
            };
        }

        private static PreRenderPagination PreRenderPagination(Entity entity)
        {
            var previous = entity.Links.SingleOrDefault(l => l.Class.Contains("pagination") && l.Rel.Contains("previous"));
            var next = entity.Links.SingleOrDefault(l => l.Class.Contains("pagination") && l.Rel.Contains("next"));

            return new PreRenderPagination
            {
                Previous = previous != null ? new PreRenderLink {Href = previous.Href, Text = previous.Title} : null,
                Next = next != null ? new PreRenderLink {Href = next.Href, Text = next.Title} : null
            };
        }
    }
}