using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public interface IHazardRepository
    {
        Task<List<Hazard>> Hazards();
        Task<Hazard> AddNew(Hazard hazard);
    }
}

