using System;
using System.Collections.Generic;

namespace PedroLamas.WP7.CTTObjectos.Model
{
    public class CttObjectTrackingState
    {
        #region Properties

        public string ObjectId { get; set; }

        public string ObjectType { get; set; }

        public DateTime? Date { get; set; }

        public string State { get; set; }

        public List<CttObjectTrackingStateDetails> Details { get; set; }

        #endregion
    }
}