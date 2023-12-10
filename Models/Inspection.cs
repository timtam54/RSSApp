using System;
using System.Collections.ObjectModel;

namespace RssMob.Models
{
    public class InspectionsViewModel
    {
        public ObservableCollection<InspectionView> InspectionViews { get; set; }
    }

    public class InspectionView
    {
        public int id { get; set; }
        public string? Areas { get; set; }
        public string? ClientName { get; set; }
        public string? Address { get; set; }
        public string? TestingInstruments { get; set; }
        public string? Inspector { get; set; }
        public DateTime InspDate { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string? Photo { get; set; }
        public string? Status { get; set; }

    }

    public class Inspection : ICloneable//, IComparable<Inspection>
    {

        public object Clone()
        {
            var ret = (Inspection)MemberwiseClone();
            return ret;
        }

        public bool Equals(Inspection other)
        {
            if (id != other.id)
                return false;
            if (InspectionDate.Date != other.InspectionDate.Date)
                return false;
            if (Areas != other.Areas)
                return false;
            if (BuildingID != other.BuildingID)
                return false;
            if (InspectorID != other.InspectorID)
                return false;
            if (TestingInstruments != other.TestingInstruments)
                return false;
            if (Photo != other.Photo)
                return false;

            return true;
        }

        public int id { get; set; }
        public DateTime InspectionDate { get; set; }
        public string? Areas { get; set; }
        //public Building? Building { get; set; }
        public int BuildingID { get; set; }
        public int? InspectorID { get; set; }
        public Employee? Inspector { get; set; }
        public string? TestingInstruments { get; set; }
        public string? Photo { get; set; }

        public List<SelectListItem> Insps { get; set; }
        public SelectListItem SelectInspectorID { get; set; }

        public List<SelectListItem> Buildings { get; set; }
        public SelectListItem SelectBuidingID { get; set; }

        public string PhotoURL
        {
            get
            {
                return "https://rssblob.blob.core.windows.net/rssimage/" + Photo;
            }
        }
        public ObservableCollection<InspEquipView> InspEquipViews { get; set; }

        public string? Status { get; set; }
    }

    public class SelectListItem
    {
      

        public string Text { get; set; }
        public int Value { get; set; }
    }
}

