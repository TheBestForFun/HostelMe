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
        [PrimaryKey, AutoIncrement]
        public int id_hostel { get; set; }
        public double h_latitude { get; set; }
        public double h_longitude { get; set; }
        public string h_name { get; set; }
        public string site { get; set; }
    }

    public class Phone
    {
        [PrimaryKey, AutoIncrement]
        public int id_phone { get; set; }
        public string phone { get; set; }
    }

    public class Metro
    {
        [PrimaryKey, AutoIncrement]
        public int id_metro { get; set; }
        public double m_latitude { get; set; }
        public double m_longitude { get; set; }
        public string m_name { get; set; }
    }

    public class Hostel2metro
    {       
        public int id_hostel { get; set; }
        [PrimaryKey, AutoIncrement]
        public int id_hostel2metro { get; set; }
        public int id_metro { get; set; }
    }

    public class Hostel2phone
    {
        public int id_hostel { get; set; }
        [PrimaryKey, AutoIncrement]
        public int id_hostel2phone { get; set; }
        public int id_phone { get; set; }
    }

    public class Tables
    {
        public IList<Hostel> hostels { get; set; }
        public IList<Phone> phones { get; set; }
        public IList<Metro> metros { get; set; }
        public IList<Hostel2metro> hostel2metros { get; set; }
        public IList<Hostel2phone> hostel2phones { get; set; }
    }

    public class Model
    {
        public Tables model { get; set; }

        public void Parse(string data)
        {
            try
            {
                model = JsonConvert.DeserializeObject<Tables>(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Answer: " + ex.ToString());
                return;
            }
        }

        public void update(Model aModel)
        {
            //update DB
            //load new model
            //model.hostel.hostelItems.Add(aModel.model.hostel.hostelItems)
        }
    }
}
