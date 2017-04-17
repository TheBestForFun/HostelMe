using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HostelMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage(int id)
        {
            InitializeComponent();
            Name.Text = Core.model.m_model.GetHostel(id).h_name;
            Address.Text = Core.model.m_model.GetHostel(id).address;
            Phone.Text = Core.model.m_model.GetPhoneByHostelId(id);            
        }
    }
}
