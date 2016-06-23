using HypermediaClient.Hypermedia.Siren.Models;

namespace HypermediaClient.Hypermedia.Siren
{
    public static class Proxy
    {
        public static Entity PrependHref(string url, Entity entity)
        {
            ReplaceLink(url, entity);
            ReplaceAction(url, entity);
            ReplaceEntities(url, entity);
            return entity;
        }

        private static void ReplaceLink(string url, Entity entity)
        {
            if (entity.Links == null) return;

            foreach (var link in entity.Links)
            {
                link.Href = $"{url}{link.Href}";
            }
        }

        private static void ReplaceAction(string url, Entity entity)
        {
            if (entity.Actions == null) return;

            foreach (var action in entity.Actions)
            {
                action.Href = $"{url}{action.Href}";
            }
        }

        private static void ReplaceEntities(string url, Entity entity)
        {
            if (entity.Entities == null) return;

            foreach (var subEntity in entity.Entities)
            {
                PrependHref(url, subEntity);
            }
        }
    }
}