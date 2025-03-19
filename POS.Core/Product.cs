using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using POS.Interfaces;
using POS.Entities;
using Microsoft.EntityFrameworkCore;

namespace POS.Core
{
    public class CoreProduct : BaseCore, IProduct
    {
        public CoreProduct(MySQLiteContext context)
            : base(context)
        {
        }

        public void Add(Product product)
        {
            try
            {
                _contextConnection.Add(product);

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
                _contextConnection.Product
                    .Remove(new Product { IdProduct = id });

                _contextConnection.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Product> Find(Expression<Func<Product,bool>> expression)
        {
            //Expression<Func<DataAccess.TransfRequest, bool>> expression = null;
            //expression = x => x.RequestStatus == status;
            //expression = expression.And(x => x.MaterialType == "PE");

            IEnumerable<Product> products = new List<Product>();

            try
            {
                products = _contextConnection.Product.Where(expression).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return products;
        }

        public IEnumerable<Product> GetAll(string status)
        {
            IEnumerable<Product> products = new List<Product>();

            try
            {
                switch (status)
                {
                    case "AC":
                        products = _contextConnection.Product
                            .Where(x => x.Status == "AC")
                            .Include(x => x.Category)
                            .ToList();
                        break;

                    case "IN":
                        products = _contextConnection.Product
                            .Where(x => x.Status == "IN")
                            .Include(x => x.Category)
                            .ToList();
                        break;

                    default:
                        products = _contextConnection.Product
                            .Include(x => x.Category)
                            .ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return products;
        }

        public Product GetById(int id)
        {
            Product product = new Product();

            try
            {
                product = _contextConnection.Product
                    .Where(x => x.IdProduct == id)
                    .Include(x => x.Category)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return product;
        }

        public void Update(Product product)
        {
            try
            {
                _contextConnection.Update(product);

                _contextConnection.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
