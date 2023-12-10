using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public interface IInspEquipTestFailRepository
    {
        Task<List<InspEquipTypeTestFail>> InspEquipTypeTestFails(int Inspequiptypetestid);

        Task<InspEquipTypeTestFail> InspEquipTypeTestFail(int id);

        Task<InspEquipTypeTestFail> AddNew(InspEquipTypeTestFail inspEquipTypeTestFail);
        Task<bool> Update(InspEquipTypeTestFail inspEquipTypeTestFail);

        Task<bool> Delete(int id);
    }
}