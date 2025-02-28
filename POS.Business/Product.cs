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

        public string Add(Product product)
        {
            ProductLog log = new();
            string menssage = string.Empty;

            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    menssage = _product.Add(product);

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
                    log.LastUpdateDate = product.CreateDate;

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

            return menssage;    
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

        public IEnumerable<Product> GetAllInactive()
        {
            try
            {
                return _product.GetAllInactive();
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

        public void Inactivate(Product product)
        {
            ProductLog log = new();

            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    product.Status = "IN";
                    product.LastUpdateUser = "Update";
                    product.LastUpdateDate = DateTime.Now;

                    _product.Inactivate(product);

                    log.IdMovement = _productLog.GetIdMovement(product.IdCategory);
                    log.IdCategory = product.IdCategory;
                    log.Name = product.Name;
                    log.Status = product.Status;
                    log.MovementType = "ED";
                    log.LastUpdateUser = "Editar";
                    log.LastUpdateDate = product.LastUpdateDate;

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

        public void Update(Product product)
        {
            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    _product.Update(product);
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
    }
}
