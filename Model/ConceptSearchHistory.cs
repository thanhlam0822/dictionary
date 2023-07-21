namespace pvo_dictionary.Model
{
    public class ConceptSearchHistory:EditInfo
    {
        public int id { get; set; }
        public Guid user_id { get; set; }
        public Guid dictionary_id { get; set; }
        public string list_concept_search { get;set; }
    }
}
