using System.Windows;
using SharedSquawk.Client.Views.Controls;
using SharedSquawk.Client.WinPhone.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using DataTemplate = System.Windows.DataTemplate;

[assembly: ExportRenderer(typeof(MessageViewCell), typeof(MessageRenderer))]

namespace SharedSquawk.Client.WinPhone.CustomRenderers
{

    public class MessageRenderer : ViewCellRenderer
    {
        public override DataTemplate GetTemplate(Cell cell)
        {
            return Application.Current.Resources["MessageDataTemplate"] as DataTemplate;
        }
    }
}
 