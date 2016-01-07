using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Web.MVC
{
    public class StreamResponseInfo
    {

        /// <summary>
        /// Ruta completa y nombre del archivo
        /// </summary>
        public String FilePath { get; set; }
        /// <summary>
        /// Nombre del archivo tal como se descargará
        /// </summary>
        public String AttachmentName { get; set; }

        public String ContentType { get; set; }

        public byte[] Bytes { get; set; }

        public bool IsAttachment { get; set; }

        public StreamResponseInfo() { }

        public static StreamResponseInfo Create(string filePath, string attachmentName, bool isAttachment)
        {
            return new StreamResponseInfo()
            {
                AttachmentName = attachmentName,
                FilePath = filePath,
                Bytes = null,
                IsAttachment =  isAttachment
            };
        }


        public static StreamResponseInfo Create(string filePath, string attachmentName)
        {
            return Create(filePath, attachmentName, true);
        }

        public static StreamResponseInfo Create(byte[] bytes, string attachmentName, bool isAttachment)
        {
            return new StreamResponseInfo()
            {
                AttachmentName = attachmentName,
                FilePath = String.Empty,
                Bytes = bytes,
                IsAttachment = isAttachment
            };
        }

        public static StreamResponseInfo Create(byte[] bytes, string attachmentName)
        {
            return Create(bytes, attachmentName, true);
        }

    }
}
