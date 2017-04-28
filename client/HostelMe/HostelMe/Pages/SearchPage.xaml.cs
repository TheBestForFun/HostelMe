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
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
            search.TextChanged += (s, e) => FilterNames();
            search.SearchButtonPressed += (s, e) => FilterNames();
            searchedHostels.ItemTapped += (s, e) => Navigation.PushAsync(new InfoPage((e.Item as Hostel).id_hostel));
        }

        private void FilterNames()
        {
            string filter = search.Text.ToUpper();
            if (filter.Length == 0)
                return;            

            searchedHostels.BeginRefresh();
            searchedHostels.ItemsSource = Core.model.m_model.hostels.Where(h => h.h_name.ToUpper().Contains(filter)).ToList();
            searchedHostels.EndRefresh();
        }
    }
}
