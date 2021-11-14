using BlueValueAssessment.Core.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueValueAssessment.Core.Data
{
    public interface IDataRepository<T> where T: BaseDocument
    {

        Task AddAsync(T document);

        Task<T> GetAsync(Guid documentId);

        Task<IList<T>> GetAllAsync();

        Task DeleteAysnc(Guid documentId);
    }
}
