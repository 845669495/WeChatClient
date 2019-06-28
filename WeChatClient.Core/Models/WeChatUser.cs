using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;
using WeChatClient.Core.Interfaces;

namespace WeChatClient.Core.Models
{
    public class WeChatUser : BaseNotifyModel, INeedDownloadImageModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 头像URL
        /// </summary>
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 备注名
        /// </summary>
        public string RemarkName { get; set; }
        /// <summary>
        /// 性别，0-未设置（公众号、保密），1-男，2-女
        /// </summary>
        public int Sex { get; set; }
        public bool IsMan => Sex == 1;
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        public string ProvinceAndCity => $"{Province} {City}".Trim();
        /// <summary>
        /// 昵称全拼
        /// </summary>
        public string PyQuanPin { get; set; }
        /// <summary>
        /// 备注名全拼
        /// </summary>
        public string RemarkPYQuanPin { get; set; }
        /// <summary>
        /// 联系人标识
        /// </summary>
        public int ContactFlag { get; set; }
        /// <summary>
        /// 0：是公众号或者是群聊 其他值是好友   //也有特殊情况，还是不行
        /// </summary>
        public string SnsFlag { get; set; }
        /// <summary>
        /// 公众号是gh_
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string ShowName
        {
            get
            {
                return string.IsNullOrEmpty(RemarkName) ? NickName : RemarkName;
            }
        }
        /// <summary>
        /// 显示的拼音全拼
        /// </summary>
        public string ShowPinYin
        {
            get
            {
                return string.IsNullOrEmpty(RemarkPYQuanPin) ? PyQuanPin : RemarkPYQuanPin;
            }
        }

        /// <summary>
        /// 分组的头
        /// </summary>
        public string StartChar { get; set; }

        /// <summary>
        /// 搜索时的类型分组描述
        /// </summary>
        public string SearchGroupDesc => this.IsRoomContact() ? "群组" : "好友";

        private int _unReadCount;
        /// <summary>
        /// 未读条数
        /// </summary>
        public int UnReadCount
        {
            get { return _unReadCount; }
            set
            {
                _unReadCount = value;
                OnPropertyChanged();
            }
        }

        private string _lastShortTime;
        /// <summary>
        /// 最后消息时间
        /// </summary>
        public string LastShortTime
        {
            get { return _lastShortTime; }
            set
            {
                _lastShortTime = value;
                OnPropertyChanged();
            }
        }

        private string _lastMessage;
        /// <summary>
        /// 最后的消息
        /// </summary>
        public string LastMessage
        {
            get { return _lastMessage; }
            set
            {
                _lastMessage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 如果是群聊且值为1，则代表消息免打扰
        /// </summary>
        public int Statues { get; set; }

        /// <summary>
        /// 消息免打扰
        /// </summary>
        public bool ChatNotifyClose { get; set; }

        /// <summary>
        /// 群聊成员人数（群聊大于0）
        /// </summary>
        public int MemberCount { get; set; }
        /// <summary>
        /// 群聊成员对象数组
        /// </summary>
        public List<ChatRoomMember> MemberList { get; set; }

        /// <summary>
        /// 内容区域显示的名称
        /// </summary>
        public string ContentShowName
        {
            get
            {
                string name = ShowName;
                if (MemberCount > 0)
                    name += $"({MemberCount})";
                return name;
            }
        }

        public string Uri => HeadImgUrl;

        private ImageSource _image;
        public ImageSource Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 消息列表
        /// </summary>
        public ObservableCollection<WeChatMessage> MessageList { get; private set; } = new ObservableCollection<WeChatMessage>();
    }

    public class WeChatUserComparer : IEqualityComparer<WeChatUser>
    {
        public bool Equals(WeChatUser x, WeChatUser y)
        {
            return x.UserName == y.UserName;
        }

        public int GetHashCode(WeChatUser obj)
        {
            return obj.UserName.GetHashCode();
        }
    }
}
