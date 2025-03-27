using CamelliaBackend;
using CamelliaBackend.Data.Play;
using CamelliaBackend.Live.Play;
using CamelliaBackend.Manager;
using Godot;
using Quaternion = System.Numerics.Quaternion;
using Vector3 = System.Numerics.Vector3;

namespace CamelliaFrontend.scripts.Implementation;

public class ImageActor : Actor
{
    private readonly Node3D _sceneRoot;
    private readonly Sprite3D _sprite = new();

    public ImageActor(Node3D sceneRoot)
    {
        _sceneRoot = sceneRoot;
        _sprite.PixelSize = 0.01f;
    }
    
    public override void Init(ActorData data)
    {
        base.Init(data);
        
        _sceneRoot.AddChild(_sprite);
    }

    public override void Fina()
    {
        base.Fina();

        _sceneRoot.RemoveChild(_sprite);
        _sprite.Texture = null;
    }

    public override bool HandleDirtyAttribute(string key, Value value)
    {
        switch (key)
        {
            case "rotation":
            {
                if (value.ValueType != Value.Types.Quaternion)
                    LoggingManager.LogError($"Rotation for image actor {Data.Id} is not a quaternion.");
                else
                    _sprite.Quaternion = ((Quaternion)value).ToGodot();
                return true;
            }
            case "position":
            {
                if (value.ValueType != Value.Types.Vector)
                    LoggingManager.LogError($"Position for image actor {Data.Id} is not a vector.");
                else
                    _sprite.Position = ((Vector3)value).ToGodot();
                return true;
            }
            case "scale":
            {
                if (value.ValueType != Value.Types.Vector)
                    LoggingManager.LogError($"Scale for image actor {Data.Id} is not a vector.");
                else
                    _sprite.Scale = ((Vector3)value).ToGodot();
                return true;
            }
            case "image_path":
            {
                if (value.ValueType != Value.Types.Text)
                    LoggingManager.LogError($"Image path for image actor {Data.Id} is not a text.");
                var image = GD.Load<Texture2D>("res://resources/" + (string)value);
                if (image is null)
                    LoggingManager.LogError($"Image not found for image actor {Data.Id}.");
                else
                    _sprite.Texture = image;
                return true;
            }
            case "pixels_per_unit":
            {
                if (value.ValueType != Value.Types.Number)
                    LoggingManager.LogError($"Pixels per unit for image actor {Data.Id} is not a number.");
                _sprite.PixelSize = 1.0f / (float)value;
                return true;
            }
            default: 
                return false;
        }
    }
}