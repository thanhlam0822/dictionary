using pvo_dictionary.Model;

namespace pvo_dictionary.DTO
{
    public class ListConceptResponseDTO
    {
        public List<Concept> ListConcept {  get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
