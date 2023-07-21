using Microsoft.AspNetCore.Mvc;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;

namespace pvo_dictionary.Repository
{
    public interface DictionaryRepository
    {
        
        ServiceResult GetAllDictionaryByUserId();
        ServiceResult LoadDictionary(Guid id);
        ServiceResult AddDictionary(AddDictonaryRequestDTO requestDTO);
        ServiceResult UpdateDictionay(UpdateDIctionaryRequestDTO requestDTO);
        ServiceResult DeleteDictionay(Guid id);
        ServiceResult TransferDictionary(TransferDictionaryRequestDTO requestDTO);
        //IActionResult DownloadFile(int fileType);
    }
}
