using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using pvo_dictionary.Data;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Claims;

namespace pvo_dictionary.Service
{
    public class AuditLogService: AuditLogRepository
    {
        private readonly DataContext dataContext;
        private readonly IHttpContextAccessor httpContext;


        public AuditLogService(DataContext dataContext, IHttpContextAccessor httpContext)
        {
            this.dataContext = dataContext;
            this.httpContext = httpContext;

        }
        
        public ServiceResult GetAuditLogs(string dateFrom, string dateTo, int pageIndex, int pageSize, string searchFilter, out int totalRecords, out int totalPages)
        {
            using (var connection = new MySqlConnection("Server=localhost; User ID=root; Password=123456; Database=dictionary"))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@dateFrom", dateFrom, DbType.String, ParameterDirection.Input);
                parameters.Add("@dateTo", dateTo, DbType.String, ParameterDirection.Input);
                parameters.Add("@pageIndex", pageIndex, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@pageSize", pageSize, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@searchFilter", searchFilter, DbType.String, ParameterDirection.Input);
                parameters.Add("@TotalRecords", DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@TotalPages", DbType.Int32, direction: ParameterDirection.Output);

                var auditLogs = connection.Query<AuditLog>("get_log_procedure", parameters, commandType: CommandType.StoredProcedure).ToList();

                totalRecords = parameters.Get<int>("@TotalRecords");
                totalPages = parameters.Get<int>("@TotalPages");
                AuditLogResponseDTO response = new AuditLogResponseDTO(auditLogs, totalRecords, totalPages);
                return new ServiceResult(ServiceResultStatus.Success,response,null,null);
                ;
            }
        }

        public ServiceResult SaveLog(SaveAuditLogRequestDTO requestDTO)
        {
            var userEmail = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            var user = dataContext.user.FirstOrDefault(u => u.email == userEmail);
            string userAgent = httpContext.HttpContext.Request.Headers["User-Agent"].ToString();
            AuditLog auditLog = new AuditLog();
            auditLog.user_id = user.user_id;
            auditLog.screen_info = requestDTO.ScreenInfo;
            auditLog.action_type = requestDTO.ActionType;
            auditLog.reference = requestDTO.Reference;
            auditLog.description = requestDTO.Description;
            auditLog.user_agent = userAgent;
            auditLog.created_at = DateTime.Now;
            dataContext.audit_log.Add(auditLog);
            dataContext.SaveChanges();
            return new ServiceResult(ServiceResultStatus.Success,null,null,null);
        }
    }
}
