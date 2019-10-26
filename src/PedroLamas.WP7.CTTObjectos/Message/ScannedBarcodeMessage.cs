namespace PedroLamas.WP7.CTTObjectos.Message
{
    public class ScannedBarcodeMessage
    {
        #region Properties

        public string ObjectId { get; private set; }

        #endregion

        public ScannedBarcodeMessage(string objectId)
        {
            ObjectId = objectId;
        }
    }
}