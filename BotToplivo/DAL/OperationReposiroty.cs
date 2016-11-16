//Реализация интерфейса IRepository<Operation> для объекта класса Operation,
//содержит описание методов для работы с данными таблицы Operations, связанной с объектом Operation

using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace BotToplivo.Models
{
    public class OperationRepository : IRepository<Operation>
    {
        private ToplivoContext db;
        public OperationRepository(ToplivoContext context)
        {
            db = context;
        }

        public void Create(Operation operation)
        {
            db.Operations.Add(operation);
        }

        public void Delete(int id)
        {
            Operation operation=db.Operations.Find(id);
            if (operation != null)
            {
                db.Operations.Remove(operation);
            }            
        }

        public IEnumerable<Operation> Find(Func<Operation, bool> predicate)
        {
            return db.Operations.Include(o => o.Fuel).Include(o => o.Tank).Where(predicate).ToList();
        }

        public Operation Get(int id)
        {
            return db.Operations.Find(id);
        }

        public IEnumerable<Operation> GetAll()
        {
            return db.Operations.Include(o=>o.Fuel).Include(o=>o.Tank).OrderBy(o=>o.OperationID);
        }

        public void Update(Operation operation)
        {
            db.Entry(operation).State=EntityState.Modified;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

     }
}