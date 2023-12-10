using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public interface IEquipTypeTestRepository
    {
        Task<List<EquipTypeTest>> EquipTypeTests(int EquipTypeID);


        Task<EquipTypeTest> EquipTypeTest(int EquipTypeID);

        Task<EquipTypeTest> AddNew(EquipTypeTest equipTypeTest);
        Task<bool> Update(EquipTypeTest equipTypeTest);

        Task<bool> Delete(int id);
    }
}
