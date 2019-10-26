using PedroLamas.WP7.CTTObjectos.Model;

namespace PedroLamas.WP7.CTTObjectos.Message
{
    public class AddObjectMessage
    {
        #region Properties

        public string Description { get; private set; }

        public string ETag { get; private set; }

        public CttObjectTrackingState Model { get; private set; }

        #endregion

        public AddObjectMessage(string description, string etag, CttObjectTrackingState model)
        {
            Description = description;
            ETag = etag;
            Model = model;
        }
    }
}