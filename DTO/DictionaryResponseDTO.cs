namespace pvo_dictionary.DTO
{
    public class DictionaryResponseDTO
    {
        public Guid DictionaryId;
        public string DictionaryName;
        public DateTime LastViewAt;
        public DictionaryResponseDTO() { }
        public DictionaryResponseDTO(Guid DictionaryId, string DictionaryName, DateTime LastViewAt)
        {
            this.DictionaryId = DictionaryId;
            this.DictionaryName = DictionaryName;
            this.LastViewAt = LastViewAt;
        }
    }
}
