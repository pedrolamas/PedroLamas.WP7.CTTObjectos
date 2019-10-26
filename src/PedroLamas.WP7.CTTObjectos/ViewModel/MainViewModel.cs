using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Cimbalino.Phone.Toolkit.Extensions;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PedroLamas.WP7.CTTObjectos.Message;
using PedroLamas.WP7.CTTObjectos.Model;

namespace PedroLamas.WP7.CTTObjectos.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IMainModel _model;
        private readonly ICttObjectTrackingService _cttObjectTrackingService;
        private readonly INavigationService _navigationService;
        private readonly ISystemTrayService _systemTrayService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly IClipboardService _clipboardService;
        private readonly IEmailComposeService _emailComposeService;
        private readonly ISmsComposeService _smsComposeService;

        private bool _isSelectionEnabled;
        private bool _isTrackedObjectsEmpty;

        #region Properties

        public ObservableCollection<ObjectDetailsViewModel> TrackedObjects { get; private set; }

        public bool IsTrackedObjectsEmpty
        {
            get
            {
                return _isTrackedObjectsEmpty;
            }
            set
            {
                if (_isTrackedObjectsEmpty == value)
                    return;

                _isTrackedObjectsEmpty = value;

                RaisePropertyChanged(() => IsTrackedObjectsEmpty);

                EnableSelectionCommand.RaiseCanExecuteChanged();
                RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsSelectionEnabled
        {
            get
            {
                return _isSelectionEnabled;
            }
            set
            {
                if (_isSelectionEnabled == value)
                    return;

                _isSelectionEnabled = value;

                RaisePropertyChanged(() => IsSelectionEnabled);
            }
        }

        public RelayCommand<ObjectDetailsViewModel> ShowObjectDetailsCommand { get; private set; }

        public RelayCommand NewObjectCommand { get; private set; }

        public RelayCommand EnableSelectionCommand { get; private set; }

        public RelayCommand RefreshCommand { get; private set; }

        public RelayCommand<IList> DeleteObjectsCommand { get; private set; }

        public RelayCommand ShowAboutCommand { get; private set; }

        public RelayCommand<CancelEventArgs> BackKeyPressCommand { get; private set; }

        public RelayCommand<ObjectDetailsViewModel> CopyCodeCommand { get; private set; }

        public RelayCommand<ObjectDetailsViewModel> MailCodeCommand { get; private set; }

        public RelayCommand<ObjectDetailsViewModel> TextCodeCommand { get; private set; }

        public bool IsBusy
        {
            get
            {
                return _systemTrayService.IsBusy;
            }
        }

        #endregion

        public MainViewModel(IMainModel model, ICttObjectTrackingService cttObjectTrackingService, INavigationService navigationService, ISystemTrayService systemTrayService, IMessageBoxService messageBoxService, IClipboardService clipboardService, IEmailComposeService emailComposeService, ISmsComposeService smsComposeService)
        {
            _model = model;
            _cttObjectTrackingService = cttObjectTrackingService;
            _navigationService = navigationService;
            _systemTrayService = systemTrayService;
            _messageBoxService = messageBoxService;
            _clipboardService = clipboardService;
            _emailComposeService = emailComposeService;
            _smsComposeService = smsComposeService;

            TrackedObjects = _model.TrackedObjects
                .Select(x => new ObjectDetailsViewModel(x))
                .ToObservableCollection();

            ShowObjectDetailsCommand = new RelayCommand<ObjectDetailsViewModel>(item =>
            {
                var objectId = item.Model.ObjectId;

                _navigationService.NavigateTo(new Uri("/View/ObjectDetailsPage.xaml?" + objectId, UriKind.Relative));
            });

            NewObjectCommand = new RelayCommand(() =>
            {
                _navigationService.NavigateTo(new Uri("/View/NewObjectPage.xaml", UriKind.Relative));
            }, () => !IsBusy);

            EnableSelectionCommand = new RelayCommand(() =>
            {
                IsSelectionEnabled = true;
            }, () => !IsTrackedObjectsEmpty && !IsBusy);

            RefreshCommand = new RelayCommand(() =>
            {
                var enumerator = TrackedObjects.GetEnumerator();

                RefreshNext(enumerator);
            }, () => !IsTrackedObjectsEmpty && !IsBusy);

            DeleteObjectsCommand = new RelayCommand<IList>(items =>
            {
                if (items == null || items.Count == 0)
                    return;

                _messageBoxService.Show("Está prestes a apagar os objectos seleccionados", "Tem a certeza?", new string[] { "eliminar", "cancelar" }, button =>
                {
                    if (button != 0)
                        return;

                    var itemsToRemove = items
                        .Cast<ObjectDetailsViewModel>()
                        .ToArray();

                    foreach (var item in itemsToRemove)
                    {
                        _model.TrackedObjects.Remove(item.Model);

                        TrackedObjects.Remove(item);
                    }

                    IsTrackedObjectsEmpty = (TrackedObjects.Count == 0);

                    _model.Save();
                });
            }, items => !IsBusy);

            ShowAboutCommand = new RelayCommand(() =>
            {
                _navigationService.NavigateTo(new Uri("/View/AboutPage.xaml", UriKind.Relative));
            });

            BackKeyPressCommand = new RelayCommand<CancelEventArgs>(e =>
            {
                if (IsSelectionEnabled)
                {
                    IsSelectionEnabled = false;

                    e.Cancel = true;
                }
            });

            CopyCodeCommand = new RelayCommand<ObjectDetailsViewModel>(item =>
            {
                _clipboardService.SetText(item.Model.ObjectId);
            });

            MailCodeCommand = new RelayCommand<ObjectDetailsViewModel>(item =>
            {
                _emailComposeService.Show("CTT Objectos", string.Format("\nO seu código de tracking: {0}\n\nPode consultar o estado entrando em http://www.ctt.pt e utilizando a opção \"Pesquisa de Objectos\".\n\nEnviado por CTT Objectos (http://bit.ly/cttobjectos)", item.Model.ObjectId));
            });

            TextCodeCommand = new RelayCommand<ObjectDetailsViewModel>(item =>
            {
                _smsComposeService.Show(string.Empty, item.Model.ObjectId);
            });

            MessengerInstance.Register<AddObjectMessage>(this, message =>
            {
                var objectViewModel = TrackedObjects.FirstOrDefault(x => x.Model.ObjectId == message.Model.ObjectId);

                if (objectViewModel == null)
                {
                    var objectModel = new ObjectModel(message.Description, message.ETag, message.Model);

                    _model.TrackedObjects.Add(objectModel);

                    TrackedObjects.Add(new ObjectDetailsViewModel(objectModel));

                    IsTrackedObjectsEmpty = false;
                }
                else
                {
                    objectViewModel.Model.Description = message.Description;
                    objectViewModel.Model.State = message.Model;
                }

                _model.Save();
            });

            IsTrackedObjectsEmpty = (TrackedObjects.Count == 0);
        }

        private void RefreshNext(IEnumerator<ObjectDetailsViewModel> enumerator)
        {
            if (enumerator.MoveNext())
            {
                var objectModel = enumerator.Current.Model;

                var objectId = objectModel.ObjectId;
                var etag = objectModel.ETag;

                _systemTrayService.SetProgressIndicator(string.Format("A actualizar: {0}...", objectId));

                _cttObjectTrackingService.GetCttObjectTrackingStatus(objectId, etag, result =>
                {
                    if (result.Error != null)
                    {
                        _systemTrayService.HideProgressIndicator();

                        _messageBoxService.Show(string.Format("Não foi possível actualizar o objecto \"{0}\"", objectId), "Erro");

                        return;
                    }
                    else if (result.StatusCode != System.Net.HttpStatusCode.NotModified)
                    {
                        objectModel.ETag = result.ETag;
                        objectModel.State = result.Data;
                    }

                    RefreshNext(enumerator);
                }, null);
            }
            else
            {
                enumerator.Dispose();

                _systemTrayService.HideProgressIndicator();

                _model.Save();
            }
        }
    }
}