using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using Newtonsoft.Json;
using RedditHypermediaClient.Clients.Siren.Models;

namespace RedditHypermediaClient.Clients.Siren
{
    public class SirenClient
    {
        private readonly Uri _baseAddress;

        public SirenClient(Uri baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public Entity Get(string relativeUri)
        {
            using (var httpClient = HttpClient())
            using (var httpResponseMessage = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, relativeUri)).GetAwaiter().GetResult())
            {
                return DeserializeObject(httpResponseMessage);
            }
        }

        public Entity Post(string relativeUri, FormCollection formCollection)
        {
            using (var httpClient = HttpClient())
            using (var httpResponseMessage = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, relativeUri)
            {
                Content = new FormUrlEncodedContent(formCollection.AllKeys.ToDictionary(k => k, v => formCollection[v]))
            }).GetAwaiter().GetResult())
            {
                return DeserializeObject(httpResponseMessage);
            }
        }

        private HttpClient HttpClient()
        {
            return new HttpClient
            {
                BaseAddress = _baseAddress,
                DefaultRequestHeaders = {Accept = {new MediaTypeWithQualityHeaderValue("application/vnd.siren+json")}}
            };
        }

        private static Entity DeserializeObject(HttpResponseMessage httpResponseMessage)
        {
            return JsonConvert.DeserializeObject<Entity>(httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
    }
}