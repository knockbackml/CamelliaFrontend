using System.Collections.Generic;
using CamelliaBackend;
using CamelliaBackend.Data;
using CamelliaBackend.Data.Action;
using CamelliaBackend.Data.Play;
using CamelliaBackend.Live.Play;
using CamelliaBackend.Manager;
using CamelliaFrontend.scripts.Implementation;
using Godot;
using Node = Godot.Node;

namespace CamelliaFrontend.scripts;

public partial class CamelliaFrontend : Node
{
    [Export] private RichTextLabel _mainDialogCharacterName, _mainDialogCharacterNick;
    [Export] private FancyTextLabel _mainDialogContent;
    [Export] private Node3D _sceneRoot;

    private Stage _stage;
    
    public override void _Ready()
    {
        LoggingManager.Init(new LoggingImpl());
        
        var mainDialog = new DialogImpl(_mainDialogCharacterName, _mainDialogCharacterNick, _mainDialogContent);
        _stage = new StageImpl(mainDialog, _sceneRoot);

        var testStage = new StageData
        {
            Scripts = new Dictionary<string, string>
            {
                ["move_to"] = 
"""
function run() {
    var from = prev === null ? orig : prev;
    var factor = time / duration;
    var x = from[0] + factor * (to[0] - from[0]);
    var y = from[1] + factor * (to[1] - from[1]);
    var z = from[2] + factor * (to[2] - from[2]);
    return [x, y, z];
};
""",
                ["typewriter"] =
"""
const seconds_per_char = 0.1;

function judge_tag(str, index) {
    var i = index + 1;
    if (i >= str.length) return ["", i];

    for (; i < str.length; i++) {
        if (str[i] === ']') break;
    }
    return [str.substring(index + 1, i), i];
}

function duration() {
    return full_text_length * seconds_per_char;
}

function run() {
    if (time >= duration()) return orig;

    var shown_count = time / seconds_per_char;
    var i = 0;
    var last_push_index = 0;
    var builder = [];

    for (; i < orig.length && shown_count >= 1; i++, shown_count--) {
        if (orig[i] !== '[') {
            continue;
        }

        var info = judge_tag(orig, i);
        i = info[1];
        if (i >= orig.length) i--;
    }
    builder.push(orig.substring(last_push_index, i));
    last_push_index = i;

    builder.push("[color=transparent]");

    var tag_depth = 0;
    for (; i < orig.length; i++) {
        if (orig[i] === '[') {
            var info = judge_tag(orig, i);
            if (info[1] < orig.length) {
                if (info[0].startsWith("color=") || info[0] === "/color") {
                    builder.push(orig.substring(last_push_index, i));
                    i = info[1];
                    last_push_index = i + 1;
                } else if (info[0].startsWith("/")) {
                    if (tag_depth <= 0) {
                        builder.push(orig.substring(last_push_index, i));

                        builder.push("[/color]");
                        builder.push(orig.substring(i, info[1] + 1));
                        builder.push("[color=transparent]");

                        i = info[1];
                        last_push_index = i + 1;
                    } else {
                        tag_depth--;
                    }
                } else {
                    tag_depth++;
                }
            } else {
                break;
            }
        }
    }
    builder.push(orig.substring(last_push_index));
    builder.push("[/color]");
    return builder.join("");
};
"""
            },
            Actions = new Dictionary<string, ActionData>
            {
                ["life"] = new LifeActionData
                {
                    ActionName = "life"
                },
                ["typewriter"] = new ModifierActionData
                {
                    ActionName = "typewriter",
                    AttributeName = "text",
                    ScriptName = "typewriter",
                    ValueType = Value.Types.Text
                },
                ["move_to"] = new ModifierActionData
                {
                    ActionName = "move_to",
                    AttributeName = "position",
                    DefaultParams = new Dictionary<string, Value>
                    {
                        ["to"] = new(0.0f, 0.0f, 0.0f)
                    },
                    ScriptName = "move_to",
                    ValueType = Value.Types.Vector
                }
            },
            Actors = new Dictionary<string, ActorData>
            {
                ["actor1"] = new()
                {
                    ActorType = "image",
                    Id = "actor1",
                    Name = "Actor 1",
                    NickName = "",
                    DefaultAttributes = new Dictionary<string, Value>
                    {
                        ["image_path"] = "actor1.png",
                        ["pixels_per_unit"] = 100.0f
                    }
                }
            },
            Beats =
            [
                new BeatData
                {
                    Activities = new Dictionary<uint, ActivityData>
                    {
                        [1u] = new()
                        {
                            ActorId = "actor1",
                            InitialAttributes = new Dictionary<string, Value>
                            {
                                ["position"] = new(0.0f, 0.0f, 0.0f),
                                ["rotation"] = new(0.0f, 0.0f, 0.0f, 1.0f),
                                ["scale"] = new(1.0f, 1.0f, 1.0f)
                            },
                            Timeline = new ActionTimelineData
                            {
                                Tracks = [
                                    [
                                        new ActionTimelineKeyframeData
                                        {
                                            PreferredDuration = 20.0f,
                                            ActionName = "move_to",
                                            OverrideParams = new Dictionary<string, Value>
                                            {
                                                ["to"] = new(0.0f, -2.0f, 0.0f)
                                            },
                                            Time = 0.0f,
                                            Linger = true
                                        },
                                        new ActionTimelineKeyframeData
                                        {
                                            PreferredDuration = 2.0f,
                                            ActionName =  "move_to",
                                            OverrideParams = new Dictionary<string, Value>
                                            {
                                                ["to"] = new(1.0f, 0.0f, 0.0f)
                                            },
                                            Time =  5.0f,
                                            Linger = true
                                        }
                                    ]
                                ]
                            }
                        }
                    },
                    Dialog = new DialogData
                    {
                        CharacterId = "actor1",
                        RegionLifeTimeline = null,
                        Regions =
                        [
                            new DialogData.TextRegionData
                            {
                                Id = 0u,
                                Text = "Lorem ipsum dolor sit amet, [i]consectetur[/i] adipiscing elit. ",
                                Timeline = new ActionTimelineData(),
                                TransitionDuration = 5.0f,
                                TransitionScriptName = "typewriter"
                            },
                            new DialogData.TextRegionData
                            {
                                Id = 1u,
                                Text = "Nam vulputate, nisi et mattis varius, [b]lorem lacus tincidunt quam[/b], at iaculis lacus augue sit amet nunc. Sed nec maximus metus, vel malesuada turpis. Nullam feugiat ex maximus lacus bibendum, ut [color=gray][s]congue dui pulvinar[/s][/color]. In condimentum at nulla sit amet sollicitudin. Vivamus dapibus aliquet leo, venenatis condimentum lacus sollicitudin et. Pellentesque eu arcu imperdiet, dictum velit sit amet, posuere ipsum. Phasellus sodales vitae libero at consectetur. ",
                                Timeline = new ActionTimelineData(),
                                TransitionDuration = 5.0f,
                                TransitionScriptName = "typewriter"
                            },
                            new DialogData.TextRegionData
                            {
                                Id = 2u,
                                Text = "Cras fermentum [color=red]condimentum[/color] metus, varius tempor nibh efficitur et.",
                                Timeline = new ActionTimelineData(),
                                TransitionDuration = 5.0f,
                                TransitionScriptName = "typewriter"
                            }
                        ]
                    }
                }
            ]
        };
        _stage.Initialize(testStage);
        _stage.Advance();
    }

    private float _timer = 0.0f;
    public override void _Process(double delta)
    {
        _timer += (float)delta;
        _stage.Update(_timer);
    }
}