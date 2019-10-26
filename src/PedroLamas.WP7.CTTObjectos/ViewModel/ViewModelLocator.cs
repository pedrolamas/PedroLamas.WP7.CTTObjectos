using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using PedroLamas.WP7.CTTObjectos.Model;

namespace PedroLamas.WP7.CTTObjectos.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<IStorageService, StorageService>();
            SimpleIoc.Default.Register<IMessageBoxService, MessageBoxService>();
            SimpleIoc.Default.Register<ISystemTrayService, SystemTrayService>();
            SimpleIoc.Default.Register<IVibrationService, VibrationService>();
            SimpleIoc.Default.Register<IPhotoCameraService, PhotoCameraService>();
            SimpleIoc.Default.Register<IShareLinkService, ShareLinkService>();
            SimpleIoc.Default.Register<IMarketplaceReviewService, MarketplaceReviewService>();
            SimpleIoc.Default.Register<IMarketplaceSearchService, MarketplaceSearchService>();
            SimpleIoc.Default.Register<IWebBrowserService, WebBrowserService>();
            SimpleIoc.Default.Register<IClipboardService, ClipboardService>();
            SimpleIoc.Default.Register<IEmailComposeService, EmailComposeService>();
            SimpleIoc.Default.Register<ISmsComposeService, SmsComposeService>();

            SimpleIoc.Default.Register<ICttObjectTrackingService, CttObjectTrackingService>();
            SimpleIoc.Default.Register<IMainModel, MainModel>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<NewObjectViewModel>();
            SimpleIoc.Default.Register<ScanBarcodeViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public NewObjectViewModel NewObject
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewObjectViewModel>();
            }
        }

        public ScanBarcodeViewModel ScanBarcode
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ScanBarcodeViewModel>();
            }
        }

        public AboutViewModel About
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}