namespace pvo_dictionary.Model
{
    public class Concept:EditInfo
    {
        
        
        public Guid concept_id { get; set; }

        
        public Guid dictionary_id { get; set; }

        
        public string title { get; set; }

        
        public string description { get; set; }

        
        
    }
}
