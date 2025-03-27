using CamelliaBackend.Data.Play;
using CamelliaBackend.Live.Play;
using CamelliaBackend.Manager;
using Godot;

namespace CamelliaFrontend.scripts.Implementation;

public class StageImpl : Stage
{

    protected override Dialog MainDialog { get; }
    
    private Node3D _sceneRoot;
        
    public override Actor AllocateActor(ActorData data)
    {
        switch( data.ActorType) 
        {
            case "image":
                return new ImageActor(_sceneRoot);
            default:
                LoggingManager.LogError($"Unsupported actor type: {data.ActorType}.");
                return null;
        };
    }

    public StageImpl(DialogImpl dialog, Node3D sceneRoot)
    {
        MainDialog = dialog;
        _sceneRoot = sceneRoot;
    }
}