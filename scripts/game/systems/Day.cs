using Godot;

public partial class Day : Resource
{
	[Export]
	public string customName;

	[Export(PropertyHint.MultilineText)]
	public string customEvents;

	[Export]
	public string saveGameVersion = Globals.gameVersion;

	[Export]
	public int day;
	[Export]
	public int score;
	[Export]
	public Seasons.SeasonType season;
	[Export]
	public Difficulty.DifficultyTypes difficulty;
	
	public InteractableObject playerHeldItem;
	[Export]
	public Godot.Collections.Array<InteractableObject> tableItems;

	public static void Save(Day daySave, string path, string pathToFile)
	{
		if (!DirAccess.DirExistsAbsolute("user://" + path)) DirAccess.MakeDirRecursiveAbsolute("user://" + path);

		pathToFile = "user://" + pathToFile + ".res";
		ResourceSaver.Save(daySave, pathToFile, ResourceSaver.SaverFlags.Compress);
		GD.Print("File saved at: " + pathToFile);
	}

	public static Day Load(string path)
	{
		path = Globals.gameDaySavePath + "/" + path + ".res";
		return ResourceLoader.Load<Day>(path);
	}

	public static bool DayExist(string path)
	{
		if (Load(path) != null) return true;
		return false;
	}
}
