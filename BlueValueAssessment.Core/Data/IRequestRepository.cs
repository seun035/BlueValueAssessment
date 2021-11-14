using BlueValueAssessment.Core.Documents;
using BlueValueAssessment.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueValueAssessment.Core.Data
{
    public interface IRequestRepository: IDataRepository<RequestDocument>
    {
        Task<IList<RequestDocument>> GetAllByDateAsync(DateRange dateRange);

        Task<long> GetStatsAsync(DateTime date);
    }
}
