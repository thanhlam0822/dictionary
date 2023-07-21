namespace pvo_dictionary.Model
{   
    public class AuditLog:EditInfo
        {
            public int audit_log_id { get; set; }
            public Guid? user_id { get; set; }
            public string? screen_info { get; set; }
            public int? action_type { get; set; }
            public string? reference { get; set; }
            public string? description { get; set; }
            public string? user_agent { get; set; }
            

        
    }

    
}
