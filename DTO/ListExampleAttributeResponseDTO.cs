using Microsoft.Win32;
using pvo_dictionary.Model;
using System.Security.Policy;


namespace pvo_dictionary.DTO
{
    public class ListExampleAttributeResponseDTO
    {
        public List<Tone> Tone { get; set; }
        public List<Mode> Mode{ get; set; }
        public List<Register> Register { get; set; }
        public List<Nuance> Nuance { get; set; }
        public List<Dialect> Dialect { get; set; }


    }
}
