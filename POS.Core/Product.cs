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

        public string Add(Product product)
        {
            string message = string.Empty;

            try
            {
                product.Status = "AC";
                product.CreateUser = "Alta";
                product.CreateDate = DateTime.Now;

                _contextConnection.Add(product);
                _contextConnection.SaveChanges();

                message = "Producto registrado";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return message;
        }

        public string Delete(int id)
        {
            Product product = new Product();

            string message = string.Empty; try
            {
                product = GetById(id);

                product.Status = "IN";
                product.LastUpdateUser = "Update";
                product.LastUpdateDate = DateTime.Now;

                _contextConnection.Update(product);
                _contextConnection.SaveChanges();

                message = "Producto eliminado";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return message;
        }

        public IEnumerable<Product> GetAll()
        {
            IEnumerable<Product> products = new List<Product>();

            try
            {
                products = _contextConnection.Product
                    .Where(x => x.Status == "AC")
                    .Include(x => x.Category) 
                    .ToList();//AsEnumerable();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return products;

        }

        public IEnumerable<Product> GetAllInactive()
        {
            IEnumerable<Product> products = new List<Product>();

            try
            {
                products = _contextConnection.Product
                    .Where(x => x.Status == "IN")
                    .Include(x => x.Category)
                    .ToList();
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
                    .FirstOrDefault();
            }
            catch (Exception ex) 
            {
                throw ex;
            }

            return product;
        }

        public void Inactivate(Product product)
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

        public void Update(Product product)
        {
            try
            {
                product.LastUpdateUser = "Update";
                product.LastUpdateDate = DateTime.Now;

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
