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
    public class CoreWarehouseLocation : BaseCore, IWarehouseLocation
    {
        public CoreWarehouseLocation(MySQLiteContext context)
            : base(context)
        {
        }

        public void Add(Entities.WarehouseLocation warehouseLocation)
        {
            try
            {
                _contextConnection.Add(warehouseLocation);

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
                _contextConnection.WarehouseLocation
                    .Remove(new WarehouseLocation { IdWL = id });

                _contextConnection.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Entities.WarehouseLocation> GetAll()
        {
            IEnumerable<WarehouseLocation> warehouseLocations = new List<WarehouseLocation>();

            try
            {
                warehouseLocations = _contextConnection.WarehouseLocation
                            .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return warehouseLocations;
        }

        public WarehouseLocation GetById(int id)
        {
            WarehouseLocation warehouseLocation = new();

            try
            {
                warehouseLocation = _contextConnection.WarehouseLocation
                    .Where(x => x.IdWL == id)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return warehouseLocation;
        }

        public void Update(Entities.WarehouseLocation warehouseLocation)
        {
            try
            {
                _contextConnection.Update(warehouseLocation);

                _contextConnection.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
