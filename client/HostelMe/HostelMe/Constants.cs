﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HostelMe
{
    public static class Constants
    {
        // URL of REST service
        public static string RestUrl = "http://192.168.230.2:8000/system/db";
        public static string DBName = "hostel.db";

        //rest api
        public static string Version = "ver";
        public static string Udid = "udid";
        public static string HostelID = "id_hostel";
        public static string Act = "action";

        public static int RequestDelay = 1000;
    }
}
