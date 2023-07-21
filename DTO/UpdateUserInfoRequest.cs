namespace pvo_dictionary.DTO
{
    public class UpdateUserInfoRequest
    {
        public string displayName { get; set; }
        public string fullName { get; set; }
        public string birthday { get; set; }
        public string position { get; set; }
        public IFormFile avatar { get; set; }
    }
}
