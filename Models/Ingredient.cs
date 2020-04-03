using System;
using System.ComponentModel.DataAnnotations;

namespace mfbcustomizerserver.Models
{
    public class Ingredient
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string PhotoUrl { get; set; }
        public string ShortDescp { get; set; }
        public float PricePerUnit { get; set; }
        public float Calories { get; set; }
        public int IsDeleted { get; set; }
    }
}