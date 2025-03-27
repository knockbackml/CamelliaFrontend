using System.Text;
using CamelliaFrontend.scripts.Implementation;
using Godot;

namespace CamelliaFrontend.scripts;

public partial class FancyTextLabel : RichTextLabel
{
	private bool _shouldUpdateText = false;
	private DialogImpl _dialog;
	
	public void MarkTextChange() => _shouldUpdateText = true;
	
	public void SetDialog(DialogImpl dialog) => _dialog = dialog;

	public override void _Process(double delta)
	{
		if (!_shouldUpdateText) return;

		var sb = new StringBuilder();
		foreach (var region in _dialog.TextRegions)
		{
			if (region.IsVisible) sb.Append(region.CurrentText);
		}

		Text = sb.ToString();
		
		_shouldUpdateText = false;
	}
}
