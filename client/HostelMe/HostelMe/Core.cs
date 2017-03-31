using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HostelMe
{
    public sealed class Core
    {
        private static readonly Lazy<Core> instance = new Lazy<Core>(() => new Core());
        private static readonly object ThreadLock = new object();
        public static Core Instance {
            get
            {               
                if (instance == null)
                {
                    lock (ThreadLock)
                    {
                        return instance.Value;
                    }
                }
                else
                {
                    return instance.Value;
                }
                
            }
        }
        private Core() { }

        private static readonly Lazy<Model> m_model = new Lazy<Model>(() => new Model());
        public static Model model {
            get
            {
                if (instance != null)
                {                   
                    return m_model.Value;
                }
                return null;                
            }
        }

        private static readonly Lazy<RestApi> m_restApi = new Lazy<RestApi>(() => new RestApi());
        public static RestApi restApi
        {
            get
            {
                if (instance != null)
                {
                    return m_restApi.Value;
                }
                return null;
            }
        }       
    
        static void init()
        {
            model.init();
        }

        public async void load(App app)
        {
            DateTime before = DateTime.Now;
            await Task.Run(() => init());            
            string data = await restApi?.GetDataAsync();
            await Task.Run(() => model?.update(data));
            DateTime after = DateTime.Now;
            var diff = Constants.RequestDelay - (after - before).Milliseconds;
            if (diff > 0)
                await Task.Delay(diff);
            Log.log.WriteLine("all load");
            app.setMapPage();
        }
    }
}
