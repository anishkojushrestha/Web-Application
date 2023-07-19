namespace Web_Application.Models
{
    public class SessionHandle
    {
        HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
        ISession session => httpContextAccessor.HttpContext!.Session;
        //public void Session(params string[] values)
        public void Session(string userId=null, string userName=null , string userFirstName=null, string userLastName=null, string userProfile=null, string userEmail=null, string companyName=null, string password=null, string address=null, string validFrom=null, string validTo=null, string companyEmail=null , string registrationDate=null )
        {

            if (!string.IsNullOrEmpty(userId))
            {
                session.SetString("userId", userId.ToString());
            }
            if (!string.IsNullOrEmpty(userName))
            {
                session.SetString("userName", userName);
            }
            if (!string.IsNullOrEmpty(userFirstName))
            {
                session.SetString("userFirstName", userFirstName);

            }
            if (!string.IsNullOrEmpty(userLastName))
            {
                session.SetString("userLastName",userLastName);
            }
            if (!string.IsNullOrEmpty(userProfile))
            {
                session.SetString("userProfile", userProfile);

            }
            if (!string.IsNullOrEmpty(userEmail))
            {
                session.SetString("userEmail", userEmail);

            }
            if (!string.IsNullOrEmpty(companyName))
            {
                session.SetString("companyName", companyName);

            }
            if (!string.IsNullOrEmpty(password))
            {
                session.SetString("password", password);

            }
            if (!string.IsNullOrEmpty(address))
            {
                session.SetString("address", address);

            }
            if (!string.IsNullOrEmpty(validFrom))
            {
                session.SetString("validFrom", validFrom);

            }
            if (!string.IsNullOrEmpty(validTo))
            {
                session.SetString("validTo", validTo);

            }
            if (!string.IsNullOrEmpty(companyEmail))
            {
                session.SetString("companyEmail", companyEmail);

            }
            if (!string.IsNullOrEmpty(registrationDate))
            {
                session.SetString("registrationDate", registrationDate);

            }
        }
    }
}
