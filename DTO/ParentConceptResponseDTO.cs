using pvo_dictionary.Model;

namespace pvo_dictionary.DTO
{
    public class ParentConceptResponseDTO
    {
       public Guid ConceptId { get; set; }
       public string Title { get; set; }
       public Guid ConceptLinkId { get; set; } 
       public string ConceptLinkName { get; set; }
       public int SortOrder { get; set; }

    }
}
