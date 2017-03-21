using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;

namespace HostelMe
{
    public class DB
    {
        private static DB instance;
        private DB() { }

        private SQLite.SQLiteConnection connection;

        private static void createTableIfNotExists<T>(ref SQLite.SQLiteConnection aConnection)
        {
            var s = typeof(T).ToString();
            var table = aConnection.GetTableInfo(typeof(T).ToString());
            if (table.Count == 0)
            {
                aConnection.CreateTable<T>();
            }
        }

        private static DB Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DB();
                    instance.connection = new SQLite.SQLiteConnection(DependencyService.Get<IFileHelper>().GetLocalFilePath(Constants.DBName));
                    createTableIfNotExists<Hostel>(ref instance.connection);
                    createTableIfNotExists<Metro>(ref instance.connection);
                    createTableIfNotExists<Phone>(ref instance.connection);
                    createTableIfNotExists<Hostel2phone>(ref instance.connection);
                    createTableIfNotExists<Hostel2metro>(ref instance.connection);
                    createTableIfNotExists<Version>(ref instance.connection);
                }
                return instance;
            }           
        }

    #region DB API
        public static void createTable<T>()
        {
            Instance.connection.CreateTable<T> ();
        }

        public static int InsertAll(IEnumerable objects)
        {
            return Instance.connection.InsertAll(objects);           
        }

        public static int Insert(object obj)
        {           
            return Instance.connection.Insert(obj);
        }

        public static int InsertOrReplace(object obj)
        {
            return Instance.connection.InsertOrReplace(obj);
        }

        public static int InsertOrUpdate(IEnumerable objects)
        {
            int count = 0;
            foreach( var obj in objects)
            {
                bool inserted = true;
                try
                {
                    Instance.connection.Insert(obj);

                }
                catch(Exception ex)
                {
                    inserted = false;
                    Debug.WriteLine(ex.ToString());
                }

                if (inserted)
                {
                    count++;
                    continue;
                }

                try
                {
                    Instance.connection.Update(obj);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    continue;
                }
                count++;
                
            }
            return count;
        }

        public static IList<T> Table<T>() where T : new ()
        {           
            try
            {
                IList<T> list = new List<T>(Instance.connection.Table<T>());
                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return new List<T>();
        }
        #endregion DB API
        #region Utils 
        public static string GetLastDBVersion()
        {
            var version = instance.connection.Table<Version>().OrderByDescending(t => t.date_update).FirstOrDefault(); 
            if (version == null)
                return "";
            return version.db_version;
        }
        #endregion Utlils
    }

}
