using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public interface IETTestHazardRepository
    {
        Task<List<EquipTypeTestHazards>> InspETTestHazards(int Inspequipid);

        Task<EquipTypeTestHazards> InspETTestHazard(int id);

        Task<EquipTypeTestHazards> AddNew(EquipTypeTestHazards inspEquipTypeTest);
        Task<bool> Update(EquipTypeTestHazards inspEquipTypeTest);

        Task<bool> Delete(int id);
    }
}