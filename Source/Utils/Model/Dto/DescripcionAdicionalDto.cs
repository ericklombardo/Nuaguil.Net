#region FileInfo
// Archivo:  DescripcionAdicionalDto.cs
// Autor:    ELNA
// Objetivo: Define el Dto DescripcionAdicionalDto
#endregion

using System.Runtime.Serialization;

namespace Nuaguil.Utils.Model.Dto
{
   /// <summary>
   /// Vista comun de tres campos
   /// </summary>   
   [DataContract]
   public class DescripcionAdicionalDto
   {
      [DataMember]
      public long Id { get; set; }

      [DataMember]
      public string Descripcion { get; set; }

      [DataMember]
      public string Adicional { get; set; }

      public DescripcionAdicionalDto() { }

      public DescripcionAdicionalDto(int id, string descripcion, string adicional)
      {
         Id = id;
         Descripcion = descripcion;
         Adicional = adicional;
      }

      public DescripcionAdicionalDto(long id, string descripcion, string adicional)
      {
         Id = id;
         Descripcion = descripcion;
         Adicional = adicional;
      }

   }
}
