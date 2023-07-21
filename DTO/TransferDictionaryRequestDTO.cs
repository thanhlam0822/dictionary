namespace pvo_dictionary.DTO
{
    public class TransferDictionaryRequestDTO
    {
        public Guid sourceDictionaryId;
        public Guid destDictionaryId;
        public bool isDeleteDestData;
        public TransferDictionaryRequestDTO() { 
        }
        public TransferDictionaryRequestDTO(Guid sourceDictionaryId, Guid destDictionaryId, bool isDeleteDestData)
        {
            this.sourceDictionaryId = sourceDictionaryId;
            this.destDictionaryId = destDictionaryId;
            this.isDeleteDestData = isDeleteDestData;
        }

    }
}
