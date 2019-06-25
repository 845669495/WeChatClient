using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core.Dependency;

namespace WeChatClient.EmojiCore.Face
{
    [ExposeServices(ServiceLifetime.Singleton)]
    public class FaceManager
    {
        public List<EmojiModel> EmojiFaceList { get; }
        public List<EmojiModel> QQFaceList { get; }
        public FaceManager()
        {
            string emojiFaceJson = File.ReadAllText("Face/emoji_face.json");
            EmojiFaceList = JsonConvert.DeserializeObject<List<EmojiModel>>(emojiFaceJson);
            EmojiFaceList.ForEach(p =>
            {
                p.Type = EmojiType.Emoji;
                p.Size = EmojiSize.Medium;
            });

            string qqFaceJson = File.ReadAllText("Face/qq_face.json");
            QQFaceList = JsonConvert.DeserializeObject<List<EmojiModel>>(qqFaceJson);
            QQFaceList.ForEach(p =>
            {
                p.Type = EmojiType.QQ;
                p.Size = EmojiSize.Medium;
            });
        }
    }
}
