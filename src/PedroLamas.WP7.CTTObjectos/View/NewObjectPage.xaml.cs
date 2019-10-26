using Microsoft.Phone.Controls;

namespace PedroLamas.WP7.CTTObjectos.View
{
    public partial class NewObjectPage : PhoneApplicationPage
    {
        public NewObjectPage()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Threading.Thread.CurrentThread.CurrentCulture.Name);

            InitializeComponent();
        }

        //private void AddObjectIconButton_Click(object sender, EventArgs e)
        //{
        //    var vm = DataContext as NewObjectViewModel;

        //    if (vm != null)
        //    {
        //        this.Focus();

        //        Dispatcher.BeginInvoke(() =>
        //        {
        //            if (vm.AddObjectCommand.CanExecute(null))
        //                vm.AddObjectCommand.Execute(null);
        //        });
        //    }
        //}
    }
}