using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatClient.Core.Http
{
    public class BaseRequest
    {
        public string DeviceID { get; set; }
        public string Sid { get; set; }
        public string Skey { get; set; }
        public int Uin { get; set; }
    }
}
