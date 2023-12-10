using Microsoft.Maui.Controls;
using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public interface IClientRepository
    {
        Task<List<Client>> Clients(string search);
    }
}

