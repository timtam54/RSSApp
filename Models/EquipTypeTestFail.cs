using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Models
{
    public class EquipTypeTestFailViewModel
    {
        //public string TestingInstruments { get; set; }
        //public string Areas { get; set; }
        //public string InspectionDate { get; set; }
        public ObservableCollection<EquipTypeTestFail> EquipTypeTestFails { get; set; }
    }
    public class EquipTypeTestFail
    {
        public int id { get; set; }
        public int? EquipTypeTestID { get; set; }
        public string? FailReason { get; set; }

    }

    public class EquipTypeTestFailInsp
    {
        public int id { get; set; }
        public int? EquipTypeTestID { get; set; }
        public string? FailReason { get; set; }
        //public bool? Failed { get; set; }
    }
}
