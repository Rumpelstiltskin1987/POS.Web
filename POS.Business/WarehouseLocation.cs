using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Core;
using POS.Entities;
using POS.Interfaces;

namespace POS.Business
{
    public class BusinessWarehouseLocation : IWarehouseLocation
    {
        private readonly MySQLiteContext _contextConnection;
        private readonly CoreWarehouseLocation _warehuseLocation;

        public BusinessWarehouseLocation(MySQLiteContext context)
        {
            _contextConnection = context;
            _warehuseLocation = new CoreWarehouseLocation(_contextConnection);
        }

        public void Add(Entities.WarehouseLocation warehouseLocation)
        {
            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    warehouseLocation.CreateUser = "Alta";

                    _warehuseLocation.Add(warehouseLocation);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    _warehuseLocation.Dispose();
                }
            }
        }

        public void Delete(int id)
        {
            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    _warehuseLocation.Delete(id);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    _warehuseLocation.Dispose();
                }
            }
        }

        public IEnumerable<Entities.WarehouseLocation> GetAll()
        {
            try
            {
                return _warehuseLocation.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _warehuseLocation.Dispose();
            }
        }

        public Entities.WarehouseLocation GetById(int id)
        {
            try
            {
                return _warehuseLocation.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _warehuseLocation.Dispose();
            }
        }

        public void Update(Entities.WarehouseLocation warehouseLocation)
        {
            using (var transaction = _contextConnection.Database.BeginTransaction())
            {
                try
                {
                    warehouseLocation.LastUpdateUser = "Update";
                    warehouseLocation.LastUpdateDate = DateTime.Now;

                    _warehuseLocation.Update(warehouseLocation);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    _warehuseLocation.Dispose();
                }
            }
        }
    }
}
