using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using System.Threading;

namespace HostelMe
{
    public sealed class DB
    {
        private SQLite.SQLiteConnection connection;

        private static DB m_db = null;
        private static readonly object s_lock = new object();
        public static DB db
        {
            get
            {
                if (m_db != null) return m_db;

                Monitor.Enter(s_lock);
                DB tmp = new DB();
                Interlocked.Exchange(ref m_db, tmp);
                SQLite.SQLiteConnection con = new SQLite.SQLiteConnection(DependencyService.Get<IFileHelper>().GetLocalFilePath(Constants.DBName));
                createTableIfNotExists<Hostel>(ref con);
                createTableIfNotExists<Metro>(ref con);
                createTableIfNotExists<Phone>(ref con);
                createTableIfNotExists<Hostel2phone>(ref con);
                createTableIfNotExists<Hostel2metro>(ref con);
                createTableIfNotExists<Version>(ref con);

                Interlocked.Exchange(ref m_db.connection, con);
                Monitor.Exit(s_lock);
                return m_db;
            }           
        }

        private static void createTableIfNotExists<T>(ref SQLite.SQLiteConnection aConnection)
        {
            var s = typeof(T).ToString();
            var table = aConnection.GetTableInfo(typeof(T).ToString());
            if (table.Count == 0)
            {
                aConnection.CreateTable<T>();
            }
        }

        #region DB API
        public static void createTable<T>()
        {
            db.connection.CreateTable<T> ();
        }

        public static int InsertAll(IEnumerable objects)
        {
            return db.connection.InsertAll(objects);           
        }

        public static int Insert(object obj)
        {           
            return db.connection.Insert(obj);
        }

        public static int InsertOrReplace(object obj)
        {
            return db.connection.InsertOrReplace(obj);
        }

        public static int InsertOrUpdate(IEnumerable objects)
        {
            int count = 0;
            foreach( var obj in objects)
            {
                bool inserted = true;
                try
                {
                    db.connection.Insert(obj);

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
                    db.connection.Update(obj);
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
                IList<T> list = new List<T>(db.connection.Table<T>());
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
            var version = db?.connection?.Table<Version>()?.OrderByDescending(t => t.date_update).FirstOrDefault(); 
            if (version == null)
                return "";
            return version.db_version;
        }
        #endregion Utlils
    }

}
