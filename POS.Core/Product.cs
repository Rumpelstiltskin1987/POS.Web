using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using POS.Interfaces;
using POS.Entities;

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
            finally
            {
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
            finally
            {
            }

            return message;
        }

        public IEnumerable<Product> Find(Expression<Product> exp)
        {
            IEnumerable<Product> products = new List<Product>();
            try
            {
                //products = _contextConnection.Product.Find(exp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return products;
        }

        public IEnumerable<Product> GetAll()
        {
            IEnumerable<Product> products = new List<Product>();

            try
            {
                products = _contextConnection.Product
                    .Where(x => x.Status == "AC").ToList();//AsEnumerable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
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
            finally
            {
            }

            return product;
        }

        public string Update(Product product)
        {
            string message = string.Empty;

            try
            {
                product.LastUpdateUser = "Update";
                product.LastUpdateDate = DateTime.Now;

                _contextConnection.Update(product);
                _contextConnection.SaveChanges();
                message = "Producto actualizado";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return message;
        }
    }
}
