using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core.Models;

namespace WeChatClient.Core.Events
{
    public class ChatWithContactEvent: PubSubEvent<WeChatUser>
    {
    }
}
