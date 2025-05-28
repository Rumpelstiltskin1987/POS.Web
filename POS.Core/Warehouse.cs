using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Interfaces;
using POS.Entities;
using Microsoft.EntityFrameworkCore;

namespace POS.Core
{
    public class CoreWarehouse : BaseCore ,IWarehouse
    {
        public CoreWarehouse(MySQLiteContext context)
            : base(context)
        {
        }

        public void Add(Warehouse warehouse)
        {
            try
            {
                _contextConnection.Add(warehouse);

                _contextConnection.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _contextConnection.Warehouse
                    .Remove(new Warehouse { IdWarehouse = id });

                _contextConnection.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Warehouse> GetAll()
        {
            IEnumerable<Warehouse> warehouses = new List<Warehouse>();

            try
            {
                warehouses = _contextConnection.Warehouse
                            .Include(x => x.WarehouseLocation)
                            .ToList();                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return warehouses;
        }

        public Warehouse GetById(int id)
        {
            Warehouse warehouse = new Warehouse();

            try
            {
                warehouse = _contextConnection.Warehouse
                    .Where(x => x.IdWarehouse == id)
                    .Include(x => x.WarehouseLocation)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return warehouse;
        }

        public void Update(Warehouse warehouse)
        {
            try
            {
                _contextConnection.Update(warehouse);

                _contextConnection.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
