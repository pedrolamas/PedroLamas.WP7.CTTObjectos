using Microsoft.Phone.Controls;

namespace PedroLamas.WP7.CTTObjectos.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Threading.Thread.CurrentThread.CurrentCulture.Name);

            InitializeComponent();
        }
    }
}