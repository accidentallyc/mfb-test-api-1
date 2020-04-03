using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mfbcustomizerserver.Models
{
    public class ObjectStack
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("Id")]
        public string ParentId { get; set; }
        public string ItemId { get; set; }
        public float TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int IsDeleted { get; set; }
        public int IsPersisted { get; set; }
        public int Type { get; set; } //sqlite doesnt support enum conversion
        [NotMapped]
        public object Item { get; set; }
        
        public static class StackTypes
        {
            public static readonly int Ingredient = 1;
            public static readonly int Recipe = 2;
        }
    }
}