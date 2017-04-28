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

        public static Position center = new Position(59.93, 30.31); // center of Petersburg      

        public MapPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

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
                pinIdMap.Add(pin, hostel.id_hostel);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            center = getMapCenter();
            var map = layoutMap.Children.First() as Map;
            layoutMap.Children.Remove(map);            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            addMap();
        }

        private void addMap()
        {   // dynamic add map and remove to show in other pages
            // now its not work correctly
            var map = new Map(MapSpan.FromCenterAndRadius(center, Distance.FromMiles(1)));

            foreach(Pin pin in pinIdMap.Keys)
            {
                map.Pins.Add(pin);
            }

            map.IsShowingUser = true;
            map.MapType = MapType.Street;
            map.HorizontalOptions = LayoutOptions.FillAndExpand;
            map.VerticalOptions = LayoutOptions.FillAndExpand;

            layoutMap.Children.Add(map);
        }

        private Position getMapCenter()
        {
            var map = layoutMap.Children.First() as Map;
            return map.VisibleRegion.Center;
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
