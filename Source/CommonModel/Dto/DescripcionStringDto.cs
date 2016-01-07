using System;

namespace Nuaguil.CommonModel.Dto
{
    [Serializable]
    public class DescripcionStringDto
    {

        #region Properties
        
        public String Id { get; set; }
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
