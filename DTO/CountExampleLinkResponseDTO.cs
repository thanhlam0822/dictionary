using pvo_dictionary.Model;

namespace pvo_dictionary.DTO
{
    public class CountExampleLinkResponseDTO
    {
        public Guid ExampleLinkId { get; set; }
        public string ExampleLinkName { get; set; }
        public string Count { get; set; }
        public int SortOrder {get;set ;}
   }
}
