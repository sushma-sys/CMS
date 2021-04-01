using CMS.BL;
using CMS.Entities;
using CMS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CMS.WebApi.Controllers
{
    public class CaseController : ApiController
    {
        public IHttpActionResult Post([FromBody] Case issue)
        {

            try
            {
                bool isCaseCreate = CmsBL.AddCase(issue);
                if (isCaseCreate)
                {
                    return Ok("Registered successfully");
                }
                else
                {
                    return StatusCode(HttpStatusCode.InternalServerError);
                }
            }
            catch (CmsExceptions ex)
            {

                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }
    }
}
