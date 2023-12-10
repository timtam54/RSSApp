using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Models
{
    public class ClientsViewModel
    {
        public ObservableCollection<Client> ClientViews { get; set; }
       

      //  public ObservableCollection<Building> NewAddressViews { get; set; }
    }
    public class Client
    {
        public int id { get; set; }
        //   [Display(Name = "Building Name")]

        public string? name { get; set; }
        //public Client Client { get; set; }
        // [Display(Name = "Client")]

        public string? ContactName { get; set; }

      
    }
}

