using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace HostelMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage(int id)
        {
            InitializeComponent();
            Hostel hostel = Core.model.m_model.GetHostel(id);
            Name.Text = hostel.h_name;
            Address.Text = hostel.address;
            Phone.Text = Core.model.m_model.GetPhoneByHostelId(id);
            Site.Text = hostel.site;

            var mapPosition = new Position(hostel.h_latitude, hostel.h_longitude); // center of Petersburg

            InfoMap.MoveToRegion(MapSpan.FromCenterAndRadius(mapPosition, Distance.FromMiles(1)));
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = mapPosition,
                Label = hostel.h_name,
                Address = hostel.address
            };
            InfoMap.Pins.Add(pin);
        }
    }
}
