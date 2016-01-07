using System;
using System.Runtime.Serialization;

namespace Nuaguil.Utils.Model.Dto
{
    [DataContract]
    public class DescripcionStringDto
    {

        #region Properties
        [DataMember]
        public String Id { get; set; }
        [DataMember]
        public String Descripcion { get; set; }
        
        #endregion

        #region Constructors

        public DescripcionStringDto(string id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }

        public DescripcionStringDto()
        {
        }

        #endregion
    }
}
