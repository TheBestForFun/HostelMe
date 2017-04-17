using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HostelMe
{
    public partial class MapPage : ContentPage
    {
        Dictionary<Pin, int> pinIdMap = new Dictionary<Pin, int>();
        public MapPage()
        {            
            InitializeComponent();
            var mapPosition = new Position(59.93, 30.31); // center of Petersburg
            HostelMap.MoveToRegion(MapSpan.FromCenterAndRadius(mapPosition, Distance.FromMiles(1)));

            foreach (var hostel in Core.model.m_model.hostels)
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(hostel.h_latitude, hostel.h_longitude),
                    Label = hostel.h_name,
                    Address = hostel.address
                };
                pin.Clicked += onPinClicked;
                HostelMap.Pins.Add(pin);
                pinIdMap.Add(pin, hostel.id_hostel);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();         
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

        }

        private async void onPinClicked(object sender, EventArgs args)
        {
            if (sender is Pin)
            {
                var pin = (Pin)sender;
                InfoPage info = new InfoPage(pinIdMap[pin]);
                await Navigation.PushAsync(info);
                
              /*  hostelName.Text = pin.Label;
                hostelAddress.Text = pin.Address;*/

            }
        }

        async void onHostelInfoClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new HostelInfoPage());
        }
    }
}
