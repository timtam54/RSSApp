using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public interface IInspPhotoRepository
    {
        Task<List<InspPhoto>> InspPhotos(int Inspequipid, string SoureTable);
        Task<bool> Delete(int id);
    }
}

