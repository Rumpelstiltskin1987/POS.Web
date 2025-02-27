using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Interfaces;
using POS.Core;
using POS.Entities;
using SQLitePCL;

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

        public string Add(Category category)
        {
            CategoryLog log = new();
            string message = string.Empty;

            try
            {
                category.Status = "AC";
                category.CreateUser = "Alta";
                category.CreateDate = DateTime.Now;

                message = _category.Add(category);

                if (message == "Categoría registrada") 
                {
                    log.IdMovement = 1;
                    log.IdCategory = category.IdCategory;
                    log.Name = category.Name;
                    log.Status = category.Status;
                    log.MovementType = "AL";
                    log.LastUpdateUser = category.CreateUser;
                    log.LastUpdateDate = category.CreateDate;

                    _categoryLog.AddLog(log);

                    _contextConnection.SaveChanges();
                }
            }
            finally
            {
                this._category.Dispose();
            }

            return message;
        }

        public string Delete(int id)
        {
            Category category = new();
            try
            {
                GetById(id);
                category.Status = "IN";
                category.LastUpdateUser = "Update";
                category.LastUpdateDate = DateTime.Now;
                return _category.Delete(id);
            }
            finally
            {
                _category.Dispose();
            }
        }

        public IEnumerable<Category> GetAll()
        {
            try
            {
                return _category.GetAll();
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
            finally
            {
                _category.Dispose();
            }
        }

        public Category Find(string name)
        {
            try
            {
                return _category.Find(name);
            }
            finally
            {
                _category.Dispose();
            }
        }

        public void Inactivate(Category category)
        {
            CategoryLog log = new();

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

                _contextConnection.SaveChanges();
            }
            finally
            {
                _category.Dispose();
            }
        }

        public string Update(Category category)
        {
            CategoryLog log = new();
            string message = string.Empty;

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

                _contextConnection.SaveChanges();
            }
            finally
            {
                _category.Dispose();
            }

            return message;
        }
    }
}
