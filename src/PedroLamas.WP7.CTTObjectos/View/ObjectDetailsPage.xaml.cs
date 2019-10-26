using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Phone.Controls;
using PedroLamas.WP7.CTTObjectos.Model;
using PedroLamas.WP7.CTTObjectos.ViewModel;

namespace PedroLamas.WP7.CTTObjectos.View
{
    public partial class ObjectDetailsPage : PhoneApplicationPage
    {
        public ObjectDetailsPage()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Threading.Thread.CurrentThread.CurrentCulture.Name);

            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                if (!SimpleIoc.Default.Contains<IMainModel>())
                {
                    MessageBox.Show("MainModel missing");

                    return;
                }

                var url = e.Uri.ToString();
                var itemUrl = url.Substring(url.IndexOf("?") + 1);

                var item = SimpleIoc.Default.GetInstance<IMainModel>().TrackedObjects
                    .FirstOrDefault(x => x.ObjectId == itemUrl);

                if (item == null)
                {
                    MessageBox.Show("Item not found");

                    return;
                }

                DataContext = new ObjectDetailsViewModel(item);
            }

            base.OnNavigatedTo(e);
        }
    }
}