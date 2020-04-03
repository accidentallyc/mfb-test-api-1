using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mfbcustomizerserver.Models
{
    public class Recipe
    {
        [Key] 
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string CookTime { get; set; }
        public int IsDeleted { get; set; }
        [NotMapped]
        [ForeignKey("ParentId")]
        public List<ObjectStack> IngredientStacks { get; set; }
    }
}