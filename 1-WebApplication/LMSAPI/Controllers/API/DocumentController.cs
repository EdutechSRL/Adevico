using Adevico.WebAPI.Controllers.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Adevico.WebAPI.Controllers.API
{
    public class DocumentController : BaseApiController
    {
        // GET api/documenti
        public IEnumerable<dtoDocument> Get([FromUri]string token, [FromUri]int idComunita)
        {
            return new dtoDocument[]  
                { new dtoDocument(){ 
                    Link = "http://www.google.com/?type=pdf", 
                    Name = "listino prezzi", 
                    Estensione = "pdf", 
                    Descrizione = "Documento PDF ti prova"
                }
                , new dtoDocument(){ 
                    Link = "http://www.google.com/?type=doc", 
                    Name = "modulo da compilare", 
                    Estensione = "doc", 
                    Descrizione = "modulo da compilare e firmare per il corso"
                }
                , new dtoDocument(){ 
                    Link = "http://www.google.com/?type=xls", 
                    Name = "calendario del corso", 
                    Estensione = "xls", 
                    Descrizione = "tutte le date e gli eventi del corso"
                } };         
        }
    }
}

public class dtoDocument {
    public string Link { get; set; }
    public string Name { get; set; }
    public string Estensione { get; set; }
    public string Descrizione { get; set; }

    public dtoDocument(){}
}