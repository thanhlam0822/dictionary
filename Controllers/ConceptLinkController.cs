using Microsoft.AspNetCore.Mvc;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;

namespace pvo_dictionary.Controllers
{
    [Route("api/conceptlink")]
    [ApiController]
    public class ConceptLinkController:ControllerBase
    {
        private readonly ConceptLinkRepository conceptLinkRepository;
        public ConceptLinkController(ConceptLinkRepository conceptLinkRepository)
        {
            this.conceptLinkRepository = conceptLinkRepository;
        }

        [HttpGet("get_list_concept_link")]
        public ServiceResult GetListConceptLink()
        {
            return conceptLinkRepository.GetListConceptLink();
        }
        
    }
}
