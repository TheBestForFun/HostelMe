using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Xamarin.Forms;

namespace HostelMe
{
    public class App : Application
    {
        public static RestApi RestService { get; private set; }
        public static Model model = new Model();

        public App()
        {
            // The root page of your application
            MainPage = new MainPageHostelMe();
            //MainPage.setModel(ref model);
            RestService = new RestApi();
            //while not do, emulation loading content
         /*   Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                MainPage = new MapPage();
                return false;
            });*/

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
