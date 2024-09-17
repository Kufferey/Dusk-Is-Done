using System;
using Godot;
using GodotCollections = Godot.Collections;

public partial class Globals : Node
{
    public static bool playerCanMoveCamera = true;

    public static int playerScore;

    public static GodotCollections.Dictionary<string, Variant> settings = new GodotCollections.Dictionary<string, Variant>
    {
        {"fullscreen", false},
        {"sens", new GodotCollections.Dictionary<string, Variant>{
            {"locked", new GodotCollections.Array<float>{
                {0.1F}, // Min value
                {2F}    // Max value
            }},
            {"value", 0.8F}
        }},
    };

    public override void _Ready()
    {
        SettingsReload();
    }

    public void SettingsReload()
    {
        // Fullscreen
        if ((bool)settings["fullscreen"]) 
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
        else
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }

        // Sensitivity
        ((Godot.Collections.Dictionary<string, float>)settings["sens"])["sens"] = Math.Clamp(
            ((GodotCollections.Dictionary<string, float>)settings["sens"])["value"],

            ((GodotCollections.Dictionary<string, GodotCollections.Array<float>>)settings["sens"])["locked"][0],
            ((GodotCollections.Dictionary<string, GodotCollections.Array<float>>)settings["sens"])["locked"][1]
        );
    }
}