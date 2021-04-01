using CMS.Entities;
using CMS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DAL
{
    public class CmsDAL
    {

        public static bool Login(ODZUser oDZUser)
        {
            try
            {
                using (CMSDBContext context = new CMSDBContext())
                {
                    var login = context.ODZUsers.Where(u => u.EmailId == oDZUser.EmailId && u.Password == oDZUser.Password).FirstOrDefault();
                    if (login == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                    //return login;
                }

            }
            catch (CmsExceptions ex)
            {

                throw ex;
            }
        }

        public static bool LDZUser(LDZUser lDZUser)
        {
            try
            {
                using (CMSDBContext context = new CMSDBContext())
                {
                    var login = context.LDZUsers.Where(u => u.EmailId == lDZUser.EmailId && u.Password == lDZUser.Password).FirstOrDefault();
                    if (login == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                    //return login;
                }

            }
            catch (CmsExceptions ex)
            {

                throw ex;
            }
        }

        public static bool AddCase(Case issue)
        {

            try
            {
                using (CMSDBContext context = new CMSDBContext())
                {
                    context.Cases.Add(issue);
                    int records = context.SaveChanges();
                    if (records > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (CmsExceptions ex)
            {

                throw ex;
            }
        }
        public static IEnumerable<Service> GetService()
        {
            try
            {
                using (CMSDBContext context = new CMSDBContext())
                {
                    return context.Services.ToList();
                }
            }
            catch (CmsExceptions ex)
            {

                throw ex;
            }
        }

            public static IEnumerable<Case> GetCase()
            {
                try
                {
                    using (CMSDBContext context = new CMSDBContext())
                    {
                        return context.Cases.ToList();
                    }
                }
                catch (CmsExceptions ex)
                {

                    throw ex;
                }
            }
        
    }
}

