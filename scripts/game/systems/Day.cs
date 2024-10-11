using Godot;

public partial class Day : Resource
{
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
	public Godot.Collections.Array<TableItemObject> tableItems;

	public static void Save(Day daySave, string path)
	{
		if (!DirAccess.DirExistsAbsolute("user://" + path)) DirAccess.MakeDirRecursiveAbsolute("user://" + path);

		path = "user://daySlots" + path + ".res";
		ResourceSaver.Save(daySave, path, ResourceSaver.SaverFlags.Compress);
		GD.Print("File saved at: " + path);
	}

	public static Day Load(string path)
	{
		path = "user://daySlots" + path + ".res";
		return ResourceLoader.Load<Day>(path);
	}
}
