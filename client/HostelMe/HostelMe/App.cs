using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HostelMe
{
    public class App : Application
    {    
        public static Core core
        {
            get
            {
                return Core.Instance;
            }
        }
        public App()
        {
            // The root page of your application
            MainPage = new MainPageHostelMe();
            core.load(this);
            //await model.updateFromServer();
            //MainPage.setModel(ref model);
            //while not do, emulation loading content

        }

        public void setMapPage()
        {
            MainPage = new MapPage();
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
