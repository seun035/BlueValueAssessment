using BlueValueAssessment.Core.Documents;
using BlueValueAssessment.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueValueAssessment.Core.Services
{
    public interface IRequestService
    {
        Task SaveRequestAsync(Request request);

        Task<BaseResponse<IList<RequestDocument>>> GetRequestsAsync();

        Task<BaseResponse<RequestDocument>> GetRequestAsync(Guid requestId);

        Task<BaseResponse<object>> DeleteAsync(Guid documentId);

        Task<BaseResponse<IList<RequestDocument>>> GetAllByDateAsync(DateRange dateRange);

        Task<BaseResponse<long>> GetCountByDateAsync(DateTime date);

    }
}
