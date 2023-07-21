using pvo_dictionary.Data;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;

namespace pvo_dictionary.Repository
{
    public interface AuditLogRepository
    {

        ServiceResult GetAuditLogs(string dateFrom, string dateTo, int pageIndex, int pageSize, string searchFilter, out int totalRecords, out int totalPages);
        ServiceResult SaveLog(SaveAuditLogRequestDTO requestDTO);
    }
}
