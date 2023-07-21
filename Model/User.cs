namespace pvo_dictionary.Model
{

    public class User : EditInfo

    {

        public Guid user_id { get; set; }

        public string user_name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string? full_name { get; set; }
        public string? display_name { get; set; }

        public DateTime? birthday { get; set; }
        public string? position { get; set; }
        public string? avatar { get; set; }
        public int status { get; set; }
        public string? verify_token { get; set; }
        public string? reset_password_token { get; set; }

    }
}
