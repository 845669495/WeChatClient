using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core.Models;

namespace WeChatClient.Core.Interfaces
{
    public interface IContactListManager
    {
        void AddContact(params WeChatUser[] contact);
        WeChatUser FindContact(string userName);
        void DelContact(params string[] userNames);
        void ModContact(params WeChatUser[] contact);
        WeChatUser[] SearchContact(string searchText);
    }
}
