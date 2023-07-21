using Microsoft.AspNetCore.Mvc.RazorPages;
using pvo_dictionary.Model;

namespace pvo_dictionary.DTO
{
    public class AuditLogResponseDTO
    {
        public List<AuditLog> AuditLogs { get; set; }
        public int totalRecords { get; set; }
        public int totalPages { get; set; }
        public AuditLogResponseDTO(List<AuditLog> AuditLogs,int totalRecords, int totalPages)
        {
            this.AuditLogs = AuditLogs;
            this.totalRecords = totalRecords;
            this.totalPages = totalPages;
        }

    }
}
