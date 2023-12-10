using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace RssMob.Models
{
    public class Hazard
    {
        public int id { get; set; }

        [Display(Name = "Hazard If Non Compliant")]

        public string? Detail { get; set; }
    }
}
