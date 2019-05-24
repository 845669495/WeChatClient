﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WeChatClient.Core.Interfaces;

namespace WeChatClient.Core.Models
{
    public class WeChatUser: INeedDownloadImageModel, INotifyPropertyChanged
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
        public string Sex { get; set; }
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
        /// 未读条数
        /// </summary>
        public int UnReadCount { get; set; }

        /// <summary>
        /// 最后消息时间
        /// </summary>
        public string LastShortTime { get; set; }
        
        /// <summary>
        /// 最后的消息
        /// </summary>
        public string LastMessage { get; set; }

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

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
