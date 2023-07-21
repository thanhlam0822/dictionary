namespace pvo_dictionary.Model
{
    public class Dictionary : EditInfo
    {
        public Guid dictionary_id { get; set; }
        public Guid user_id { get; set; }
        public string dictionary_name { get; set; }
        public DateTime last_view_at { get; set; }
    }
}
