using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mfbcustomizerserver.Models
{
    public class FoodBag
    {
        [Key] 
        public string Id { get; set; } // TODO: Essentially a guid but sqlite has no uuid/guid type
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public float TotalCalories { get; set; }
        public float TotalPrice { get; set; }
        public float TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int IsPersisted { get; set; }
        public string CreatorId { get; set; }
        
        [NotMapped]
        public User Creator { get; set; }
        [NotMapped]
        public string Description { get; set; }
        [NotMapped]
        [ForeignKey("ParentId")]
        public List<ObjectStack> IngredientStacks { get; set; }
        [NotMapped]
        [ForeignKey("ParentId")]
        public List<ObjectStack> RecipeStacks { get; set; }

    }
}