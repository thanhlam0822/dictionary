using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using pvo_dictionary.Data;
using pvo_dictionary.DTO;
using pvo_dictionary.Model;
using pvo_dictionary.Repository;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace pvo_dictionary.Service
{
    public class ConceptService : ConceptRepository
    {
        private readonly DataContext dataContext;
        private readonly IHttpContextAccessor httpContext;

        public ConceptService(DataContext dataContext, IHttpContextAccessor httpContext)
        {
            this.dataContext = dataContext;
            this.httpContext = httpContext;
        }

        public ServiceResult GetNumberRecord(Guid? dictionaryId)
        {
            ServiceResult serviceResult = null;
            if (dictionaryId == Guid.Empty || string.IsNullOrWhiteSpace(dictionaryId.ToString()))
            {
                Console.WriteLine("Nhay vao  null");
                string currentDictionaryId = httpContext.HttpContext.Session.GetString("CurrentDictionaryID");
                List<Concept> concepts = dataContext.concept.Where(c => c.dictionary_id == new Guid(currentDictionaryId)).ToList();
                List<Example> examples = dataContext.example.Where(e => e.dictionary_id == new Guid(currentDictionaryId)).ToList();
                NumberRecordResponseDTO responseDTO = new NumberRecordResponseDTO();
                responseDTO.NumberConcept = concepts.Count;
                responseDTO.NumberExample = examples.Count;
                serviceResult = new ServiceResult(ServiceResultStatus.Success, responseDTO, null, null);

            }
            else
            {
                Console.WriteLine("Nhay vao ko null");
                List<Concept> concepts = dataContext.concept.Where(c => c.dictionary_id == dictionaryId).ToList();
                List<Example> examples = dataContext.example.Where(e => e.dictionary_id == dictionaryId).ToList();
                NumberRecordResponseDTO responseDTO = new NumberRecordResponseDTO();
                responseDTO.NumberConcept = concepts.Count;
                responseDTO.NumberExample = examples.Count;
                serviceResult = new ServiceResult(ServiceResultStatus.Success, responseDTO, null, null);

            }
            return serviceResult;

        }
        public ServiceResult GetListConept(Guid? dictionaryId)
        {
            ServiceResult serviceResult = null;
            if (dictionaryId == Guid.Empty || string.IsNullOrWhiteSpace(dictionaryId.ToString()))
            {
                string currentDictionaryId = httpContext.HttpContext.Session.GetString("CurrentDictionaryID");
                List<Concept> concepts = dataContext.concept.Where(c => c.dictionary_id == new Guid(currentDictionaryId)).ToList();
                ListConceptResponseDTO responseDTO = new ListConceptResponseDTO();
                responseDTO.ListConcept = concepts;
                responseDTO.LastUpdatedAt = DateTime.Now;
                serviceResult = new ServiceResult(ServiceResultStatus.Success, responseDTO, null, null);

            }
            else
            {
                List<Concept> concepts = dataContext.concept.Where(c => c.dictionary_id == dictionaryId).ToList();
                ListConceptResponseDTO responseDTO = new ListConceptResponseDTO();
                responseDTO.ListConcept = concepts;
                responseDTO.LastUpdatedAt = DateTime.Now;
                serviceResult = new ServiceResult(ServiceResultStatus.Success, responseDTO, null, null);

            }
            return serviceResult;
        }

        public ServiceResult AddConcept(SaveConceptRequestDTO request)
        {

            Concept oldConcept = dataContext.concept.FirstOrDefault(c => c.title == request.title);
            ServiceResult serviceResult = null;
            if (oldConcept == null)
            {
                Concept newConcept = new Concept();
                newConcept.title = request.title;
                newConcept.description = request.description;
                if (request.dictionaryId.IsNullOrEmpty() || string.IsNullOrWhiteSpace(request.dictionaryId.ToString()))
                {
                    string currentDictionaryId = httpContext.HttpContext.Session.GetString("CurrentDictionaryID");
                    newConcept.dictionary_id = new Guid(currentDictionaryId);
                    Console.WriteLine("request nulll");

                }
                else
                {
                    newConcept.dictionary_id = new Guid(request.dictionaryId);
                }
                dataContext.concept.Add(newConcept);
                dataContext.SaveChanges();
                serviceResult = new ServiceResult(ServiceResultStatus.Success, null, null, null);
            }
            else
            {
                serviceResult = new ServiceResult(ServiceResultStatus.Fail, null, "Concept already exists", 3001);
            }
            return serviceResult;
        }

        public ServiceResult UpdateConcept(UpdateConceptRequestDTO request)
        {
            Concept updateConcept = dataContext.concept.FirstOrDefault(c => c.concept_id == request.conceptId);
            Concept oldConcept = dataContext.concept.FirstOrDefault(c => c.title == request.title);
            ServiceResult serviceResult = null;
            if (oldConcept == null)
            {
                updateConcept.title = request.title;
                updateConcept.description = request.description;
                dataContext.concept.Update(updateConcept);
                dataContext.SaveChanges();
                serviceResult = new ServiceResult(ServiceResultStatus.Success, null, null, null);
            }
            else
            {
                serviceResult = new ServiceResult(ServiceResultStatus.Fail, null, "Concept already exists", 3001);
            }
            return serviceResult;

        }

        public ServiceResult DeleteConcept(DeleteConceptRequestDTO request)
        {

            ServiceResult serviceResult = null;
            if (request.isForce == false)
            {
                List<ExampleRelationship> relationshipList = dataContext.example_relationship.
                    Where(e => e.concept_id == request.conceptId).ToList();
                if (relationshipList.IsNullOrEmpty())
                {
                    Concept concept = dataContext.concept.FirstOrDefault(c => c.concept_id == request.conceptId);
                    dataContext.concept.Remove(concept);
                    dataContext.SaveChanges();
                    serviceResult = new ServiceResult(ServiceResultStatus.Success, null, null, null);
                }
                else
                {
                    serviceResult = new ServiceResult(ServiceResultStatus.Fail, null, "Concept can't be deleted", 3002);
                }
            }
            else
            {
                List<ExampleRelationship> relationshipList = dataContext.example_relationship.
                    Where(e => e.concept_id == request.conceptId).ToList();
                if (relationshipList.Count > 0)
                {
                    dataContext.example_relationship.RemoveRange(relationshipList);
                }
                Concept concept = dataContext.concept.FirstOrDefault(c => c.concept_id == request.conceptId);
                dataContext.concept.Remove(concept);
                dataContext.Entry(concept).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dataContext.SaveChanges();
                serviceResult = new ServiceResult(ServiceResultStatus.Success, null, null, null);
            }
            return serviceResult;
        }

        public ServiceResult GetConcept(Guid conceptId)
        {
            GetConceptResponseDTO responseDTO = new GetConceptResponseDTO();
            var query = from concept in dataContext.concept where concept.concept_id == conceptId select new { concept };
            var query2 = from example_relationship in dataContext.example_relationship
                         join example in dataContext.example on example_relationship.example_id equals example.example_id
                         where example_relationship.concept_id == conceptId
                         select new { example };
            foreach (var data in query)
            {
                responseDTO.Title = data.concept.title;
                responseDTO.Description = data.concept.description;

            }
            responseDTO.Examples = query2.AsList();

            return new ServiceResult(ServiceResultStatus.Success, responseDTO, null, null);
        }

        public ServiceResult SearchConcept(string searchKey)
        {
            string currentDictionaryId = httpContext.HttpContext.Session.GetString("CurrentDictionaryID");
            var userEmail = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            var user = dataContext.user.FirstOrDefault(u => u.email == userEmail);
            searchKey.Trim();
            List<Concept> concepts = dataContext.concept
                .Where(c => c.dictionary_id == new Guid(currentDictionaryId) && c.title.Contains(searchKey) ||
                c.dictionary_id == new Guid(currentDictionaryId) && c.description.Contains(searchKey)).ToList();
            ConceptSearchHistory history = new ConceptSearchHistory();
            history.user_id = user.user_id;
            history.dictionary_id = new Guid(currentDictionaryId);
            history.list_concept_search = JsonConvert.SerializeObject(concepts);
            dataContext.concept_search_history.Add(history);
            dataContext.SaveChanges();
            return new ServiceResult(ServiceResultStatus.Success, concepts, null, null);
        }
        public ServiceResult GetConceptRelationship(Guid conceptId, Guid parentId)
        {
            var query = from concept_link in dataContext.concept_link
                        join concept_relationship in dataContext.concept_relationship
                        on concept_link.concept_link_id equals concept_relationship.concept_link_id
                        where
                        concept_relationship.concept_id == conceptId && concept_relationship.parent_id == parentId
                        select new
                        { concept_link };

            GetConceptRelationshipDTO dto = new GetConceptRelationshipDTO();
            if (query.IsNullOrEmpty())
            {
                dto.ConceptLinkId = Guid.Empty;
                dto.ConceptLinkName = "No link";
            }
            else
            {
                foreach (var data in query)
                {

                    dto.ConceptLinkId = data.concept_link.concept_link_id;
                    dto.ConceptLinkName = data.concept_link.concept_link_name;


                }
            }

            return new ServiceResult(ServiceResultStatus.Success, dto, null, null);
        }

        public ServiceResult GetSaveSearch()
        {
            string currentDictionaryId = httpContext.HttpContext.Session.GetString("CurrentDictionaryID");
            var userEmail = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            var user = dataContext.user.FirstOrDefault(u => u.email == userEmail);
            List<ConceptSearchHistory> histories = dataContext.concept_search_history.Where(c => c.user_id == user.user_id && c.dictionary_id == new Guid(currentDictionaryId)).ToList();
            List<string> conceptHistories = new List<string>();
            foreach (var history in histories)
            {
                conceptHistories.Add(history.list_concept_search);

                
            }
            return new ServiceResult(ServiceResultStatus.Success, conceptHistories, null, null);
        }

        public ServiceResult GetTree(Guid conceptId, string conceptName)
        {
            Concept concept = dataContext.concept.FirstOrDefault(c => c.concept_id == conceptId || c.title.Contains(conceptName));
            if(concept != null)
            {
                List<Guid> parentIds = new List<Guid>();
                List<ParentConceptResponseDTO> responseDTOs = new List<ParentConceptResponseDTO>();
                List<ConceptRelationship> conceptRelationship = dataContext.concept_relationship.Where(c => c.concept_id == conceptId).ToList();
                foreach(var data in conceptRelationship)
                {
                    parentIds.Add(data.parent_id);
                }
                foreach(var id in parentIds)
                {
                    var query = from concept_link in dataContext.concept_link
                                join
                                concept_relationship in dataContext.concept_relationship
                                on concept_link.concept_link_id equals concept_relationship.concept_link_id
                                join conceptTable in dataContext.concept on concept_relationship.parent_id equals conceptTable.concept_id
                                where concept_relationship.parent_id == id
                            select new { concept_link, concept_relationship, conceptTable };
                    foreach (var data in query)
                    {
                        ParentConceptResponseDTO responseDTO = new ParentConceptResponseDTO();
                        responseDTO.ConceptId = data.conceptTable.concept_id;
                        responseDTO.Title = data.conceptTable.title;
                        responseDTO.ConceptLinkId = data.concept_link.concept_link_id;
                        responseDTO.ConceptLinkName = data.concept_link.concept_link_name;
                        responseDTO.SortOrder = data.concept_link.sort_order;
                        responseDTOs.Add(responseDTO);

                    }
                }
                List<ParentConceptResponseDTO> responseDTOs2 = new List<ParentConceptResponseDTO>();
                var query2 = from concept_link in dataContext.concept_link
                            join
                            concept_relationship in dataContext.concept_relationship
                            on concept_link.concept_link_id equals concept_relationship.concept_link_id
                            join conceptTable in dataContext.concept on concept_relationship.parent_id equals conceptTable.concept_id
                            where concept_relationship.parent_id == conceptId
                            select new { concept_link, concept_relationship, conceptTable };
                foreach (var data in query2)
                {
                    ParentConceptResponseDTO responseDTO = new ParentConceptResponseDTO();
                    responseDTO.ConceptId = data.conceptTable.concept_id;
                    responseDTO.Title = data.conceptTable.title;
                    responseDTO.ConceptLinkId = data.concept_link.concept_link_id;
                    responseDTO.ConceptLinkName = data.concept_link.concept_link_name;
                    responseDTO.SortOrder = data.concept_link.sort_order;
                    responseDTOs2.Add(responseDTO);

                }
               
                GetTreeResponseDTO treeResponseDTO = new GetTreeResponseDTO();
                treeResponseDTO.ListParent = responseDTOs;
                treeResponseDTO.ChildParent = responseDTOs2;
                return new ServiceResult(ServiceResultStatus.Success, treeResponseDTO, null, null);
            }
            else
            {
                return new ServiceResult(ServiceResultStatus.Fail, null, null, null);
            }
        }

        public ServiceResult GetConceptParents(Guid conceptId)
        {
            List<Guid> parentIds = new List<Guid>();
            List<ParentConceptResponseDTO> responseDTOs = new List<ParentConceptResponseDTO>();
            List<ConceptRelationship> conceptRelationship = dataContext.concept_relationship.Where(c => c.concept_id == conceptId).ToList();
            foreach (var data in conceptRelationship)
            {
                parentIds.Add(data.parent_id);
            }
            foreach (var id in parentIds)
            {
                var query = from concept_link in dataContext.concept_link
                            join
                            concept_relationship in dataContext.concept_relationship
                            on concept_link.concept_link_id equals concept_relationship.concept_link_id
                            join conceptTable in dataContext.concept on concept_relationship.parent_id equals conceptTable.concept_id
                            where concept_relationship.parent_id == id
                            select new { concept_link, concept_relationship, conceptTable };
                foreach (var data in query)
                {
                    ParentConceptResponseDTO responseDTO = new ParentConceptResponseDTO();
                    responseDTO.ConceptId = data.conceptTable.concept_id;
                    responseDTO.Title = data.conceptTable.title;
                    responseDTO.ConceptLinkId = data.concept_link.concept_link_id;
                    responseDTO.ConceptLinkName = data.concept_link.concept_link_name;
                    responseDTO.SortOrder = data.concept_link.sort_order;
                    responseDTOs.Add(responseDTO);

                }
            }
            return new ServiceResult(ServiceResultStatus.Success, responseDTOs, null, null);
        }

        public ServiceResult GetConceptChildren(Guid conceptId)
        {
            List<ParentConceptResponseDTO> responseDTOs2 = new List<ParentConceptResponseDTO>();
            var query2 = from concept_link in dataContext.concept_link
                         join
                         concept_relationship in dataContext.concept_relationship
                         on concept_link.concept_link_id equals concept_relationship.concept_link_id
                         join conceptTable in dataContext.concept on concept_relationship.parent_id equals conceptTable.concept_id
                         where concept_relationship.parent_id == conceptId
                         select new { concept_link, concept_relationship, conceptTable };
            foreach (var data in query2)
            {
                ParentConceptResponseDTO responseDTO = new ParentConceptResponseDTO();
                responseDTO.ConceptId = data.conceptTable.concept_id;
                responseDTO.Title = data.conceptTable.title;
                responseDTO.ConceptLinkId = data.concept_link.concept_link_id;
                responseDTO.ConceptLinkName = data.concept_link.concept_link_name;
                responseDTO.SortOrder = data.concept_link.sort_order;
                responseDTOs2.Add(responseDTO);

            }
            return new ServiceResult(ServiceResultStatus.Success, responseDTOs2, null, null);
        }
    }
}
