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
            m_model.init();

        }

        private async void OnTestClicked(object sender, EventArgs e)
        {
            Color randomColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            Button button = (Button)sender;
            button.BackgroundColor = randomColor;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();                       
        }

        public void setModel(Model model) { m_model = model; }
    }
}
