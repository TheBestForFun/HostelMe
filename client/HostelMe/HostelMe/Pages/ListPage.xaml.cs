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
    public class ItemHostel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Distance { get; set; }
        public double rawDist { set; get; }
        public int Id { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        public List<ItemHostel> DistHostels { get; set; }
        public ListPage()
        {            
            DistHostels = findNearestHostel(MapPage.center);
            InitializeComponent();
            this.BindingContext = this;
        }

        private List<ItemHostel> findNearestHostel(Position pos)
        {
            List<ItemHostel> result = new List<ItemHostel>();
            foreach (var hostel in Core.model.m_model.hostels)
            {
                var distHostel = new ItemHostel
                {
                    Name = hostel.h_name,
                    Address = hostel.address, 
                    Id = hostel.id_hostel                                    
                };
                getDistance(pos, hostel.h_latitude, hostel.h_longitude, ref distHostel);
                result.Add(distHostel);
            }
            return result.OrderBy(h => h.rawDist).ToList();
            
        }

        private void getDistance(Position pos, double lat, double lon, ref ItemHostel distHost)
        {
            distHost.rawDist = distance(pos.Latitude, pos.Longitude, lat, lon, 'K');
            if (distHost.rawDist < 1)
            {
                distHost.Distance = (distHost.rawDist * 1000).ToString("F0") + " m";
            }
            else
            {
                distHost.Distance = distHost.rawDist.ToString("F") + " km";
            }
        }

        private async void hostelItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new InfoPage((e.Item as ItemHostel).Id));
        }
    
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //:::                                                                         :::
    //:::  This routine calculates the distance between two points (given the     :::
    //:::  latitude/longitude of those points). It is being used to calculate     :::
    //:::  the distance between two locations using GeoDataSource(TM) products    :::
    //:::                                                                         :::
    //:::  Definitions:                                                           :::
    //:::    South latitudes are negative, east longitudes are positive           :::
    //:::                                                                         :::
    //:::  Passed to function:                                                    :::
    //:::    lat1, lon1 = Latitude and Longitude of point 1 (in decimal degrees)  :::
    //:::    lat2, lon2 = Latitude and Longitude of point 2 (in decimal degrees)  :::
    //:::    unit = the unit you desire for results                               :::
    //:::           where: 'M' is statute miles (default)                         :::
    //:::                  'K' is kilometers                                      :::
    //:::                  'N' is nautical miles                                  :::
    //:::                                                                         :::
    //:::  Worldwide cities and other features databases with latitude longitude  :::
    //:::  are available at http://www.geodatasource.com                          :::
    //:::                                                                         :::
    //:::  For enquiries, please contact sales@geodatasource.com                  :::
    //:::                                                                         :::
    //:::  Official Web site: http://www.geodatasource.com                        :::
    //:::                                                                         :::
    //:::           GeoDataSource.com (C) All Rights Reserved 2015                :::
    //:::                                                                         :::
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

    private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
    {
        double theta = lon1 - lon2;
        double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
        dist = Math.Acos(dist);
        dist = rad2deg(dist);
        dist = dist * 60 * 1.1515;
        if (unit == 'K')
        {
            dist = dist * 1.609344;
        }
        else if (unit == 'N')
        {
            dist = dist * 0.8684;
        }
        return (dist);
    }

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //::  This function converts decimal degrees to radians             :::
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    private double deg2rad(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //::  This function converts radians to decimal degrees             :::
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    private double rad2deg(double rad)
    {
        return (rad / Math.PI * 180.0);
    }
}
}
