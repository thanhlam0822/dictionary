namespace pvo_dictionary.DTO
{
    public class GetTreeResponseDTO
    {
        public List<ParentConceptResponseDTO> ListParent { get; set; }
        public List<ParentConceptResponseDTO> ChildParent { get; set; }
    }
}
