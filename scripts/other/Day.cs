using Godot;

public partial class Day : Resource
{
	[Export]
	public int day;
	[Export]
	public int score;
	[Export]
	public Godot.Collections.Array<TableItemObject> tableItems;

	public static void Save(Day daySave, string path, string file)
	{
		if (!DirAccess.DirExistsAbsolute("user://" + path)) DirAccess.MakeDirRecursiveAbsolute("user://" + path);

		path = "user://" + path + "/" + file;
		ResourceSaver.Save(daySave, path, ResourceSaver.SaverFlags.Compress);
		GD.Print("File saved at: " + path);
	}

	public static Day Load(string path)
	{
		path = "user://" + path;
		return ResourceLoader.Load<Day>(path);
	}
}
