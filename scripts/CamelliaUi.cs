using System;
using CamelliaBridge;
using Godot;

namespace CamelliaFrontend.scripts;

public partial class CamelliaUi : Node, ICamelliaUi
{
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
		GD.Print("Hello");
	}
}