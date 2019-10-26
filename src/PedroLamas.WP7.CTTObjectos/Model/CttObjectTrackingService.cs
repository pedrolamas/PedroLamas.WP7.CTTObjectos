using PedroLamas.WP7.ServiceModel;
using RestSharp;

namespace PedroLamas.WP7.CTTObjectos.Model
{
    public class CttObjectTrackingService : ICttObjectTrackingService
    {
        private readonly RestClient _client;

        public CttObjectTrackingService()
        {
            _client = new RestClient("http://svcs.pedrolamas.com/");

            _client.AddDefaultHeader("Accept", "text/json");
            _client.AddDefaultHeader("Accept-Encoding", "gzip");
        }

        public void GetCttObjectTrackingStatus(string objectId, ResultCallback<CttObjectTrackingState> callback, object state)
        {
            GetCttObjectTrackingStatus(objectId, null, callback, state);
        }

        public void GetCttObjectTrackingStatus(string objectId, string etag, ResultCallback<CttObjectTrackingState> callback, object state)
        {
            var request = new RestRequest("cttObjectos/v1/getObjectStatus", Method.GET);

            request.AddParameter("objectId", objectId);

            if (!string.IsNullOrEmpty(etag))
                request.AddHeader("If-None-Match", etag);

            _client.GetResultAsync(request, callback, state);
        }
    }
}