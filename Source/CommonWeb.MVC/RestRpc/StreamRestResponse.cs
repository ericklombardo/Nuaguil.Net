namespace Common.Web.MVC.RestRpc
{
    using System;
    using System.IO;

    internal class StreamRestResponse : Common.Web.MVC.RestRpc.RestResponse
    {

        private StreamResponseInfo result;

        public StreamRestResponse(Common.Web.MVC.RestRpc.RestRequest request)
            : base(request)
        {
        }

        protected  override void GenerateResponse(Stream output, object result)
        {
            this.result = (StreamResponseInfo)result;
            downloadFile();
        }

        private void downloadFile()
        {

            Stream iStream = null;

            //Buffer a leer en partes de  10K bytes
            byte[] buffer = new byte[10000];

            //Longitud del archivo
            int length;

            //Total de bytes a leer
            long dataToRead;

            try
            {
                if (result.Bytes != null)
                {
                    iStream = new MemoryStream(result.Bytes);
                }
                else
                {
                    //Abrir el archivo
                    string filePath = result.FilePath;
                    if (!File.Exists(filePath))
                        throw new Exception("El archivo " + filePath + " no existe ");

                    iStream = new FileStream(filePath, FileMode.Open,
                                             FileAccess.Read, FileShare.Read);
                }
                
                downloadStream(iStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (iStream != null)
                {
                    //Cerrar el archivo
                    iStream.Close();
                    //_context.Response.Close();
                }
            }

        }


        private void downloadStream(Stream iStream)
        {

            //Buffer a leer en partes de  10K bytes
            byte[] buffer = new byte[10000];

            //Longitud del archivo
            int length;

            //Total de bytes a leer
            long dataToRead;

            try
            {
                //Total de bytes a leer:
                dataToRead = iStream.Length;

                //string attach = result.IsAttachment ? "attachment; " : "";
                string attach = result.IsAttachment ? "inline; " : "";
                
                //Flush any pending response.
                _context.Response.Clear();
                //Set the HTTP headers for a PDF response.
                _context.Response.ClearContent();

                _context.Response.ClearHeaders();
                _context.Response.AppendHeader("Cache-Control", "public");
                _context.Response.AppendHeader("Content-Disposition", attach + "filename=" + _context.Server.UrlEncode(result.AttachmentName));
                _context.Response.AppendHeader("Content-Description", "File Transfer");
                _context.Response.ContentType = getFileMimeType(Path.GetExtension(result.AttachmentName).ToLower());

                /*
                if(result.IsAttachment)
                {
                    _context.Response.ClearHeaders();
                    _context.Response.AppendHeader("Cache-Control", "public");
                    _context.Response.AppendHeader("Content-Disposition", attach + "filename=" + _context.Server.UrlEncode(result.AttachmentName));
                    _context.Response.AppendHeader("Content-Description", "File Transfer");
                    _context.Response.ContentType = getFileMimeType(Path.GetExtension(result.AttachmentName).ToLower());
                }
                else
                {
                    _context.Response.ContentType = result.ContentType;
                    _context.Response.AppendHeader("Content-Disposition", attach + "filename=" + _context.Server.UrlEncode(result.AttachmentName));
                }
                */

                //Leer bytes
                while (dataToRead > 0)
                {
                    //Verificar que el cliente es conectado
                    if (_context.Response.IsClientConnected)
                    {
                        //Leer los datos del buffer
                        length = iStream.Read(buffer, 0, 10000);

                        //Escribir los datos en el output stream.
                        _context.Response.OutputStream.Write(buffer, 0, length);

                        //Enviar los datos al HTML output.
                        _context.Response.Flush();

                        buffer = null;
                        buffer = new byte[10000];//Limpiar el buffer
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        //Prevenir un loop infinito si el usuario se desconecta
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string getFileMimeType(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";

                case ".txt":
                    return "text/plain";


                case ".doc":
                case ".docx":
                    return "application/ms-word";

                case ".tiff":
                case ".tif":
                    return "image/tiff";

                case ".asf":
                    return "video/x-ms-asf";

                case ".avi":
                    return "video/avi";

                case ".zip":
                    return "application/zip";

                case ".xls":
                case ".xlsx":
                case ".csv":
                    return "application/vnd.ms-excel";

                case ".gif":
                    return "image/gif";

                case ".jpg":
                case "jpeg":
                    return "image/jpeg";

                case ".bmp":
                    return "image/bmp";

                case ".wav":
                    return "audio/wav";

                case ".mp3":
                    return "audio/mpeg3";

                case ".mpg":
                case "mpeg":
                    return "video/mpeg";

                case ".rtf":
                    return "application/rtf";

                case ".asp":
                    return "text/asp";

                case ".pdf":
                    return "application/pdf";

                case ".fdf":
                    return "application/vnd.fdf";

                case ".ppt":
                    return "application/mspowerpoint";

                case ".dwg":
                    return "image/vnd.dwg";

                case ".msg":
                    return "application/msoutlook";

                case ".xml":
                case ".sdxl":
                    return "application/xml";

                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";

                default:
                    return "application/x-unknown";
            }
        } 



    }
}
