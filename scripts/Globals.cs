using System;
using Godot;
using GodotCollections = Godot.Collections;

public partial class Globals : Node
{
    // Game
    public static string gameVersion = "0.0.0";

    // Gameplay
    public static bool playerCanMoveCamera = true;
    public static bool playerCanInteract = true;

    // Other

    public static GodotCollections.Dictionary<string, Variant> saveData = new GodotCollections.Dictionary<string, Variant>
    {
        {"player", new GodotCollections.Dictionary<string, Variant>{
            {"score", 0},
            {"highscore", 0},
        }},
        {"days", new GodotCollections.Array<GodotCollections.Dictionary<string, Variant>>{
            new GodotCollections.Dictionary<string, Variant>
            {
                // Days
            }
        }}
    };

    public static GodotCollections.Dictionary<string, Variant> settings = new GodotCollections.Dictionary<string, Variant>
    {
        {"fullscreen", false},

        {"sens", new GodotCollections.Dictionary<string, Variant>{
            {"locked", new GodotCollections.Array<float>{
                {0.1F}, // Min value
                {2F},   // Max value
                {0.8F}  // Def value
            }},
            {"value", 0.8F} // Default 0.8
        }},

        {"keybinds", new GodotCollections.Dictionary<string, InputEvent>{
            {"mouseleft", null},
            {"mouseright", null},
            {"interact", null},
        }},


        // Versions
        {"version", gameVersion},
        {"settings_version", "0.1.0"},
    };

    public static void SaveSettings()
    {
        ConfigFile configFile = new ConfigFile();

        // Display
        configFile.SetValue("display", "fullscreen", settings["fullscreen"]);

        // Gameplay
        configFile.SetValue("gameplay", "sensitivity", ((GodotCollections.Dictionary<string, float>)settings["sens"])["value"]);
        configFile.SetValue("gameplay", "keybinds", settings["keybinds"]);

        // Misc
        configFile.SetValue("other", "version", settings["version"]);
        configFile.SetValue("other", "settings_version", settings["settings_version"]);

        // Save/Cleanup
        configFile.Save("user://settings.cfg");
        configFile = null;
    }

    public static void SetKeybind(GodotCollections.Dictionary<string, InputEvent> keybind)
    {

    }

    public static void SaveData(string savePath, Variant toSave)
    {
        FileAccess fileAccess = FileAccess.Open("user://" + savePath, FileAccess.ModeFlags.Write);
        fileAccess.StoreVar(toSave);

        if (!DirAccess.DirExistsAbsolute("user://" + savePath)) DirAccess.MakeDirRecursiveAbsolute("user://" + savePath);
        
        fileAccess.Close();
        fileAccess = null;
    }

    public static void SaveDay(int day, int score, GodotCollections.Array<TableItemObject> tableItems)
    {
        Day daySave = new Day();
        daySave.day = day;
        daySave.score = score;
        daySave.tableItems = tableItems;

        Day.Save(daySave, "daySlots", "day-" + daySave.day + ".tscn");
        daySave = null;
    }

    public static void SettingsReloadSave() // this will be deleted after game releases.
    {
        SettingsReload();
        SaveSettings();
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