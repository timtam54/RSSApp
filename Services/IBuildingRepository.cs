using Microsoft.Maui.Controls;
using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public interface IBuildingRepository
    {
        Task<Building> AddNew(Building hazard);
        Task<List<Building>> Buildings(string search);

        Task<Building> Building(int id);
    }
}

