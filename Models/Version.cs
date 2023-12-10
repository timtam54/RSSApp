
namespace RssMob.Models
{
   
    public class Version
    {
        //public int id { get; set; }
        ////   [Display(Name = "Building Name")]

        //public string? BuildingName { get; set; }
        ////public Client Client { get; set; }
        //// [Display(Name = "Client")]

        //public int ClientID { get; set; }

        //public string? Address { get; set; }

        //public List<SelectListItem> Clients { get; set; }
        //public SelectListItem SelectClientID { get; set; }
        //public int AuthorID { get; set; }
        //public List<SelectListItem> Authors { get; set; }
        //public SelectListItem SelectAuthorID { get; set; }

        
        public int id { get; set; }
        public int? VersionNo { get; set; }
        //public string? VersionType { get; set; }
        public int AuthorID { get; set; }
        public string? Information { get; set; }
        //[Display(Name = "Inspection")]
        public int InspectionID { get; set; }

        public List<SelectListItem> Authors { get; set; }
        public SelectListItem SelectAuthorID { get; set; }

    
        
    }

    public class VersionRpt
    {
        public int id { get; set; }
        public int? VersionNo { get; set; }
        public string? VersionType { get; set; }
        public string? Author { get; set; }
        public string? Information { get; set; }

        public int InspectionID { get; set; }
    }
}
