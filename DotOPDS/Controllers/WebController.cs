﻿using DotOPDS.Utils;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace DotOPDS.Controllers
{
    public class WebController : ApiController
    {
        //[Route("{*filename}")]
        [HttpGet]
        public HttpResponseMessage ServeIndex(string filename)
        {
            var file = GetFile("index.html");
            if (file == null)
            {
                file = Request.CreateResponse(HttpStatusCode.Moved);
                file.Headers.Location = new Uri("/opds", UriKind.Relative);
            }
            return file;
        }

        [Route("static/{*filename}")]
        [HttpGet]
        public HttpResponseMessage ServeStatic(string filename)
        {
            return GetFile(filename) ?? new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        private HttpResponseMessage GetFile(string filename)
        {
            var file = Path.Combine(Util.Normalize(Settings.Instance.Web), filename);
            if (File.Exists(file))
            {
                var mimeType = MimeMapping.GetMimeMapping(filename);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(File.OpenRead(file));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                return response;
            }
            return null;
        }
    }
}
