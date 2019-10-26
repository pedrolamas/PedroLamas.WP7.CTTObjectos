using Microsoft.Phone.Controls;
using PedroLamas.WP7.CTTObjectos.ViewModel;

namespace PedroLamas.WP7.CTTObjectos.View
{
    public partial class ScanBarcodePage : PhoneApplicationPage
    {
        public ScanBarcodePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var vm = DataContext as ScanBarcodeViewModel;

            if (vm != null)
            {
                vm.StartScanning();
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            var vm = DataContext as ScanBarcodeViewModel;

            if (vm != null)
            {
                vm.StopScanning();
            }

            base.OnNavigatingFrom(e);
        }
    }
}