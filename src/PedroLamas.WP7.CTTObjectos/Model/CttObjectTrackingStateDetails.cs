using System;

namespace PedroLamas.WP7.CTTObjectos.Model
{
    public class CttObjectTrackingStateDetails
    {
        #region Properties

        public DateTime? Date { get; set; }

        public string State { get; set; }

        public string Reason { get; set; }

        public string Location { get; set; }

        public string Receiver { get; set; }

        #endregion
    }
}