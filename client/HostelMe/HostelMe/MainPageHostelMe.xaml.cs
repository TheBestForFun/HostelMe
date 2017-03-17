using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Json;

using Xamarin.Forms;

namespace HostelMe
{
    public partial class MainPageHostelMe : ContentPage
    {
        private Random rnd = new Random();
        private Model m_model = new Model();

        public MainPageHostelMe()
        {
            InitializeComponent();
        }

        private void OnTestClicked(object sender, EventArgs e)
        {
            Color randomColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            Button button = (Button)sender;
            button.BackgroundColor = randomColor;
            Debug.WriteLine("Color: " + randomColor);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            string answer = await App.RestService.GetDataAsync();
            var model = new Model();
            model.Parse(answer);
            var path = DependencyService.Get<IFileHelper>().GetLocalFilePath(Constants.DBName);
            DB.createTable<Hostel>();
            var res = DB.Insert(model.model.hostels.First());
            var res1 = DB.InsertAll(model.model.hostels);
            //var data = DB.Table<Hostel>();

            m_model.update(model);
        }

        public void setModel(Model model) { m_model = model; }
    }
}
