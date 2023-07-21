namespace pvo_dictionary.DTO
{
    public class UpdateConceptRequestDTO
    {
        public Guid conceptId { get; set; } 
        public string? title { get; set;}
        public string? description { get; set;} 
    }
}
