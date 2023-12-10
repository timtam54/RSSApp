using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Models
{
    public class EquipTypeTestHazardViewModel
    {
        public List<EquipTypeTestHazards> equipTypeTestHazards { get; set; }//ObservableCollection
    }
    public class EquipTypeTestHazards
    {
        public int id { get; set; }
        public int HazardID { get; set; }
        public int EquipTypeTestID { get; set; }
        public Hazard? Hazard { get; set; }
       // public string? Haz { get; set; }

        public List<SelectListItem> Hazards { get; set; }
        public SelectListItem SelHazard { get; set; }
    }

    public class EquipTypeTestHazardsRpt
    {
        public int id { get; set; }
        public int HazardID { get; set; }
        public int EquipTypeTestID { get; set; }
        public Hazard? Hazard { get; set; }
        public string? Haz { get; set; }
    }
}
