using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public interface IEquipTypeRepository
    {
        Task<List<EquipType>> EquipTypes();
    }
}

