using System.Collections.Generic;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace PedroLamas.WP7.CTTObjectos.Model
{
    public class MainModel : ObservableObject, IMainModel
    {
        private const string FILENAME = @"data.txt";

        private readonly IStorageService _storageService;

        #region Properties

        public IList<ObjectModel> TrackedObjects { get; private set; }

        #endregion

        public MainModel(IStorageService storageService)
        {
            _storageService = storageService;

            Load();
        }

        private void Load()
        {
            if (_storageService.FileExists(FILENAME))
                TrackedObjects = JsonConvert.DeserializeObject<List<ObjectModel>>(_storageService.ReadAllText(FILENAME));
            else
                TrackedObjects = new List<ObjectModel>();
        }

        public void Save()
        {
            _storageService.WriteAllText(FILENAME, JsonConvert.SerializeObject(TrackedObjects));
        }
    }
}