namespace pvo_dictionary.Model
{
    public class ExampleRelationship:EditInfo
    {
        
        public int example_relationship_id { get; set; }

        
        public Guid? dictionary_id { get; set; }

       
        public Guid concept_id { get; set; }

        
        public Guid? example_id { get; set; }

       
        public Guid? example_link_id { get; set; }
    }
}
