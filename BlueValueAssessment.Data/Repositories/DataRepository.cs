using BlueValueAssessment.Core.Data;
using BlueValueAssessment.Core.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueValueAssessment.Data.Repositories
{
    public abstract class DataRepository<T> : IDataRepository<T> where T : BaseDocument
    {
        private readonly IMongoCollection<T> _collection;

        public DataRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public async Task AddAsync(T document)
        {
            try
            {
                await _collection.InsertOneAsync(document);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<T> GetAsync(Guid documentId)
        {
            try
            {
                using (var result = await _collection.FindAsync(d => d.Id == documentId))
                {
                   return await result.FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public async Task<IList<T>> GetAllAsync()
        {
            try
            {
                var result = _collection.AsQueryable();
                return await result.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAysnc(Guid documentId)
        {
            try
            {
                await _collection.DeleteOneAsync(d => d.Id == documentId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
