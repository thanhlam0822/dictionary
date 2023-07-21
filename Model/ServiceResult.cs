namespace pvo_dictionary.Model
{
    public class ServiceResult
    {
        public ServiceResultStatus Status { get; set; }
        public object? Data { get; set; }
        public string? Code { get; set; }
        public int? ErrorCode { get; set; }
        public ServiceResult(ServiceResultStatus status, object Data, string Code,int? errorCode) 
        {
            this.Status = status;
            this.Data = Data;
            this.Code = Code;
            this.ErrorCode = errorCode;
        }

        
    }
}
