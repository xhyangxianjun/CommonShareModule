using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTDDJYDS.CommonModule
{
    public class ServiceStackClientHelper
    {
        public void Client(string url)
        {
            Soap11ServiceClient client = new Soap11ServiceClient(url);

        }
    }
}
