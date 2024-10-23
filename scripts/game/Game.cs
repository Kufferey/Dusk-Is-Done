using System;
using static Godot.GD;
using Godot;

public partial class Game : Node3D
{
	[Signal] public delegate void NewDayEventHandler();
	[Signal] public delegate void EndDayEventHandler();

	public static Difficulty.DifficultyTypes? difficulty {get; set;}
	public static int currentDay {get; set;}
	public static int currentSection {get; set;}

	[Export] private Player player {get; set;}
	[Export] private Table table {get; set;}

	private int maxCherries = 6;
	private int currentCherries;

	public override void _Ready()
	{
		EnterGame();
	}

	public override void _Process(double delta)
	{
		if (IsSectionClear()) NewSection();
	}

	public void EnterGame()
	{
		// Load all game assets and set variables.
	}

	public void ExitGame()
	{
		// Unload all game assets and null variables.
	}

	private bool IsSectionClear()
	{
		int cherryAmount = GetNode<Node>("Interactables/Cherry/Cherries").GetChildCount();
		
		if (cherryAmount > 0) return false;
		return true;
	}

	private void NewSection()
	{
		Print("NEW SECTION");
		AddCherries(3);

		currentSection++;
	}

	private Godot.Collections.Array<Vector3> GetSpawnPoints()
	{
		Godot.Collections.Array<Vector3> positions = new Godot.Collections.Array<Vector3>{};
		Godot.Collections.Array<Node> spawnPoints = GetNode<Node>("Interactables/Cherry/SpawnPoints").GetChildren();

		foreach (Node3D node in spawnPoints) positions.Add(node.Position);

		return positions;
	}

	private Vector3 GetRandomSpawnPoint()
	{
		Godot.Collections.Array<Vector3> spawnPoints = GetSpawnPoints();
		var randomVector = RandRange(0, GetSpawnPoints().Count - 1);
		var randomSpawnPosition = spawnPoints[randomVector];
		Print($"Returned: {randomSpawnPosition}\n RandomIndex: {randomVector}");

		return (Vector3)randomSpawnPosition;
	}

	private void AddCherries(int amount)
	{
		// TODO: GODOT DOES NOT LIKE RANDOMRANGE FOR LOOPS. "randomPosition', the random positions work. NVM Fixed. // TO KEEP NOTE USING GODOT C# - (Skill issue. I forgot to decrement by 1)
		currentCherries = 0;
		for (int cherries = 0; cherries < amount; cherries++)
		{
			AddInteractableObject(InteractableObject.InteractableObjectType.Cherry, GetRandomSpawnPoint(), new Vector3(0,0,0), "Interactables/Cherry/Cherries");
			currentCherries++;
		}
	}

    public void PlayerZoomCamera(float from, float to, Tween.EaseType easeType, bool useCameraSens, float duration = 0.3F)
	{
		if (player.HasMethod("ZoomCamera"))
		{
			player.Callv(Player.MethodName.ZoomCamera, new Godot.Collections.Array{
				from, to, (int)easeType, useCameraSens, duration
			});	
		}
	}

	public void PlayerZoomAndLockCamera(float from, float to, Vector3 position, Tween.EaseType easeType, bool useSens, bool lockCam, float duration = 0.3F)
	{
		if (player.HasMethod("ZoomAndLockCamera"))
		{
			player.Callv(Player.MethodName.ZoomAndLockCamera, new Godot.Collections.Array{
				from, to, position, (int)easeType, useSens, lockCam, duration
			});	
		}
	}

	public void StoreTableItem(InteractableObject.InteractableObjectType type)
	{
		// if ( > (int)(tableItemsAllowedRow + tableItemsAllowedCollumn)) return;


	}


	public void OnItemHovered(InteractableObject.InteractableObjectType type)
	{

	}

	public void OnItemInteracted(InteractableObject.InteractableObjectType type)
	{
		switch (type)
		{
			case InteractableObject.InteractableObjectType.None:

				Player.playerHealth -= 50.0F;

			break;

			default: break;
		}
	}

	public void OnItemUsed(InteractableObject.InteractableObjectType type)
	{

	}

	public void AddInteractableObject(InteractableObject.InteractableObjectType type, Vector3 position = default, Vector3 rotation = default, string nodePath = "Interactables")
	{
		InteractableObject interactableObject = InteractableObjectManager.interactableObjectPrefabs[type].Instantiate<InteractableObject>(PackedScene.GenEditState.Disabled);

		// Item signals
		interactableObject.ItemHovered += OnItemHovered;
		interactableObject.ItemInteracted += OnItemInteracted;
		interactableObject.ItemUsed += OnItemUsed;

		// Item position, and rotation
		interactableObject.Position = (Vector3)position;
		interactableObject.Rotation = (Vector3)rotation;

		GetNode<Node>(nodePath).AddChild(interactableObject);
	}
}
