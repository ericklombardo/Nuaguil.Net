using System.Runtime.Serialization;

namespace Nuaguil.Utils.Model.Dto
{
    [DataContract]
    public class PageInfoDto
    {
        [DataMember]
        public int Start { get; set; }
        [DataMember]
        public int Limit { get; set; }
    }
}