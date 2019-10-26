using PedroLamas.WP7.ServiceModel;

namespace PedroLamas.WP7.CTTObjectos.Model
{
    public interface ICttObjectTrackingService
    {
        void GetCttObjectTrackingStatus(string objectId, ResultCallback<CttObjectTrackingState> callback, object state);

        void GetCttObjectTrackingStatus(string objectId, string etag, ResultCallback<CttObjectTrackingState> callback, object state);
    }
}