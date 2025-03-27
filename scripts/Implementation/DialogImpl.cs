using CamelliaBackend.Live.Play;
using Godot;

namespace CamelliaFrontend.scripts.Implementation;

public class DialogImpl : Dialog
{
    public class TextRegionImpl : TextRegion
    {
        public override bool HandleTextUpdate(string text)
        {
            ((DialogImpl)ParentDialog)._contentLabel.MarkTextChange();
            return true;
        }

        public override void OnVisibilityUpdate(bool isVisible)
        {
            ((DialogImpl)ParentDialog)._contentLabel.MarkTextChange();
        }
    }

    private RichTextLabel _characterNameLabel, _characterNickLabel;
    private FancyTextLabel _contentLabel;

    public DialogImpl(RichTextLabel characterNameLabel, RichTextLabel characterNickLabel, FancyTextLabel contentLabel)
    {
        _characterNameLabel = characterNameLabel;
        _characterNickLabel = characterNickLabel;
        _contentLabel = contentLabel;
        _contentLabel.SetDialog(this);
    }

    public override TextRegion AllocateTextRegion()
    {
        return new TextRegionImpl();
    }
}