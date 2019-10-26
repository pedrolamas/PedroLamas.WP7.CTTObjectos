using GalaSoft.MvvmLight;

namespace PedroLamas.WP7.CTTObjectos.Model
{
    public class ObjectModel : ObservableObject
    {
        private string _objectId;
        private string _description;
        private string _etag;
        private CttObjectTrackingState _state;

        #region Properties

        public string ObjectId
        {
            get
            {
                return _objectId;
            }
            set
            {
                if (_objectId == value)
                    return;

                _objectId = value;

                RaisePropertyChanged(() => ObjectId);
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description == value)
                    return;

                _description = value;

                RaisePropertyChanged(() => Description);
            }
        }

        public string ETag
        {
            get
            {
                return _etag;
            }
            set
            {
                if (_etag == value)
                    return;

                _etag = value;

                RaisePropertyChanged(() => ETag);
            }
        }

        public CttObjectTrackingState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state == value)
                    return;

                _state = value;

                RaisePropertyChanged(() => State);
            }
        }

        #endregion

        public ObjectModel()
        {
        }
        public ObjectModel(string description, string etag, CttObjectTrackingState state)
        {
            Description = description;
            ObjectId = state.ObjectId;
            ETag = etag;
            State = state;
        }
    }
}