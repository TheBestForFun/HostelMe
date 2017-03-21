using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using SQLite;

namespace HostelMe
{
    //http://jsonutils.com/

    public class Hostel
    {       
        public string address { get; set; }
        public string h_date_add { get; set; }
        public string h_date_update { get; set; }
        [PrimaryKey]
        public int id_hostel { get; set; }
        public double h_latitude { get; set; }
        public double h_longitude { get; set; }
        public string h_name { get; set; }
        public string site { get; set; }
    }

    public class Phone
    {
        [PrimaryKey]
        public int id_phone { get; set; }
        public string phone { get; set; }
    }

    public class Metro
    {
        [PrimaryKey]
        public int id_metro { get; set; }
        public double m_latitude { get; set; }
        public double m_longitude { get; set; }
        public string m_name { get; set; }
    }

    public class Hostel2metro
    {       
        public int id_hostel { get; set; }
        [PrimaryKey]
        public int id_hostel2metro { get; set; }
        public int id_metro { get; set; }
    }

    public class Hostel2phone
    {
        public int id_hostel { get; set; }
        [PrimaryKey]
        public int id_hostel2phone { get; set; }
        public int id_phone { get; set; }
    }

    public class Version
    {
        [PrimaryKey]
        public int id { get;  set;}
        public string db_version { get; set; }
        public string date_update { get; set; }
    }

    public class Tables
    {              
        public IList<Hostel> hostels { get; set; }
        public IList<Phone> phones { get; set; }
        public IList<Metro> metros { get; set; }
        public IList<Hostel2metro> hostel2metros { get; set; }
        public IList<Hostel2phone> hostel2phones { get; set; }
        public IList<Version> versions { get; set; }
    }    

    public class Model
    {
        public Tables m_model = new Tables();                

        public void Parse(string data)
        {
            try
            {
                m_model = JsonConvert.DeserializeObject<Tables>(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Answer: " + ex.ToString());
                return;
            }
        }

        public void init()
        {
            m_model.hostels = DB.Table<Hostel>();
            m_model.phones = DB.Table<Phone>();
            m_model.metros = DB.Table<Metro>();
            m_model.hostel2metros = DB.Table<Hostel2metro>();
            m_model.hostel2phones = DB.Table<Hostel2phone>();
            m_model.versions = DB.Table<Version>();
        }

        public void update(string aAnswer)
        {
            if (aAnswer == null || aAnswer.Length == 0)
                return;

            var model = new Model();
            model.Parse(aAnswer);
            if (model == null)
                return;

            DB.InsertOrUpdate(model.m_model.hostels);
            DB.InsertOrUpdate(model.m_model.metros);
            DB.InsertOrUpdate(model.m_model.phones);
            DB.InsertOrUpdate(model.m_model.hostel2metros);
            DB.InsertOrUpdate(model.m_model.hostel2phones);
            DB.InsertOrUpdate(model.m_model.versions);
            init();
        }
    }
}
