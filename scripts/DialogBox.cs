using Godot;

namespace CamelliaFrontend.scripts;

public partial class DialogBox : Control
{
	[Export] private RichTextLabel _flowText;
	[Export] private RichTextLabel _charaName;
	[Export] private RichTextLabel _charaNickname;

	public void UpdateFlowText(string flowText)
	{
		_flowText.Text = flowText;
	}

	public void ClearDialog()
	{
		_flowText.Clear();
		_charaName.Clear();
		_charaNickname.Clear();
	}
}
