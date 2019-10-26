using GalaSoft.MvvmLight;
using PedroLamas.WP7.CTTObjectos.Model;

namespace PedroLamas.WP7.CTTObjectos.ViewModel
{
    public class ObjectDetailsViewModel : ViewModelBase
    {
        #region Properties

        public ObjectModel Model { get; private set; }

        public string Description
        {
            get
            {
                var description = Model.Description;

                if (string.IsNullOrEmpty(description))
                    return Model.ObjectId;

                return description;
            }
        }

        #endregion

        public ObjectDetailsViewModel(ObjectModel model)
        {
            Model = model;
        }
    }
}