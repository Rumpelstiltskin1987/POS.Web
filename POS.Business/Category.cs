using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Interfaces;
using POS.Core;
using POS.Entities;
using SQLitePCL;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace POS.Business
{
    public class BusinessCategory : ICategory
    {
        private readonly MySQLiteContext _contextConnection;
        private readonly CoreCategory _category;
        private readonly CoreCategoryLog _categoryLog;

        public BusinessCategory(MySQLiteContext context)
        {
            _contextConnection = context;
            _category = new CoreCategory(_contextConnection);
            _categoryLog = new CoreCategoryLog(_contextConnection);
        }

        public void Add(Category category)
        {
            CategoryLog log = new();

            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    category.Status = "AC";
                    category.CreateUser = "Alta";
                    category.CreateDate = DateTime.Now;

                    _category.Add(category);

                    log.IdMovement = 1;
                    log.IdCategory = _category.GetIdCategory();
                    log.Name = category.Name;
                    log.Status = category.Status;
                    log.MovementType = "AL";
                    log.LastUpdateUser = category.CreateUser;
                    log.LastUpdateDate = category.CreateDate;

                    _categoryLog.AddLog(log);

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    this._category.Dispose();
                }
            }
        }

        public void Delete(int id)
        {
            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    _category.Delete(id);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    _category.Dispose();
                }
            }                
        }

        public IEnumerable<Category> GetAll(string status)
        {
            try
            {
                return _category.GetAll(status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _category.Dispose();
            }
        }

        public Category GetById(int id)
        {
            try
            {
                return _category.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _category.Dispose();
            }
        }

        public void Inactivate(Category category)
        {
            CategoryLog log = new();

            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    category.Status = "IN";
                    category.LastUpdateUser = "Update";
                    category.LastUpdateDate = DateTime.Now;

                    _category.Inactivate(category);

                    log.IdMovement = _categoryLog.GetIdMovement(category.IdCategory) + 1;
                    log.IdCategory = category.IdCategory;
                    log.Name = category.Name;
                    log.Status = category.Status;
                    log.MovementType = "ED";
                    log.LastUpdateUser = "Editar";
                    log.LastUpdateDate = category.LastUpdateDate;

                    _categoryLog.AddLog(log);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    _category.Dispose();
                    _categoryLog.Dispose();
                }
            }
        }

        public void Update(Category category)
        {
            CategoryLog log = new();

            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    category.LastUpdateUser = "Update";
                    category.LastUpdateDate = DateTime.Now;

                    _category.Update(category);

                    log.IdMovement = _categoryLog.GetIdMovement(category.IdCategory) + 1;
                    log.IdCategory = category.IdCategory;
                    log.Name = category.Name;
                    log.Status = category.Status;
                    log.MovementType = "ED";
                    log.LastUpdateUser = "Editar";
                    log.LastUpdateDate = category.LastUpdateDate;

                    _categoryLog.AddLog(log);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    _category.Dispose();
                }
            }
        }
    }
}
