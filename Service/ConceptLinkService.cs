using Microsoft.AspNetCore.Http;
using pvo_dictionary.Data;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;
using System.Security.Claims;

namespace pvo_dictionary.Service
{
    public class ConceptLinkService : ConceptLinkRepository
    {
        private readonly DataContext dataContext;
        private readonly IHttpContextAccessor httpContext;
        public ConceptLinkService(DataContext dataContext, IHttpContextAccessor httpContext)
        {
            this.dataContext = dataContext;
            this.httpContext = httpContext;
        }

       
        public ServiceResult GetListConceptLink()
        {   
            List<GetListConceptLinkResponseDTO> responseDTOs = new List<GetListConceptLinkResponseDTO>();
            var userEmail = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            var user = dataContext.user.FirstOrDefault(u => u.email == userEmail);
            List<ConceptLink> conceptLinks = dataContext.concept_link.Where(c => c.user_id == user.user_id).OrderBy(c=>c.sort_order).ToList();
            foreach(var conceptLink in conceptLinks)
            {
                GetListConceptLinkResponseDTO conceptLinkResponseDTO = new GetListConceptLinkResponseDTO();
                conceptLinkResponseDTO.ConceptLinkId = conceptLink.concept_link_id;
                conceptLinkResponseDTO.SysConceptLinkId = conceptLink.sys_concept_link_id;
                conceptLinkResponseDTO.ConceptLinkName = conceptLink.concept_link_name;
                conceptLinkResponseDTO.ConceptLinkType = conceptLink.concept_link_type.ToString();
                conceptLinkResponseDTO.SortOrder = conceptLink.sort_order;
                responseDTOs.Add(conceptLinkResponseDTO);

            }
            return new ServiceResult(ServiceResultStatus.Success, responseDTOs, null, null);
        }
        
    }
}
