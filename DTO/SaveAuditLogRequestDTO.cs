namespace pvo_dictionary.DTO
{
    public class SaveAuditLogRequestDTO
    {
        
       
        public string? ScreenInfo { get; set; }
        public int? ActionType { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
        
    }
}
