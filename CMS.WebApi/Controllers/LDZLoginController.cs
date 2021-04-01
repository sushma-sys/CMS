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
    public class LDZLoginController : ApiController
    {
        public IHttpActionResult Post([FromBody] LDZUser lDZUser)
        {
            try
            {
                bool isLogged = CmsBL.LDZLogin(lDZUser);
                if (isLogged)
                {
                    return Created("user", lDZUser);
                }
                else
                {
                    return StatusCode(HttpStatusCode.InternalServerError);
                }
            }
            catch (CmsExceptions ex)
            {

                throw ex;
            }
        }
    }
}
