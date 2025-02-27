using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Entities;

using POS.Interfaces;

namespace POS.Core
{
    public class CoreCategoryLog : BaseCore
    {
        public CoreCategoryLog(MySQLiteContext context)
            : base(context)
        {
        }
        public void AddLog(CategoryLog log)
        {
            try
            {
                _contextConnection.Add(log);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetIdMovement(int IdCategory)
        {
            int movementID = 0;

            try
            {
                movementID = _contextConnection.CategoryLog
                    .Where(x => x.IdCategory == IdCategory)
                    .Max(x => (int?)x.IdMovement) ?? 0;
            }
            catch (Exception ex)
            {
                throw ex;   
            }

            return movementID;
        }
    }
}
