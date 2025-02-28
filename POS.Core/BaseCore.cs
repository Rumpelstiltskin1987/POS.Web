using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using POS.Entities;

namespace POS.Core
{
    public class BaseCore : System.IDisposable
    {
        public MySQLiteContext _contextConnection;
       
        public BaseCore(MySQLiteContext context)
        {
            _contextConnection = context;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            if (this._contextConnection != null)
            {
                this._contextConnection.Dispose(); 
            }
        }

        ~BaseCore()
        {
            this.Dispose(false);
        }
    }
}
