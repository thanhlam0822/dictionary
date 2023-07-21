namespace pvo_dictionary.DTO
{
    public class UpdateConceptRelationshipResponseDTO
    {
       public Guid conceptId { get; set; }
       public Guid parentId { get; set; }
       public Guid conceptLinkId { get; set; } 
       public bool isForce { get; set; }
    }
}
