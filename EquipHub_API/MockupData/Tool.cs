using System.ComponentModel.DataAnnotations;

namespace EquipHub_API.MockupData
{
    public class Tool
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Owner {  get; set; }
        public string? Note { get; set; }
    }
}
