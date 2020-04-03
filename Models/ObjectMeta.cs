using System.ComponentModel.DataAnnotations;

namespace mfbcustomizerserver.Models
{
    public class ObjectMeta
    {
        [Key] public string Id { get; set; }
        public string ObjectId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}