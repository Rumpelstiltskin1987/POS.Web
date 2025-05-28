using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Entities;

namespace POS.Interfaces
{
    public interface IWarehouseLocation
    {
        void Add(WarehouseLocation warehouseLocation);
        void Delete(int id);
        IEnumerable<WarehouseLocation> GetAll();
        WarehouseLocation GetById(int id);
        void Update(WarehouseLocation warehouseLocation);
    }
}
