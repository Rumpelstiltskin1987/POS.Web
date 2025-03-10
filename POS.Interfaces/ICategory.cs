using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Entities;

namespace POS.Interfaces
{
    public interface ICategory
    {
        void Add(Category category);
        void Delete(int id);
        IEnumerable<Category> GetAll(string status);
        Category GetById(int id);
        void Update(Category category);
    }
}
