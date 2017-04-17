using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using HostelMe.Droid;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationRenderer))]  
namespace HostelMe.Droid
{
    public class CustomNavigationRenderer : NavigationRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);
            RemoveAppIconFromActionBar();
        }

        void RemoveAppIconFromActionBar()
        {
            var actionBar = ((Activity)Context).ActionBar;
            actionBar.SetIcon(new ColorDrawable(Color.Transparent.ToAndroid()));
        }

    }
}