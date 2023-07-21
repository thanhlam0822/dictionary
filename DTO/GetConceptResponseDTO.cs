using pvo_dictionary.Model;
using System.Collections;

namespace pvo_dictionary.DTO
{
    public class GetConceptResponseDTO
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public IList Examples { get; set; }
    }
}
