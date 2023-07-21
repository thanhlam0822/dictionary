using System.ComponentModel.DataAnnotations;

namespace pvo_dictionary.DTO
{
    public class SaveConceptRequestDTO
    {
        [Required]
        public string title { get; set; }
        public string? description { get; set; }
        
        public string dictionaryId { get; set; }  
    }
}
