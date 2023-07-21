using pvo_dictionary.Model;

namespace pvo_dictionary.DTO
{
    public class GetListConceptLinkResponseDTO
    {
      public Guid ConceptLinkId { get; set; }
      public Guid SysConceptLinkId { get; set; }
      public string ConceptLinkName { get; set; }
      public string ConceptLinkType { get; set; }
      public int? SortOrder { get; set; }

    }
}
