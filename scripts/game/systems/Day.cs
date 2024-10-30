using Godot;

public partial class Day : Resource
{
	[Export] public string saveCustomName;

	[Export(PropertyHint.MultilineText)] public Godot.Collections.Array<Events.EventType> saveCustomEvents;

	[Export] public byte[] saveGameVersion = new byte[] {Globals.gameVersionMajor, Globals.gameVersionMinor, Globals.gameVersionPatch};

	[Export] public int saveDay;
	[Export] public int saveScore;
	[Export] public int saveDayNextSeason;
	[Export] public Seasons.SeasonType saveSeason;
	[Export] public Difficulty.DifficultyTypes saveDifficulty;
	
	[Export] public Godot.Collections.Array<InteractableObject.InteractableObjectType> saveTableItemsRow = new Godot.Collections.Array<InteractableObject.InteractableObjectType>{};
	[Export] public Godot.Collections.Array<InteractableObject.InteractableObjectType> saveTableItemsCollumn = new Godot.Collections.Array<InteractableObject.InteractableObjectType>{};

	public InteractableObject.InteractableObjectType savePlayerHeldItem;

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
