using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Views
{
    public interface iRefreshData
    {
        Task<bool> RefreshDataAsync();
        void NewID(int id);
    }
}
