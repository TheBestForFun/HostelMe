using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HostelMe
{
    public class DB
    {
        private static DB instance;
        private DB() { }

        private SQLite.SQLiteConnection connection;

        private static DB Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DB();
                    instance.connection = new SQLite.SQLiteConnection(DependencyService.Get<IFileHelper>().GetLocalFilePath(Constants.DBName));
                }
                return instance;
            }           
        }

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

        /* public static TableQuery<T> Table<T>()
         {
             return getInstance().connection.Table<T>();
         }*/
    }
}
