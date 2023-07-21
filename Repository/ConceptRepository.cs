using pvo_dictionary.DTO;
using pvo_dictionary.Model;

namespace pvo_dictionary.Repository
{
    public interface ConceptRepository
    {
        ServiceResult GetNumberRecord( Guid? dictionaryId );
        ServiceResult GetListConept( Guid? dictionaryId );
        ServiceResult AddConcept(SaveConceptRequestDTO request);
        ServiceResult UpdateConcept(UpdateConceptRequestDTO request);
        ServiceResult DeleteConcept(DeleteConceptRequestDTO request);
        ServiceResult GetConcept(Guid conceptId);
        ServiceResult SearchConcept(string searchKey );
        ServiceResult GetConceptRelationship(Guid conceptId, Guid parentId);
        ServiceResult GetSaveSearch();
        ServiceResult GetTree(Guid conceptId,string conceptName);

        ServiceResult GetConceptParents(Guid conceptId);
        ServiceResult GetConceptChildren(Guid conceptId);
    }
}
