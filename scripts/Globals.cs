using System;
using Godot;
using GodotCollections = Godot.Collections;

public partial class Globals : Node
{
    // Game
    public static string gameVersion = "0.0.0";

    public static string gameDaySavePath = "user://save_slots";

    public static GodotCollections.Dictionary<string, Variant> saveData = new GodotCollections.Dictionary<string, Variant>
    {
        {"player", new GodotCollections.Dictionary<string, Variant>{
            {"score", 0},
            {"highscore", 0},
        }},
        {"days", new GodotCollections.Array<GodotCollections.Dictionary<string, Day>>{
            // Days
            GetDays()
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
        fileAccess.StoreVar(toSave, true);

        if (!DirAccess.DirExistsAbsolute("user://" + savePath)) DirAccess.MakeDirRecursiveAbsolute("user://" + savePath);
        
        fileAccess.Close();
        fileAccess = null;
    }

    public static Variant LoadData(string path)
    {
        FileAccess fileAccess = FileAccess.Open("user://" + path, FileAccess.ModeFlags.Read);
        fileAccess.GetVar(true);
        fileAccess.Close();

        return fileAccess;
    }

    public static Day LoadDay(int dayNumber)
    {
        string dayId = "day-" + dayNumber.ToString() + "/" + "day-" + dayNumber.ToString();
        if (Day.DayExist(dayId)) return Day.Load(dayId);
        return null;
    }

    public static void SaveDay(int day, int score, GodotCollections.Array<InteractableObject> tableItems, string customSaveName = "", string customEvents = "")
    {
        Day daySave = new Day();

        daySave.customName = customSaveName;
        daySave.customEvents = customEvents;

        daySave.day = day;
        daySave.score = score;
        daySave.tableItems = tableItems;

        Day.Save(daySave, ("save_slots/" + "day-" + daySave.day), ("save_slots/" + "day-" + daySave.day + "/" + "day-" + daySave.day));
        daySave = null;
    }

    public static GodotCollections.Dictionary<string, Day> GetDays()
    {
        if (!DirAccess.DirExistsAbsolute(gameDaySavePath + "/")) return null;
        
        string[] savedDays = DirAccess.GetDirectoriesAt(gameDaySavePath + "/");

        GodotCollections.Dictionary<string, Day> days = new GodotCollections.Dictionary<string, Day>{};
        foreach (string day in savedDays) days.Add(day, Day.Load(day + "/" + day));

        return days;
    }

    public static void SettingsReloadSave() // this will be deleted after game releases.
    {
        SettingsReload();
        SaveSettings();
    }

    public static void SettingsReload()
    {
        // Fullscreen
        if ((bool)settings["fullscreen"]) DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        else DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);

        // Sensitivity
        ((GodotCollections.Dictionary<string, float>)settings["sens"])["value"] = Math.Clamp(
            ((GodotCollections.Dictionary<string, float>)settings["sens"])["value"],

            ((GodotCollections.Dictionary<string, GodotCollections.Array<float>>)settings["sens"])["locked"][0],
            ((GodotCollections.Dictionary<string, GodotCollections.Array<float>>)settings["sens"])["locked"][1]
        );
    }
}