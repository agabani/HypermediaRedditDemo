using System.Linq;
using RedditHypermediaClient.Clients.Siren.Models;
using RedditHypermediaClient.Services.Models;

namespace RedditHypermediaClient.Services
{
    public class PreRenderService
    {
        public PreRender PreRender(Entity entity)
        {
            var navigationalBrand = entity.Links.First(l => l.Class.Contains("navigation") && l.Class.Contains("brand"));
            var navigationalLinks = entity.Links.Where(l => l.Class.Contains("navigation") && !l.Class.Contains("brand"));
            var navigationalActions = entity.Actions.Where(a => a.Class.Contains("navigation"));

            var preRender = new PreRender
            {
                PreRenderNavigationalBar = new PreRenderNavigationalBar
                {
                    Brand = new PreRenderLink
                    {
                        Text = navigationalBrand.Title,
                        Href = navigationalBrand.Href
                    },
                    Links = navigationalLinks.Select(l => new PreRenderLink {Text = l.Title, Href = l.Href}).ToList(),
                    Actions = navigationalActions.Select(a => new PreRenderAction
                    {
                        Method = a.Method,
                        Action = a.Href,
                        Button = new PreRenderActionButton
                        {
                            Type = "submit",
                            Text = a.Title
                        },
                        Fields = a.Fields.Select(f => new PreRenderActionField
                        {
                            Id = f.Name,
                            Name = f.Name,
                            Value = f.Value,
                            Type = f.Type
                        }).ToList()
                    }).ToList()
                },
                Entity = entity
            };

            return preRender;
        }
    }
}