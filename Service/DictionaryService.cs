using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using pvo_dictionary.Data;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;
using System.Security.Claims;

namespace pvo_dictionary.Service
{
    public class DictionaryService : DictionaryRepository
    {
        private readonly DataContext dataContext;
        private readonly IHttpContextAccessor httpContext;

        public DictionaryService(DataContext dataContext, IHttpContextAccessor httpContext)
        {
            this.dataContext = dataContext;
            this.httpContext = httpContext;
        }

        public ServiceResult AddDictionary(AddDictonaryRequestDTO requestDTO)
        {
            var userEmail = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            var user = dataContext.user.FirstOrDefault(u => u.email == userEmail);
            requestDTO.dictionaryName.Trim();
            Dictionary dictionaryExist = dataContext.dictionary.FirstOrDefault(d => d.dictionary_name.ToLower() == requestDTO.dictionaryName.ToLower());
            if (dictionaryExist != null)
            {
                return new ServiceResult(ServiceResultStatus.Fail, null, "Dictionary name already in use", 2001);
            }
            else
            {
                if(requestDTO.cloneDictionaryId == null) 
                {
                    Dictionary dictionary = new Dictionary();
                    dictionary.dictionary_id = new Guid();
                    dictionary.dictionary_name = requestDTO.dictionaryName;
                    dictionary.user_id = user.user_id;
                    dictionary.created_at = DateTime.Now;
                    dataContext.dictionary.Add(dictionary);
                    dataContext.SaveChanges();
                    return new ServiceResult(ServiceResultStatus.Success, null, null, null);
                }
                else
                {
                    Dictionary mainDictionary = dataContext.dictionary.FirstOrDefault(d => d.dictionary_id == requestDTO.cloneDictionaryId);
                    Dictionary dictionary = new Dictionary();
                    dictionary.dictionary_id = new Guid();
                    dictionary.dictionary_name = requestDTO.dictionaryName;
                    dictionary.user_id = mainDictionary.user_id;
                    dictionary.created_at = mainDictionary.created_at;
                    dataContext.dictionary.Add(dictionary);
                    dataContext.SaveChanges();
                    return new ServiceResult(ServiceResultStatus.Success, null, null, null);
                }
            }
            
        }

        public ServiceResult DeleteDictionay(Guid id)
        {
            string currentDictionaryId = httpContext.HttpContext.Session.GetString("CurrentDictionaryID");
            Console.WriteLine(currentDictionaryId);
            if (id.ToString().Equals(currentDictionaryId))
            {
                return new ServiceResult(ServiceResultStatus.Fail, null,  "Dictionary is in use",2002);
            }
            else
            {
                Dictionary dictionary = dataContext.dictionary.FirstOrDefault(d => d.dictionary_id == id);
                dataContext.dictionary.Remove(dictionary);
                dataContext.SaveChanges();
                return new ServiceResult(ServiceResultStatus.Success, null, null, null);
            }
            
        }

        public ServiceResult GetAllDictionaryByUserId()
        {
            var userEmail = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            var user = dataContext.user.FirstOrDefault(u => u.email == userEmail);
            List<Dictionary> dictionaries = dataContext.dictionary.Where(d => d.user_id == user.user_id).ToList();
            return new ServiceResult(ServiceResultStatus.Success, dictionaries, null, null);
        }

        public ServiceResult LoadDictionary(Guid id)
        {
            var dictionary = dataContext.dictionary.FirstOrDefault(d => d.dictionary_id == id);
            if (dictionary != null)
            {
               httpContext.HttpContext.Session.SetString("CurrentDictionaryID", dictionary.dictionary_id.ToString());
               
               dictionary.last_view_at = DateTime.Now;
               dataContext.SaveChanges();
               var responseDTO = new DictionaryResponseDTO(dictionary.dictionary_id, dictionary.dictionary_name, dictionary.last_view_at);
               return new ServiceResult(ServiceResultStatus.Success, responseDTO, null, null);
            }
            else
            {
                return new ServiceResult(ServiceResultStatus.Fail, null, "Dictionary doesn’t exist", 2000);
            }
        }
        public ServiceResult UpdateDictionay(UpdateDIctionaryRequestDTO requestDTO)
        {
            requestDTO.DictionaryName.Trim();
            Dictionary oldDictinary = dataContext.dictionary.FirstOrDefault(d => d.dictionary_id == requestDTO.DictionaryId);
            if(oldDictinary == null)
            {
                return new ServiceResult(ServiceResultStatus.Fail, null, "Dictionary doesn’t exist", 2000);
            }
            else
            {
                Dictionary dictionary = dataContext.dictionary.FirstOrDefault(d => d.dictionary_name.ToLower() == requestDTO.DictionaryName.ToLower());
                if(dictionary == null)
                {
                    oldDictinary.dictionary_name = requestDTO.DictionaryName;
                    dataContext.Update(oldDictinary);
                    dataContext.SaveChanges();
                    return new ServiceResult(ServiceResultStatus.Success, null, null, null);
                }
                else
                {
                    return new ServiceResult(ServiceResultStatus.Fail, null, "Dictionary name already in use", 2001);
                }
            }
        }
        public ServiceResult TransferDictionary(TransferDictionaryRequestDTO requestDTO)
        {
              List<Concept> concepts = dataContext.concept.Where(c => c.dictionary_id == requestDTO.sourceDictionaryId).ToList();
              
            if (concepts.IsNullOrEmpty())
              {
                  return new ServiceResult(ServiceResultStatus.Fail, null, "Source dictionary is empty", 2000);
              }
              else
              {
                 if(requestDTO.isDeleteDestData == false)
                 {
                    List<Concept> list = new List<Concept>();
                      foreach (Concept concept in concepts)
                      {
                          Concept newConcept = new Concept();
                          newConcept.concept_id = new Guid();
                          newConcept.dictionary_id = requestDTO.destDictionaryId;
                          newConcept.title = concept.title;
                          newConcept.description = concept.description;
                          newConcept.created_at = DateTime.Now;
                          list.Add(newConcept);
                      }
                    dataContext.concept.AddRange(list);
                    dataContext.SaveChanges();
                    return new ServiceResult(ServiceResultStatus.Success, null, null, null);
                }
                 else
                {
                    List<Concept> listConceptDest = dataContext.concept.Where(c => c.dictionary_id == requestDTO.destDictionaryId).ToList();
                    dataContext.concept.RemoveRange(listConceptDest);
                    dataContext.SaveChanges();
                    List<Concept> list = new List<Concept>();
                    foreach (Concept concept in concepts)
                    {
                        Concept newConcept = new Concept();
                        newConcept.concept_id = new Guid();
                        newConcept.dictionary_id = requestDTO.destDictionaryId;
                        newConcept.title = concept.title;
                        newConcept.description = concept.description;
                        newConcept.created_at = DateTime.Now;
                        list.Add(newConcept);
                    }
                    dataContext.concept.AddRange(list);
                    dataContext.SaveChanges();
                    return new ServiceResult(ServiceResultStatus.Success,null,null,null);
                    }
              }
            
        }
    }
}
