using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;
using Microsoft.EntityFrameworkCore;
using POS.Interfaces;
using POS.Entities;
using System.Linq.Expressions;

namespace POS.Core
{
    public class CoreCategory : BaseCore, ICategory
    {
        public CoreCategory(MySQLiteContext context)
            : base(context)
        {
        }

        public void Add(Category category)
        {
            try
            {
                _contextConnection.Add(category);

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
                _contextConnection.Category
                    .Remove(new Category { IdCategory = id });

                _contextConnection.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Category> GetAll(string status)
        {
            IEnumerable<Category> categories = new List<Category>();

            try
            {
                switch (status)
                {
                    case "AC":
                        categories = _contextConnection.Category
                            .Where(x => x.Status == "AC")
                            .ToList();
                        break;

                    case "IN":
                        categories = _contextConnection.Category
                            .Where(x => x.Status == "IN")
                            .ToList();
                        break;

                    default:
                        categories = _contextConnection.Category
                            .ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return categories;
        }
        public Category GetById(int id)
        {
            Category category = new Category();

            try
            {
                category = _contextConnection.Category
                    .Where(x => x.IdCategory == id)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return category;
        }

        public int GetIdCategory()
        {
            int idCategory = 0;

            try
            {
                idCategory = _contextConnection.Database
                    .SqlQueryRaw<int>("SELECT seq FROM sqlite_sequence WHERE name = 'Category'")
                    .AsEnumerable()
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return idCategory;
        }

        public void Update(Category category)
        {
            try
            {
                _contextConnection.Update(category);

                _contextConnection.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
