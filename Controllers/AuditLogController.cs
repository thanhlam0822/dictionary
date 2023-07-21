using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;
using pvo_dictionary.Service;

namespace pvo_dictionary.Controllers
{
    [Route("api/auditlog")]
    [ApiController]
    public class AuditLogController
    {
        private readonly AuditLogRepository auditLogRepository;

        public AuditLogController(AuditLogRepository auditLogRepository)
        {
            this.auditLogRepository = auditLogRepository;

        }
        [HttpGet("get_logs")]

        public ServiceResult GetAuditLogs(string dateFrom, string dateTo, int pageIndex = 0, int pageSize = 10, string? searchFilter = " ")
        {
            int totalRecords;
            int totalPages;
            var auditLogs = auditLogRepository.GetAuditLogs(dateFrom, dateTo, pageIndex, pageSize, searchFilter, out totalRecords, out totalPages);
            return auditLogs;
        }
        [HttpPost("save_log"),Authorize]
        public ServiceResult SaveLog( [FromBody] SaveAuditLogRequestDTO requestDTO)
        {
           return auditLogRepository.SaveLog(requestDTO);

        }
    }
}
