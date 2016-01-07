using System;

namespace Nuaguil.CommonModel.Dto
{
    [Serializable]
    public class DescripcionDto
    {

        #region Properties
        
        public Int64 Id { get; set; }
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
