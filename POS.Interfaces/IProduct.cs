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
        string Add(Product product);
        string Delete(int id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAllInactive();
        Product GetById(int id);
        //IEnumerable<Product> Find(Expression<Product> exp);        
        void Inactivate(Product product);
        void Update(Product product);
    }
}
