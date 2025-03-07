


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFDataAccess.Helpers
{

    public class CustomResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Object Data { get; set; }

        public Object Error { get; set; }
    }

    public class LoginResponse
    {
        public Object Details { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiry { get; set; }
        public string UserType { get; set; }
        public int StatusCode { get; set; }
        //public List<ActionPermission> Permissions { get; set; }


    }


}
