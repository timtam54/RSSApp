using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Models
{
    
    public class Building : ICloneable
    {
        public object Clone()
        {
            var ret = (InspEquip)MemberwiseClone();
            return ret;
        }
        public ObservableCollection<InspPhoto> BuildingPhotos { get; set; }
        public bool Equals(Building other)
        {
            if (id != other.id)
                return false;
            if (ClientName != other.ClientName)
                return false;
            if (BuildingName != other.BuildingName)
                return false;
            if (ClientID != other.ClientID)
                return false;
            if (Address != other.Address)
                return false;
            return true;
        }
        public int id { get; set; }
        public string? ClientName { get; set; }

        public string? BuildingName { get; set; }
        //public Client Client { get; set; }
        // [Display(Name = "Client")]

        public int ClientID { get; set; }
        public string? ContactName { get; set; }
        public string? ContactNumber { get; set; }
        public string? AccessDetails { get; set; }

        public string? Address { get; set; }

        public List<SelectListItem> Clients { get; set; }
        public SelectListItem SelectClientID { get; set; }
    }

    public class BuildingsViewModel
    {
        public ObservableCollection<Building> BuildingViews { get; set; }


        public ObservableCollection<Building> NewAddressViews { get; set; }
    }
}

