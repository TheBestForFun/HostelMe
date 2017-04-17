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
    public partial class LoadPage : ContentPage
    {        
        private Model m_model = new Model();

        public LoadPage()
        {            
            InitializeComponent();
            m_model.init();
        }

        public void setModel(Model model) { m_model = model; }
    }
}
