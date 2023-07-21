using MailKit;
using Microsoft.AspNetCore.Mvc;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;
using pvo_dictionary.Service;

namespace pvo_dictionary.Controllers
{
    [Route("api/concept")]
    [ApiController]
    public class ConceptController : ControllerBase
    {
        private readonly ConceptRepository conceptRepository;
        public ConceptController(ConceptRepository conceptRepository)
        {
            this.conceptRepository = conceptRepository;
        }
        [HttpGet("get_number_record")]
        public ServiceResult GetNumberRecord(Guid dictionaryId)
        {
            return conceptRepository.GetNumberRecord(dictionaryId);
        }
        [HttpGet("get_list_concept")]
        public ServiceResult GetListConept(Guid? dictionaryId)
        {
            return conceptRepository.GetListConept(dictionaryId);
        }
        [HttpPost("add_concept")]
        public ServiceResult AddConcept([FromBody] SaveConceptRequestDTO request)
        {
            
            return conceptRepository.AddConcept(request);
        }
        [HttpPost("update_concept")]
        public ServiceResult UpdateConcept([FromBody] UpdateConceptRequestDTO request)
        {

            return conceptRepository.UpdateConcept(request);
        }
        [HttpDelete("delete_concept")]
        public ServiceResult DeleteConcept([FromBody] DeleteConceptRequestDTO request)
        {
            return conceptRepository.DeleteConcept(request);
        }
        [HttpGet("get_concept")]
        public ServiceResult GetConcept(Guid conceptId)
        {
            return conceptRepository.GetConcept(conceptId);
        }
        [HttpGet("search_concept")]
        public ServiceResult SearchConcept(string searchKey)
        {
            return conceptRepository.SearchConcept(searchKey);
        }
        [HttpGet("get_concept_relationship")]
        public ServiceResult GetConceptRelationship(Guid conceptId, Guid parentId)
        {
            return conceptRepository.GetConceptRelationship(conceptId, parentId);
        }
        [HttpGet("get_saved_search")]
        public ServiceResult GetSavedSearch()
        {
            return conceptRepository.GetSaveSearch();
        }
        [HttpGet("get_tree")]
        public ServiceResult GetTree(Guid conceptId, string conceptName = "")
        {
            return conceptRepository.GetTree( conceptId,  conceptName);
        }
        [HttpGet("get_concept_parents")]
        public ServiceResult GetConceptParents(Guid conceptId)
        {
            return conceptRepository.GetConceptParents(conceptId);
        }
        [HttpGet("get_concept_children")]
        public ServiceResult GetConceptChildren(Guid conceptId)
        {
            return conceptRepository.GetConceptChildren(conceptId);
        }

    }
  }