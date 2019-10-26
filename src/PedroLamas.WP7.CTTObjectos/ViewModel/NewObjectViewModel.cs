using System;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PedroLamas.WP7.CTTObjectos.Message;
using PedroLamas.WP7.CTTObjectos.Model;

namespace PedroLamas.WP7.CTTObjectos.ViewModel
{
    public class NewObjectViewModel : ViewModelBase
    {
        private readonly ICttObjectTrackingService _cttObjectTrackingService;
        private readonly INavigationService _navigationService;
        private readonly ISystemTrayService _systemTrayService;
        private readonly IMessageBoxService _messageBoxService;

        private string _objectId;
        private string _description;

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

        public RelayCommand AddObjectCommand { get; private set; }

        public RelayCommand ScanBarcodeCommand { get; private set; }

        public bool IsBusy
        {
            get
            {
                return _systemTrayService.IsBusy;
            }
        }

        #endregion

        public NewObjectViewModel(ICttObjectTrackingService cttObjectTrackingService, INavigationService navigationService, ISystemTrayService systemTrayService, IMessageBoxService messageBoxService)
        {
            _cttObjectTrackingService = cttObjectTrackingService;
            _navigationService = navigationService;
            _systemTrayService = systemTrayService;
            _messageBoxService = messageBoxService;

            AddObjectCommand = new RelayCommand(() =>
            {
                if (string.IsNullOrWhiteSpace(ObjectId))
                {
                    _messageBoxService.Show("Por favor indique o número do objecto", "Erro");

                    return;
                }

                _systemTrayService.SetProgressIndicator(string.Format("A adicionar: {0}...", ObjectId));

                _cttObjectTrackingService.GetCttObjectTrackingStatus(ObjectId, result =>
                {
                    _systemTrayService.HideProgressIndicator();

                    if (result.Error != null)
                    {
                        _messageBoxService.Show(string.Format("Não foi possível adicionar o objecto \"{0}\"", ObjectId), "Erro");

                        ScanBarcodeCommand.RaiseCanExecuteChanged();
                        AddObjectCommand.RaiseCanExecuteChanged();
                    }
                    else
                    {
                        MessengerInstance.Send<AddObjectMessage>(new AddObjectMessage(_description, result.ETag, result.Data));

                        _navigationService.GoBack();

                        ObjectId = null;
                    }
                }, null);

                ScanBarcodeCommand.RaiseCanExecuteChanged();
                AddObjectCommand.RaiseCanExecuteChanged();
            }, () => !IsBusy);

            ScanBarcodeCommand = new RelayCommand(() =>
            {
                _navigationService.NavigateTo(new Uri("/View/ScanBarcodePage.xaml", UriKind.Relative));
            }, () => !IsBusy);

            MessengerInstance.Register<ScannedBarcodeMessage>(this, message =>
            {
                ObjectId = message.ObjectId;
            });
        }
    }
}