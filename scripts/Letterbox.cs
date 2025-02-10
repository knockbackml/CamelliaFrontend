using Godot;

namespace CamelliaFrontend.scripts;

public partial class Letterbox : ColorRect
{
	private readonly StringName _hThicknessName = new("h_thickness");
	private readonly StringName _screenSizeName = new("screen_size");
	[Export] private float _maxAspectRatio;

	private Viewport _viewport;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_viewport = GetViewport();
		_viewport.SizeChanged += UpdateMaterial;
		UpdateMaterial();
	}

	protected override void Dispose(bool disposing)
	{
		_viewport.SizeChanged -= UpdateMaterial;
		base.Dispose(disposing);
	}

	void UpdateMaterial()
	{
		var mat = (ShaderMaterial)Material;
		var size = _viewport is Window win ? win.Size : ((SubViewport)_viewport).Size;
		var aspect = size.Aspect();

		if (aspect > _maxAspectRatio)
		{
			var maxWidth = size.Y * _maxAspectRatio;
			mat.SetShaderParameter(_hThicknessName, (size.X - maxWidth) / 2.0f);
			mat.SetShaderParameter(_screenSizeName, size);
			Visible = true;
		}
		else Visible = false;
	}
}