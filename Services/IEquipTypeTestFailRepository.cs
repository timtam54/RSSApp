using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public interface IEquipTypeTestFailRepository
    {
        Task<List<EquipTypeTestFail>> EquipTypeTestFails(int EquipTypeTestID);


        Task<EquipTypeTestFail> EquipTypeTestFail(int EquipTypeTestFailID);

        Task<EquipTypeTestFail> AddNew(EquipTypeTestFail equipTypeTestFail);
        Task<bool> Update(EquipTypeTestFail equipTypeTestFail);

        Task<bool> Delete(int id);
    }
}
