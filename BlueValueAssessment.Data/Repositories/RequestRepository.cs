using BlueValueAssessment.Core.Data;
using BlueValueAssessment.Core.Documents;
using BlueValueAssessment.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueValueAssessment.Data.Repositories
{
    public class RequestRepository: DataRepository<RequestDocument>, IRequestRepository
    {
        private readonly MongoDbContext _mongoDbContext;

        public RequestRepository(MongoDbContext mongoDbContext)
            :base(mongoDbContext.Requests)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task<IList<RequestDocument>> GetAllByDateAsync(DateRange dateRange)
        {
            try
            {
                var builder = Builders<RequestDocument>.Filter;
                var filter = builder.Gte(x => x.TimeStamp, dateRange.StartDate) & builder.Lte(x => x.TimeStamp, dateRange.EndDate);

                using (var results = await _mongoDbContext.Requests.FindAsync(filter))
                {
                    return await results.ToListAsync();
                }
   
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<long> GetStatsAsync(DateTime date)
        {
           DateTime beginPeriod = date.Date;
           DateTime endPeriod = date.Date.AddDays(1);

           var result = await _mongoDbContext.Requests.Aggregate<RequestDocument>()
                .Match(r => r.TimeStamp >= beginPeriod && r.TimeStamp < endPeriod)
                .Count().FirstOrDefaultAsync();

            if (result == null)
                return 0;
            return result.Count;
        }
    }
}
