using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity.Attributes;
using WeChatClient.EmojiCore;
using WeChatClient.EmojiCore.Emoji;
using WeChatClient.EmojiCore.Events;
using WeChatClient.EmojiCore.Face;

namespace WeChatClient.Face.ViewModels
{
    public class FaceViewModel : ReactiveObject
    {
        [Reactive]
        public List<EmojiModel> EmojiFaceList { get; private set; }
        [Reactive]
        public List<EmojiModel> QQFaceList { get; private set; }
        public ICommand ClickFaceCommand { get; }

        public FaceViewModel(FaceManager faceManager, EmojiManager emojiManager, IEventAggregator ea)
        {
            EmojiFaceList = faceManager.EmojiFaceList;
            QQFaceList = faceManager.QQFaceList;

            ClickFaceCommand = ReactiveCommand.Create<EmojiModel>(face =>
            {
                EmojiModel emoji = emojiManager.FaceToEmoji(face);
                if (emoji != null)
                    ea.GetEvent<InputEmojiEvent>().Publish(emoji);
            });
        }
    }
}
