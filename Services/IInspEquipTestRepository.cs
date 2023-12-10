using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public interface IInspEquipTestRepository
    {
        Task<List<InspEquipTypeTestRpt>> InspTests(int Inspequipid);

        Task<InspEquipTypeTest> InspTest(int id);

        Task<InspEquipTypeTest> AddNew(InspEquipTypeTest inspEquipTypeTest);
        Task<bool> Update(InspEquipTypeTest inspEquipTypeTest);

        Task<bool> Delete(int id);
    }
}