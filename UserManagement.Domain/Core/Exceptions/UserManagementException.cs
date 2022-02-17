using System;

namespace UserManagement.Domain.Core.Exceptions
{
    public class UserManagementException : Exception
    {
        public UserManagementException() { }

        public UserManagementException(string message)
            : base(message)
        {
            BusinessMessage = message;
        }

        public UserManagementException(string message, Exception inner)
            : base(message, inner)
        {
            BusinessMessage = message;
            /*
            var cookie = HttpContext.Current.Request.Cookies[ConfigurationResources.BaseCulture];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value)) return;
            var cultureName = cookie.Value;
            Language.Culture = new System.Globalization.CultureInfo(cultureName);
            */
        }
        public string BusinessMessage { get; set; }
    }
}
