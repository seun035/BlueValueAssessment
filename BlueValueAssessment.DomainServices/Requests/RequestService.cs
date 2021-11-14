using BlueValueAssessment.Core.Data;
using BlueValueAssessment.Core.Documents;
using BlueValueAssessment.Core.Helpers;
using BlueValueAssessment.Core.Models;
using BlueValueAssessment.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueValueAssessment.DomainServices.Requests
{
    public class RequestService: IRequestService
    {
        private readonly IRequestRepository _requestRepository;

        public RequestService(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task SaveRequestAsync(Request request)
        {
            var document = new RequestDocument
            {
                CreatedDateUtc = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                ImdbID = request.ImdbID,
                IpAddress = request.IpAddress,
                ModifiedDateUtc = DateTime.UtcNow,
                SearchToken = request.SearchToken,
                TimeStamp = request.TimeStamp,
                ProcessingTimeMs = request.ProcessingTimeMs
            };

           await  _requestRepository.AddAsync(document);
        }

        public async Task<BaseResponse<RequestDocument>> GetRequestAsync(Guid requestId)
        {
            var request = await _requestRepository.GetAsync(requestId);
            return new BaseResponse<RequestDocument>
            {
                Data = request,
                Message = ResponseMessage.OperationSuccessful,
                StatusCode = 200
            };
        }

        public async Task<BaseResponse<IList<RequestDocument>>> GetRequestsAsync()
        {
            var requests = await _requestRepository.GetAllAsync();
            return new BaseResponse<IList<RequestDocument>>
            {
                Data = requests,
                Message = ResponseMessage.OperationSuccessful,
                StatusCode = 200
            };
        }

        public async Task<BaseResponse<IList<RequestDocument>>> GetAllByDateAsync(DateRange dateRange)
        {
            var requests = await _requestRepository.GetAllByDateAsync(dateRange);
            return new BaseResponse<IList<RequestDocument>>
            {
                Data = requests,
                Message = ResponseMessage.OperationSuccessful,
                StatusCode = 200
            };
        }

        public async Task<BaseResponse<long>> GetCountByDateAsync(DateTime date)
        {
            var count = await _requestRepository.GetStatsAsync(date);
            return new BaseResponse<long>
            {
                Data = count,
                Message = ResponseMessage.OperationSuccessful,
                StatusCode = 200
            };
        }

        public async Task<BaseResponse<object>> DeleteAsync(Guid documentId)
        {

            await _requestRepository.DeleteAysnc(documentId);

            return new BaseResponse<object>
            {
                Message = ResponseMessage.OperationSuccessful,
                StatusCode = 200
            };
        }
    }
}
