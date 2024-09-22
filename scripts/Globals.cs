using System;
using Godot;
using GodotCollections = Godot.Collections;

public partial class Globals : Node
{
    public static bool playerCanMoveCamera = true;

    public static GodotCollections.Dictionary<string, Variant> saveData = new GodotCollections.Dictionary<string, Variant>
    {
        {"player", new GodotCollections.Dictionary<string, Variant>{
            {"score", 0},
            {"highscore", 0},
        }}
    };

    public static GodotCollections.Dictionary<string, Variant> settings = new GodotCollections.Dictionary<string, Variant>
    {
        {"fullscreen", false},
        {"sens", new GodotCollections.Dictionary<string, Variant>{
            {"locked", new GodotCollections.Array<float>{
                {0.1F}, // Min value
                {2F}    // Max value
            }},
            {"value", 0.7F} // Default 0.8
        }},


        // Versions
        {"version", "0.1.0"},
        {"settings_version", "0.1.0"},
    };

    public static void SettingsSave()
    {
        ConfigFile configFile = new ConfigFile();

        // Display
        configFile.SetValue("display", "fullscreen", settings["fullscreen"]);

        // Gameplay
        configFile.SetValue("gameplay", "sensitivity", ((GodotCollections.Dictionary<string, float>)settings["sens"])["value"]);

        // Misc
        configFile.SetValue("other", "version", settings["version"]);
        configFile.SetValue("other", "settings_version", settings["settings_version"]);

        // Save/Cleanup
        configFile.Save("user://settings.cfg");
        configFile = null;
    }

    public static void SettingsReloadSave()
    {
        SettingsReload();
        SettingsSave();
    }

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