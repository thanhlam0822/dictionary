namespace pvo_dictionary.Model
{
    public class ConceptRelationship:EditInfo
    {
        
        public int concept_relationship_id { get; set; }

        
        public Guid dictionary_id { get; set; }

       
        public Guid concept_id { get; set; }

        
        public Guid parent_id { get; set; }

       
        public Guid concept_link_id { get; set; }

    }
}
