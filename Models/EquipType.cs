using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace RssMob.Models
{
    public class EquipType
    {
        public int id { get; set; }
        [Display(Name = "Equipment Type")]
        public string? EquipTypeDesc { get; set; }
    }
}
