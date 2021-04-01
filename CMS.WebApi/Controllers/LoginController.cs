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
    public class LoginController : ApiController
    {
        public IHttpActionResult Post([FromBody] ODZUser oDZUser)
        {
            try
            {
                bool isLogged = CmsBL.Login(oDZUser);
                if (isLogged)
                {
                    return Created("user", oDZUser);
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
