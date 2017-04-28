using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HostelMe
{
    public class MenuItem
    {
        public string Title{get;set;}
        public Type TargetType { get; set; }
    }    

    public partial class MenuPage : ContentPage
    {
        public ListView ListView { get { return listView; } }        
        public MenuPage()
        {
            InitializeComponent();
            listView.ItemsSource = new List<MenuItem>
            {
                new MenuItem{ Title = "Map", TargetType = typeof(MapPage)},
                new MenuItem{ Title = "List", TargetType = typeof(ListPage)},
                new MenuItem{ Title = "Metro", TargetType = typeof(MetroPage)},
                new MenuItem{ Title = "Search", TargetType = typeof(SearchPage)},
            };            
        }  
    }    
}
