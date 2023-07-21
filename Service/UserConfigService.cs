using pvo_dictionary.Data;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;
using System.Security.Claims;

namespace pvo_dictionary.Service
{
    public class UserConfigService : UserConfigRepository
    {
        private readonly DataContext dataContext;
        private readonly IHttpContextAccessor httpContext;
        public UserConfigService(DataContext dataContext, IHttpContextAccessor httpContext)
        {
            this.dataContext = dataContext;
            this.httpContext = httpContext;
        }

        

        public ServiceResult GetListExampleLink()
        {
            var userEmail = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            var user = dataContext.user.FirstOrDefault(u => u.email == userEmail);
            List<ExampleLinkResponseDTO> responseDTOs = new List<ExampleLinkResponseDTO>();
            List<ExampleLink> exampleLinks = dataContext.example_link.Where(e => e.user_id == user.user_id).ToList();
            foreach(var data in exampleLinks)
            {
                ExampleLinkResponseDTO responseDTO = new ExampleLinkResponseDTO();
                responseDTO.ExampleLinkId = data.example_link_id;
                responseDTO.SysExampleLinkId = data.sys_example_link_id;
                responseDTO.ExampleLinkName = data.example_link_name;
                responseDTO.ExampleLinkType = data.example_link_type.ToString();
                responseDTO.SortOrder = data.sort_order;
                responseDTOs.Add(responseDTO);
            }
            return new ServiceResult(ServiceResultStatus.Success,responseDTOs,null,null);
        }
        public ServiceResult GetListExampleAttribute()
        {
            var userEmail = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            var user = dataContext.user.FirstOrDefault(u => u.email == userEmail);
            List<Tone> Tone = dataContext.tone.Where(t => t.user_id == user.user_id).ToList();
            List<Mode> Mode = dataContext.mode.Where(t => t.user_id == user.user_id).ToList();
            List<Register> Register = dataContext.register.Where(t => t.user_id == user.user_id).ToList();
            List<Nuance> Nuance = dataContext.nuance.Where(t => t.user_id == user.user_id).ToList();
            List<Dialect> Dialect = dataContext.dialect.Where(t => t.user_id == user.user_id).ToList();
            ListExampleAttributeResponseDTO responseDTO = new ListExampleAttributeResponseDTO();
            responseDTO.Tone = Tone;
            responseDTO.Mode = Mode;
            responseDTO.Register = Register;
            responseDTO.Nuance = Nuance;
            responseDTO.Dialect = Dialect;
            return new ServiceResult(ServiceResultStatus.Success, responseDTO, null, null);

        }

        public ServiceResult DeleteExample(Guid exampleId)
        {
            Example example = dataContext.example.FirstOrDefault(e => e.example_id == exampleId);
            if(example != null) {
                dataContext.example.Remove(example);
                dataContext.SaveChanges();
            }
            return new ServiceResult(ServiceResultStatus.Success, null, null, null);
        }
    }
}
