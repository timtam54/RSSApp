using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Models
{
    public class InspEquipTypeTestViewModel
    {
        public List<InspEquipTypeTestRpt> InspEquipTypeTests { get; set; }//ObservableCollection
    }

    public class InspEquipTypeTest : ICloneable//, IComparable<InspEquipTypeTest>
    {

        public object Clone()
        {
            var ret = (InspEquipTypeTest)MemberwiseClone();
            return ret;
        }

        public bool Equals(InspEquipTypeTest other)
        {
            if (id != other.id)
                return false;
            if (InspEquipID != other.InspEquipID)
                return false;
            if (EquipTypeTestID != other.EquipTypeTestID)
                return false;
           // if (Pass != other.Pass)
             //   return false;
            if (Comment != other.Comment)
                return false;
            if (Reason != other.Reason)
                return false;
            if (EquipTypeTestID != other.EquipTypeTestID)
                return false;
            return true;
        }
        public int id { get; set; }

        public int InspEquipID { get; set; }

        public int EquipTypeTestID { get; set; }

       // public int Pass { get; set; }

       // public object Ps { get; set; }
        public string? Comment { get; set; }
        public string? Reason { get; set; }

        public List<SelectListItem> Ett { get; set; }

      public  List<EquipTypeTestFailInsp> EETFs { get; set; }
        

        public string? EquipTypeTestDesc { get; set; }

        public ObservableCollection<SelectListItem> ETTs { get; set; }

        // public ObservableCollection<EquipTypeTestHazards> equipTypeTestHazards { get; set; }//todotim 16/4/23
        public ObservableCollection<EquipTypeTestFailInsp> equipTypeTestFail { get; set; }
    }


    public class InspEquipTypeTestRpt
    {
        public int Severity { get; set; }
        public int EquipTypeTestID { get; set; }
        public int id { get; set; }

        public int InspEquipID { get; set; }

        public String EquipTypeTest { get; set; }

      //  public int Pass { get; set; }
        public string? Comment { get; set; }
        public string PassFail
        {
            get
            {
                if (Severity == 0)
                    return "Rec";
                return "Fail";
                //else if (Pass==1)
                  //  return "Pass";
                
            }
        }

        public Color PassFailColour
        {
            get
            {
                
                if (Severity == 0)
                    return Microsoft.Maui.Graphics.Color.FromRgb(34, 139, 34);
                return Microsoft.Maui.Graphics.Color.FromRgb(255, 0, 0);
            //    else if (Pass == 1)
             //       return Microsoft.Maui.Graphics.Color.FromRgb(34, 139, 34);
               // else
                 //   return Microsoft.Maui.Graphics.Color.FromRgb(255, 128, 0);
            }
        }
        public string? Reason { get; set; }

        //public List<SelectListItem> Ett { get; set; }
        // public SelectListItem SelEquipTypeTest { get; set; }
    }
}
