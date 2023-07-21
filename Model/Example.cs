namespace pvo_dictionary.Model
{
    public class Example:EditInfo
    {
        public Guid example_id { get; set; }
        public Guid dictionary_id { get; set; }
        public string? detail { get; set; } 
        public string? detail_html { get; set; }    
        public string? note { get; set; }
        public Guid tone_id { get; set; } 
        public Guid register_id { get; set; }
        public Guid dialect_id { get; set; } 
        public Guid mode_id { get; set; }   
        public Guid nuance_id { get; set; }
    }
}
