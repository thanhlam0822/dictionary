using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using pvo_dictionary.Data;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;
using OfficeOpenXml;
namespace pvo_dictionary.Controllers
{
    [Route("api/dictionary")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly DictionaryRepository dictionaryRepository;
        private readonly string _uploadFolderPath = "C:/Users/PV/source/repos/pvo-dictionary/pvo-dictionary/template";

        public DictionaryController(DictionaryRepository dictionaryRepository)
        {
            this.dictionaryRepository = dictionaryRepository;

        }

        [HttpGet("get_list_dictionary"), Authorize]
        public ServiceResult GetAllDictionaryByUserId()
        {
            return dictionaryRepository.GetAllDictionaryByUserId();
        }

        [HttpGet("load_dictionary")]
        public ServiceResult LoadDictionary(Guid id)
        {
            return dictionaryRepository.LoadDictionary(id);
        }

        [HttpPost("add_dictionary"), Authorize]
        public ServiceResult AddDictionary([FromBody] AddDictonaryRequestDTO requestDTO)
        {
            return dictionaryRepository.AddDictionary(requestDTO);
        }

        [HttpPatch("update_dictionary")]
        public ServiceResult UpdateDictonary([FromBody] UpdateDIctionaryRequestDTO requestDTO)
        {
            return dictionaryRepository.UpdateDictionay(requestDTO);
        }

        [HttpDelete("delete_dictionary")]
        public ServiceResult DeleteDictonary(Guid id)
        {
            return dictionaryRepository.DeleteDictionay(id);
        }
        [HttpPost("transfer_dictionary")]
        public ServiceResult TransferDictionary([FromBody] TransferDictionaryRequestDTO requestDTO)
        {
            return dictionaryRepository.TransferDictionary(requestDTO);
        }
        [HttpGet("download_template_import_dictionary")]
        public IActionResult DownloadFile(int fileType)
        {
            try
            {
                string fileName;
                string fileExtension;

                if (fileType == 0)
                {
                    fileName = "template excel 2007.xlsx";
                    fileExtension = ".xlsx";
                }
                else if (fileType == 1)
                {
                    fileName = "template excel 2003.xls";
                    fileExtension = ".xls";
                }
                else
                {
                    return BadRequest("Invalid file type");
                }

                string filePath = Path.Combine(_uploadFolderPath, fileName);

                if (!System.IO.File.Exists(filePath))
                    return NotFound("File not found");

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Set the Content-Disposition header to prompt the user to download the file
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = fileName,
                    Inline = false
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());

                // Determine the appropriate content type based on the file extension
                string contentType;
                if (fileExtension == ".xlsx")
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                else if (fileExtension == ".xls")
                    contentType = "application/vnd.ms-excel";
                else
                    return BadRequest("Invalid file type");

                // Return the file as a FileStreamResult
                return new FileStreamResult(new MemoryStream(fileBytes), contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error downloading file: {ex.Message}");
            }
        }

        [HttpPost("test123")]
        public IActionResult ImportExcelFile(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length <= 0)
            {
                // Handle invalid file
                return BadRequest("Invalid file");
            }

            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                // Get all the worksheets in the Excel file
                var worksheets = package.Workbook.Worksheets;
                for(int i = 1; i <= 2; i++)
                {
                    var rowCount = worksheets[1].Dimension.Rows;
                    var colCount = worksheets[1].Dimension.Columns;
                }

                foreach (var worksheet in worksheets)
                {
                    
                    
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= colCount; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Value?.ToString();
                            
                            // Process the cell value as needed
                            // ...
                        }
                    }
                }
            }

            return Ok("File imported successfully");
        }
    }

}

