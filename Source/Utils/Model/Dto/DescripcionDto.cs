using System;
using System.Runtime.Serialization;

namespace Nuaguil.Utils.Model.Dto
{
    [DataContract]
    public class DescripcionDto
    {

        #region Properties
        
        [DataMember]
        public Int64 Id { get; set; }
        [DataMember]
        public String Descripcion { get; set; }
        
        #endregion

        #region Constructors

        public DescripcionDto(long id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }

        public DescripcionDto(int id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;            
        }

        public DescripcionDto()
        {
        }

        #endregion
    }
}
