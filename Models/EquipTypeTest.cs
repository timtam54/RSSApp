using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Models
{
    public class EquipTypeTestViewModel
    {
        //public string TestingInstruments { get; set; }
        //public string Areas { get; set; }
        //public string InspectionDate { get; set; }
        public ObservableCollection<EquipTypeTest> EquipTypeTests { get; set; }
    }
    public class EquipTypeTest
    {
        public int id { get; set; }
        public int? EquipTypeID { get; set; }
        public string? Test { get; set; }
        public int? Severity { get; set; }

        public EquipType? EquipType { get; set; }
        public ObservableCollection<EquipTypeTestHazards> equipTypeTestHazards { get; set; }//todotim 16/4/23
    }
}
