using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_API.Models.Dto
{
    public class VillaDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
        public string Amenity { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        public string ImageUrl { get; set; }
    }
}