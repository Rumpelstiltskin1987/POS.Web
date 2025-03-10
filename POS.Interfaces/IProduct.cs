using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using POS.Entities;

namespace POS.Interfaces
{
    public interface IProduct
    {
        void Add(Product product);
        void Delete(int id);
        IEnumerable<Product> GetAll(string status);
        Product GetById(int id);       
        void Update(Product product);
    }
}
