using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Attributes;
using WeChatClient.EmojiCore;
using WeChatClient.EmojiCore.Face;

namespace WeChatClient.Face.ViewModels
{
    public class FaceViewModel : ReactiveObject
    {
        [Reactive]
        public List<EmojiModel> EmojiFaceList { get; private set; }
        [Reactive]
        public List<EmojiModel> QQFaceList { get; private set; }

        public FaceViewModel(FaceManager faceManager)
        {
            EmojiFaceList = faceManager.EmojiFaceList;
            QQFaceList = faceManager.QQFaceList;
        }
    }
}
