using Microsoft.AspNetCore.Mvc;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;
using System.Runtime.ConstrainedExecution;

namespace pvo_dictionary.Controllers
{
    [Route("api/userconfig")]
    [ApiController]
    public class UserConfigController:ControllerBase
    {
        private readonly UserConfigRepository repository;
        public UserConfigController(UserConfigRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet("get_list_example_link")]
        public ServiceResult GetListExampleLink()
        {
            return repository.GetListExampleLink();
        }
        [HttpGet("get_list_example_attribute")]
        public ServiceResult GetListExampleAttribute()
        {
            return repository.GetListExampleAttribute();
        }
        [HttpDelete("delete_example")]
        public ServiceResult DeleteExample(Guid exampleID)
        {
            return repository.DeleteExample(exampleID);
        }

    }
}
