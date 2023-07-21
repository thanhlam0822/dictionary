namespace pvo_dictionary.DTO
{
    public class UpdateUserInfoResponseDTO
    {
        public string displayName { get; set; }
        public string fullName { get; set; }
        public string birthday { get; set; }
        public string position { get; set; }
        public string avatar { get; set; }
        public UpdateUserInfoResponseDTO(string displayName,string fullName,string birthday,string position,string avatar)
        {
            this.displayName = displayName;
            this.fullName = fullName;
            this.birthday = birthday;
            this.position = position;
            this.avatar = avatar;
        }
    }
}
