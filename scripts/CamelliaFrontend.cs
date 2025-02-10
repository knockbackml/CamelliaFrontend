using System;
using CamelliaBridge.Events;
using CamelliaBridge.Interfaces;
using CamelliaBridge.Messages;
using Godot;
using Godot.Collections;

namespace CamelliaFrontend.scripts;

public partial class CamelliaFrontend : Node, ICamelliaFrontend
{
	// Maps identifiers used by CamelliaBridge to Godot nodes.
	[Export] private Dictionary<ulong, Node> _nodeDict;

	[Export] private DialogBox _dialogBox;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Init()
	{
		GD.Print("Frontend initialized.");
	}

	public void Notify(CamelliaMessage msg)
	{
		switch (msg)
		{
			case DialogClearMessage:
			{
				_dialogBox.ClearDialog();
				break;
			}
			case DialogFlowTextUpdateMessage dialogFlowTextUpdateMessage:
			{
				_dialogBox.UpdateFlowText(dialogFlowTextUpdateMessage.Text);
				break;
			}
			case DialogHeaderUpdateMessage dialogHeaderUpdateMessage:
			{
				break;
			}
			case TargetedCamelliaMessage targetedCamelliaMessage:
			{
				var hasNode = _nodeDict.TryGetValue(targetedCamelliaMessage.Target, out var node);
				if ((targetedCamelliaMessage.Access & TargetedCamelliaMessage.TargetAccess.Create) == 0 && !hasNode)
					throw new ArgumentException("The target of targeted message is not found.");

				switch (targetedCamelliaMessage)
				{
					case DialogFreeTextUpdateMessage dialogFreeTextUpdateMessage:
					{
						break;
					}
					default:
						throw new ArgumentOutOfRangeException(nameof(msg));
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException(nameof(msg));
		}
	}

	public event Action<CamelliaEvent> OnEvent;
}
