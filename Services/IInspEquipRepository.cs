using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public interface IInspEquipRepository
    {
        Task<List<InspEquipView>> InspItems(int Inspectionid);
        Task<InspEquip> InspItem(int Inspequipid);

        Task<InspEquip> AddNew(InspEquip inspEquip);
        Task<bool> Update(InspEquip inspEquip);

        Task<bool> Delete(int id);
    }
}

