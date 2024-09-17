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
            {"value", 5.8F} // Default 0.8
        }},
    };

    public static void SettingsReload()
    {
        // Fullscreen
        if ((bool)settings["fullscreen"]) 
        {DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);}
        else
        {DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);}

        // Sensitivity
        ((GodotCollections.Dictionary<string, float>)settings["sens"])["value"] = Math.Clamp(
            ((GodotCollections.Dictionary<string, float>)settings["sens"])["value"],

            ((GodotCollections.Dictionary<string, GodotCollections.Array<float>>)settings["sens"])["locked"][0],
            ((GodotCollections.Dictionary<string, GodotCollections.Array<float>>)settings["sens"])["locked"][1]
        );
    }
}