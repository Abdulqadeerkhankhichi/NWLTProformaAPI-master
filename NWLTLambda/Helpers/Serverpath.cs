using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace NWLTLambda.Helpers
{
    public class Serverpath
    {

       
            private readonly IHostingEnvironment _hostingEnvironment;

            public Serverpath(IHostingEnvironment hostingEnvironment)
            {
                _hostingEnvironment = hostingEnvironment;
            }

            public string path()
            {
                // application's base path
                string contentRootPath = _hostingEnvironment.ContentRootPath;

                // application's publishing path
                string webRootPath = _hostingEnvironment.WebRootPath;

            return webRootPath;
            }
        }
    }

