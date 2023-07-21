using pvo_dictionary.Model;

namespace pvo_dictionary.Repository
{
    public interface UserConfigRepository
    {
        ServiceResult GetListExampleLink();
        ServiceResult GetListExampleAttribute();
        ServiceResult DeleteExample(Guid exampleId);    
    }
}
