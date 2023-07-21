namespace pvo_dictionary.Model
{
    public class Tone
    {
        
        public Guid tone_id { get; set; }

       
        public Guid sys_tone_id { get; set; }

        
        public Guid user_id { get; set; }

        
        public string tone_name { get; set; }

        
        public int tone_type { get; set; }

       
        public int sort_order { get; set; }
    }
}
