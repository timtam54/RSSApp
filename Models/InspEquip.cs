using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RssMob.Models
{
    public class InspEquipViewModel
    {
        public string TestingInstruments { get; set; }
        public string Areas { get; set; }
        public string InspectionDate { get; set; }
        public ObservableCollection<InspEquipView> InspEquipViews { get; set; }
    }

    public class InspEquip:ICloneable//,IComparable<InspEquip>
    {

        public object Clone()
        {
            var ret = (InspEquip)MemberwiseClone();
            return ret;
        }

        public bool Equals(InspEquip other)
        {
            if (id != other.id)
                return false;
            if (EquipTypeID != other.EquipTypeID)
                return false;
            if (InspectionID != other.InspectionID)
                return false;
            if (Location != other.Location)
                return false;
            if (Notes != other.Notes)
                return false;
            if (Manufacturer != other.Manufacturer)
                return false;
            if (SerialNo != other.SerialNo)
                return false;
            if (Rating != other.Rating)
                return false;
            if (Qty != other.Qty)
                return false;
            if (Installer != other.Installer)
                return false;
            if (WithdrawalDate != other.WithdrawalDate)
                return false;
            return true;
        }

        //  public List<SelectListItem> Et { get; set; }
        // public SelectListItem SelEquipType { get; set; }
        public string SelEquipType { get; set; }
        public int id { get; set; }
        [Display(Name = "Type")]
        public int EquipTypeID { get; set; }
        [Display(Name = "Inspection")]
        public int InspectionID { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }

        public Inspection? Inspection { get; set; }
     
        public string? Manufacturer { get; set; }
        public string? Installer { get; set; }
        public string? Rating { get; set; }
        public string? SerialNo { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public string? requiredControls { get; set; }
        public ObservableCollection<InspPhoto> InspPhotos { get; set; }
        public List<InspEquipTypeTestRpt> InspEquipTypeTests { get; set; }//ObservableCollection
        public int? Qty { get; set; }

        public int? Ordr { get; set; }
    }
    public class InspEquipView
    {
        public int? Ordr { get; set; }
        public int id { get; set; }
        [Display(Name = "Type")]
        public string? EquipDesc { get; set; }
        public int InspectionID { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
        public string? Photos { get; set; }

        public string? Manufacturer { get; set; }
        public string? Installer { get; set; }
        public string? Rating { get; set; }
        public string? SerialNo { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public string? requiredControls { get; set; }
    }



}