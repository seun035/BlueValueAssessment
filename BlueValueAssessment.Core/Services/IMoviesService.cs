using BlueValueAssessment.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueValueAssessment.Core.Services
{
    public interface IMoviesService
    {
        public Task<BaseResponse<SearchResult>> SearchMoviesAsync(SearchQuery searchQuery);
    }
}
