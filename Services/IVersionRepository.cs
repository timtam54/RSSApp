using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = RssMob.Models.Version;
namespace RssMob.Services
{
    public interface IVersionRepository
    {
        Task<List<VersionRpt>> Versions(int bid);
        Task<Version> Version(int Inspequipid);

        Task<Version> AddNew(Version inspEquip);
        Task<bool> Update(Version inspEquip);

        Task<bool> Delete(int id);
    }
}
