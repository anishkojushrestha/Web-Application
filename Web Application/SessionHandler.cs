using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Tls;
using System.Data.SqlClient;
using System.Drawing;

namespace Web_Application
{

    public  class SessionHandler
    {
       private IHttpContextAccessor accessor;
        //private ISession session=>accessor.HttpContext.Session;
        //public SessionHandler(IHttpContextAccessor _accessor)
        //{
        //    this.accessor = _accessor;
        //}


        public  string _userName
        {
            
            get
            {
                //session.GetString("_userName");
                if (accessor.HttpContext.Session.GetString("_userName") == null)
                    return string.Empty;
                else
                    return accessor.HttpContext.Session.GetString("_userName").ToString();
            }
            set
            {
                accessor.HttpContext.Session.SetString("_userName", value);
            }
        }
        public string _userId
        {
            get
            {
                if (accessor.HttpContext.Session.GetString("_userId") == null)
                    return string.Empty;
                else
                    return accessor.HttpContext.Session.GetString("_userId").ToString();
            }
            set
            {
                accessor.HttpContext.Session.SetString("_userId", value);
            }
        }




    }
}
