using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WeChatClient.Core.Dependency;

namespace WeChatClient.EmojiCore.Emoji
{
    [ExposeServices(ServiceLifetime.Singleton)]
    public class EmojiManager
    {
        private Dictionary<string, string> EmojiMap { get; }
        private Dictionary<string, int> QQEmojiMap { get; }
        private List<EmojiModel> EmojiModelList { get; }
        private List<EmojiModel> QQEmojiModelList { get; }
        private Dictionary<string, string> EmojiCodeMap { get; }
        public EmojiManager()
        {
            string emojiMapJson = File.ReadAllText("Emoji/emoji_map.json");
            EmojiMap = JsonConvert.DeserializeObject<Dictionary<string,string>>(emojiMapJson);

            string qqEmojiMapJson = File.ReadAllText("Emoji/qq_emoji_map.json");
            QQEmojiMap = JsonConvert.DeserializeObject<Dictionary<string, int>>(qqEmojiMapJson);

            string emojiModelJson = File.ReadAllText("Emoji/emoji.json");
            EmojiModelList = JsonConvert.DeserializeObject<List<EmojiModel>>(emojiModelJson);
            EmojiModelList.ForEach(p =>
            {
                p.Type = EmojiType.Emoji;
                p.Size = EmojiSize.Small;
            });

            string qqEmojiModelJson = File.ReadAllText("Emoji/qq_emoji.json");
            QQEmojiModelList = JsonConvert.DeserializeObject<List<EmojiModel>>(qqEmojiModelJson);
            QQEmojiModelList.ForEach(p =>
            {
                p.Type = EmojiType.QQ;
                p.Size = EmojiSize.Small;
            });

            string emojiCodeMapJson = File.ReadAllText("Emoji/emoji_code_map.json");
            EmojiCodeMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(emojiCodeMapJson);
        }

        /// <summary>
        /// 将表情对话框中的表情转换为富文本中的表情
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public EmojiModel FaceToEmoji(EmojiModel face)
        {
            if (face.Size == EmojiSize.Small)
                return null;

            EmojiModel model = null;
            if (face.Type == EmojiType.Emoji)
            {
                if (EmojiMap.TryGetValue(face.Code, out string code))
                {
                    model = EmojiModelList.FirstOrDefault(p => p.Code == code);
                }
            }
            else
            {
                if (QQEmojiMap.TryGetValue(face.Code, out int index))
                {
                    if (QQEmojiModelList.Count > index)
                        model = QQEmojiModelList[index];
                }
            }
            return model;
        }

        /// <summary>
        /// FlowDocument转成字符串
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public string FlowDocumentToString(FlowDocument document)
        {
            string rtn = null;
            foreach (Paragraph block in document.Blocks)
            {
                foreach (var item in block.Inlines)
                {
                    if (item is Run r)
                    {
                        rtn += r.Text;
                    }
                    else if(item is LineBreak)
                    {
                        rtn += Environment.NewLine;
                    }
                    else if (item is InlineUIContainer ui)
                    {
                        ContentControl image = (ContentControl)ui.Child;
                        rtn += image.Content.ToString();
                    }
                }
                rtn += Environment.NewLine;
            }
            return rtn?.TrimEnd();
        }

        /// <summary>
        /// 将字符串转换为富文本设置到TextBlock中
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public void SetToTextBlock(string str, TextBlock document)
        {
            document.Inlines.Clear();
            if (str == null)
                return;
            string[] ss = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < ss.Length; i++)
            {
                SetToTextBlock(document, ss[i]);
                if (i < ss.Length - 1)
                    document.Inlines.Add(new LineBreak());
            }
        }

        private void SetToTextBlock(TextBlock document, string str)
        {
            if (str.Contains("<span class=\"emoji emoji"))
            {
                foreach (var emoji in EmojiModelList)
                {
                    if (TryAddEmoji(emoji, document, str))
                        return;
                }
            }
            if (str.Contains("["))
            {
                foreach (var emoji in QQEmojiModelList)
                {
                    if (TryAddEmoji(emoji, document, str))
                        return;
                }
            }
            document.Inlines.Add(new Run(str));
        }

        private bool TryAddEmoji(EmojiModel emoji, TextBlock document, string str)
        {
            if (str.Contains(emoji.ToString()))
            {
                string left = str.Substring(0, str.IndexOf(emoji.ToString()));
                if (left.Length != 0)
                {
                    if (left.Contains("[") || left.Contains("<span class=\"emoji emoji"))
                        SetToTextBlock(document, left);
                    else
                        document.Inlines.Add(new Run(left));
                }
                ContentControl image = new ContentControl
                {
                    Content = emoji,
                    Width = 20,
                    Height = 20,
                    Style = (Style)Application.Current.FindResource("FaceImgStyle")
                };
                var container = new InlineUIContainer(image) { BaselineAlignment = BaselineAlignment.Bottom };
                document.Inlines.Add(container);

                str = str.Remove(0, (left + emoji.ToString()).Length);
                SetToTextBlock(document, str);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 将Emoji格式的字符串替换成Emoji编码
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string ReplaceEmojiCode(string msg)
        {
            foreach (var item in EmojiCodeMap)
            {
                msg = msg.Replace($"<span class=\"emoji emoji{item.Key}\"></span>", item.Value);
            }
            return msg;
        }
    }
}
