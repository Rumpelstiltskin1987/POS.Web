using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Entities;

namespace POS.Interfaces
{
    public interface IWarehouse
    {
        void Add(Warehouse warehouse);
        void Delete(int id);
        IEnumerable<Warehouse> GetAll();
        Warehouse GetById(int id);
        void Update(Warehouse warehouse);
    }
}
