using POS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class CoreProductLog : BaseCore
    {
        public CoreProductLog(MySQLiteContext context)
            : base(context)
        {
        }

        public void AddLog(ProductLog log)
        {
            try
            {
                _contextConnection.Add(log);
                _contextConnection.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetIdMovement(int IdProduct)
        {
            int IdMovement = 0;

            try
            {
                IdMovement = _contextConnection.ProductLog
                    .Where(x => x.IdProduct == IdProduct)
                    .Max(x => (int?)x.IdMovement) ?? 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IdMovement;
        }
    }
}
