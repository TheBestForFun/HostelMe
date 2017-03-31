using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HostelMe
{
    public sealed class Log
    {
        private static Log m_log = null;
        private static readonly object s_lock = new object();
        public static Log log
        {
            get
            {
                if (m_log == null)
                {
                    Monitor.Enter(s_lock);
                    Log tmp = new Log();
                    Interlocked.Exchange(ref m_log, tmp);
                    Monitor.Exit(s_lock);
                }
                return m_log;
            }
        }
        public void WriteLine(object obj)
        {
            Debug.WriteLine("[HM] " + obj);
        }
    }
}
