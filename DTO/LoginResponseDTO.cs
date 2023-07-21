namespace pvo_dictionary.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; } 
        public string Avatar { get; set; } 
        public Guid DictionaryId { get; set; }
        public string DictionaryName { get; set; }
        public LoginResponseDTO(string token, Guid userId, string userName, string displayName, string avatar, Guid dictionaryId, string dictionaryName)
        {
            this.Token = token;
            this.UserId = userId;
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Avatar = avatar;
            this.DictionaryId = dictionaryId;
            this.DictionaryName = dictionaryName;
        }

    }
}
