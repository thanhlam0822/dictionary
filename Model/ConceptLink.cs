namespace pvo_dictionary.Model
{
    public class ConceptLink:EditInfo
    {
        
        public Guid concept_link_id { get; set; }

        
        public Guid sys_concept_link_id { get; set; }

        
        public Guid user_id { get; set; }

        
        public string concept_link_name { get; set; }

        
        public int? concept_link_type { get; set; }

        
        public int sort_order { get; set; }
    }

}
