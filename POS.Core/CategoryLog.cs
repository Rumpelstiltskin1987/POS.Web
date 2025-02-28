using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
                _contextConnection.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }        
        }

        public int GetIdMovement(int IdCategory)
        {
            int IdMovement = 0;

            try
            {
                IdMovement = _contextConnection.CategoryLog
                    .Where(x => x.IdCategory == IdCategory)
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
