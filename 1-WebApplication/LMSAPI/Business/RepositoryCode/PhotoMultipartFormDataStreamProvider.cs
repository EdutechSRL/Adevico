using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Adevico.WebAPI.Repository
{
    public class PhotoMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
    
        public PhotoMultipartFormDataStreamProvider(string path) : base(path)    
        {
        }
 
        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            string extension = "";
            string nameFile = headers.ContentDisposition.FileName;

            if (!string.IsNullOrEmpty(nameFile))
            {
                nameFile = nameFile.Replace("\"","");
                int lastPoint = nameFile.LastIndexOf(".");
                if (!string.IsNullOrEmpty(nameFile) && lastPoint > 0)
                    extension = nameFile.Substring(lastPoint, nameFile.Length - lastPoint);
            }
          
            return Guid.NewGuid().ToString() + extension;
        }
    }
}