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
        private readonly CoreProductLog _productLog;
        public BusinessProduct(MySQLiteContext context) 
        { 
            _contextConnection = context;
            _product = new CoreProduct(_contextConnection);
            _productLog = new CoreProductLog(_contextConnection);
        }

        public void Add(Product product)
        {
            ProductLog log = new();
            string menssage = string.Empty;

            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    product.Status = "AC";
                    product.CreateUser = "Alta";

                    _product.Add(product);

                    log.IdMovement = 1;
                    log.IdProduct = product.IdProduct;
                    log.Name = product.Name;
                    log.Description = product.Description;
                    log.IdCategory = product.IdCategory;
                    log.Price = product.Price;
                    log.MeasureUnit = product.MeasureUnit;
                    log.UrlImage = product.UrlImage;
                    log.Status = product.Status;
                    log.MovementType = "AL";
                    log.LastUpdateUser = product.CreateUser;

                    _productLog.AddLog(log);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {                    
                    _product.Dispose();
                    _productLog.Dispose();
                }
            }  
        }

        public void Delete(int id)
        {
            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    _product.Delete(id);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    _product.Dispose();
                }
            } 
        }

        public IEnumerable<Product> GetAll(string status)
        {
            try
            {
                return _product.GetAll(status);
            }
            catch (Exception ex)
            {
                throw ex;
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _product.Dispose();
            }
        }

        public void Update(Product product)
        {
            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                ProductLog log = new();

                try
                {
                    product.LastUpdateUser = "Update";
                    product.LastUpdateDate = DateTime.Now;

                    _product.Update(product);

                    log.IdMovement = _productLog.GetIdMovement(product.IdProduct) + 1;
                    log.IdProduct = product.IdProduct;
                    log.Name = product.Name;
                    log.Description = product.Description;
                    log.IdCategory = product.IdCategory;
                    log.Price = product.Price;
                    log.MeasureUnit = product.MeasureUnit;
                    log.UrlImage = product.UrlImage;
                    log.Status = product.Status;
                    log.MovementType = "ED";
                    log.LastUpdateUser = product.CreateUser;

                    _productLog.AddLog(log);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    _product.Dispose();
                    _productLog.Dispose();
                }
            }
        }
    }
}
