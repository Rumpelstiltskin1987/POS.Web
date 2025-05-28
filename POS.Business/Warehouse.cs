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
    public class BusinessWarehouse : IWarehouse
    {
        private readonly MySQLiteContext _contextConnection;
        private readonly CoreWarehouse _warehuse;

        public BusinessWarehouse(MySQLiteContext context)
        {
            _contextConnection = context;
            _warehuse = new CoreWarehouse(_contextConnection);
        }
        public void Add(Entities.Warehouse warehouse)
        {
            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    warehouse.CreateUser = "Alta";

                    _warehuse.Add(warehouse);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    _warehuse.Dispose();
                }
            }
        }

        public void Delete(int id)
        {
            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    _warehuse.Delete(id);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    _warehuse.Dispose();
                }
            }
        }

        public IEnumerable<Entities.Warehouse> GetAll()
        {
            try
            {
                return _warehuse.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _warehuse.Dispose();
            }
        }

        public Entities.Warehouse GetById(int id)
        {
            try
            {
                return _warehuse.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _warehuse.Dispose();
            }
        }

        public void Update(Entities.Warehouse warehouse)
        {
            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    warehouse.LastUpdateUser = "Update";
                    warehouse.LastUpdateDate = DateTime.Now;

                    _warehuse.Update(warehouse);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    _warehuse.Dispose();
                }
            }
        }
    }
}
