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
            //string data = @"{""hostels"":[{""address"":""Марата ул., д.25"",""h_date_add"":""2017 - 01 - 18 11:31:46"",""h_date_update"":""2017 - 03 - 28 08:31:46"",""id_hostel"":1,""h_latitude"":59.927391,""h_longitude"":30.352618,""h_name"":""Жить хорошо"",""site"":""http://vk.com/otelhorosho\n""},{""address"":""Большая Конюшенная ул., д.13Н"",""h_date_add"":""2017-01-18 18:06:33"",""h_date_update"":""2017-03-28 08:00:00"",""id_hostel"":2,""h_latitude"":59.939135,""h_longitude"":30.322498,""h_name"":""Stay Simple\r\n"",""site"":""http://vk.com/staysimple\n""}],""phones"":[{""id_phone"":1,""phone"":""+7 (812) 456-44-34\r\n\r\n""},{""id_phone"":2,""phone"":""+7 (950) 224-40-00\r\n""},{""id_phone"":3,""phone"":""+7 (812) 989-71-99 ""}],""metros"":[{""id_metro"":1,""m_latitude"":59.92746375,""m_longitude"":30.34817149,""m_name"":""Владимирская""},{""id_metro"":2,""m_latitude"":59.93557875,""m_longitude"":30.32702549,""m_name"":""Невский проспект""}],""hostel2metros"":[{""id_hostel"":1,""id_hostel2metro"":1,""id_metro"":1},{""id_hostel"":2,""id_hostel2metro"":2,""id_metro"":2}],""hostel2phones"":[{""id_hostel"":1,""id_hostel2phone"":1,""id_phone"":1},{""id_hostel"":1,""id_hostel2phone"":2,""id_phone"":2},{""id_hostel"":2,""id_hostel2phone"":3,""id_phone"":3}],""versions"":[{""id"":1,""db_version"":""0.1"",""date_update"":""2017-01-18 08:31:47""},{""id"":2,""db_version"":""0.2"",""date_update"":""2017-01-18 15:06:33""},{""id"":3,""db_version"":""0.3"",""date_update"":""2017-03-29 15:06:33""}]}";
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
