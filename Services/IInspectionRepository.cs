using System;
using RssMob.Models;

namespace RssMob.Services
{
    public interface IInspectionRepository
    {
        Task<List<InspectionView>> Inspections(string search,DateTime DteFrom,DateTime DteTo);
        Task<Inspection> Inspection(int id);

        Task<Inspection> Copy(int id);
        Task<Inspection> AddNew(Inspection inspection);
        Task<bool> Update(Inspection inspection);
        Task<bool> Delete(int id);
    }
}

