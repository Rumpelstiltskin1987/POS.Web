using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Interfaces;
using POS.Core;
using POS.Entities;

namespace POS.Business
{
    public class BusinessProduct : IProduct
    {
        private readonly MySQLiteContext _contextConnection;
        private readonly CoreProduct _product;
        public BusinessProduct(MySQLiteContext context) 
        { 
            _contextConnection = context;
            _product = new CoreProduct(_contextConnection);    
        }

        public string Add(Product product)
        {
            try
            {
                return _product.Add(product);  
            }
            finally
            {
                _product.Dispose();
            }
        }

        public string Delete(int id)
        {
            try
            {
                return _product.Delete(id);
            }
            finally
            {
                _product.Dispose();
            }
        }

        //public IEnumerable<Product> Find(Expression<Product> exp)
        //{
        //    try
        //    {
        //        return _product.Find(exp);
        //    }
        //    finally
        //    {
        //        _product.Dispose();
        //    }
        //}

        public IEnumerable<Product> GetAll()
        {
            try
            {
                return _product.GetAll();
            }
            finally
            {
                _product.Dispose();
            }
        }

        public Product GetById(int id)
        {
            try
            {
                return _product.GetById(id);
            }
            finally
            {
                _product.Dispose();
            }
        }

        public string Update(Product product)
        {
            try
            {
                return _product.Update(product);
            }
            finally
            {
                _product.Dispose();
            }
        }
    }
}
