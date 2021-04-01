using CMS.DAL;
using CMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BL
{
    public class CmsBL
    {
        public static bool Login(ODZUser oDZUser)
        {
            return CmsDAL.Login( oDZUser);
        }

       
        public static bool AddCase(Case issue)
        {
            return CmsDAL.AddCase(issue);
        }

        public static bool LDZLogin(LDZUser lDZUser)
        {
            return CmsDAL.LDZUser(lDZUser);
        }
    }
}
