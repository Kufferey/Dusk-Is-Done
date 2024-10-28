using static Godot.GD;
using Godot;
using System;

public partial class Game : Node3D
{
	[Signal] public delegate void NewDayEventHandler();
	[Signal] public delegate void EndDayEventHandler();

	public static Difficulty.DifficultyTypes? Difficulty {get; set;}
	public static int CurrentDay {get; set;}
	public static int CurrentSection {get; set;}

	[Export] private Player Player {get; set;}
	[Export] private Table Table {get; set;}

	private byte _maxCherries = 6;
	private byte _currentCherries;

	public override void _Ready()
	{
		EnterGame();
	}


	public override void _Process(double delta)
	{
		if (IsSectionClear()) NewSection();
	}

	private void EnterGame()
	{
		// Load all game assets and set variables.
	}

	private void ExitGame()
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
		AddCherries(1);

		CurrentSection++;
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
		// TODO: GODOT DOES NOT LIKE RANDOMRANGE FOR LOOPS. "randomPosition', the random positions work. NVM Fixed. // TO KEEP NOTE USING GODOT C# - (Skill issue. I forgot to decrement)
		_currentCherries = 0;
		for (int cherries = 0; cherries < amount; cherries++)
		{
			byte randomChance = (byte)RandRange(1, 5);
			if (randomChance < 1) AddInteractableObject(InteractableObject.InteractableObjectType.CherrySpoiled, GetRandomSpawnPoint(), new Vector3(0,0,0), "Interactables/Cherry/Cherries");
			else AddInteractableObject(InteractableObject.InteractableObjectType.Cherry, GetRandomSpawnPoint(), new Vector3(0,0,0), "Interactables/Cherry/Cherries");

			_currentCherries++;
		}
	}

    private void PlayerZoomCamera(float from, float to, Tween.EaseType easeType,
	bool useCameraSens, float duration = 0.3F)
	{
		if (Player.HasMethod("ZoomCamera"))
		{
			Player.Callv(Player.MethodName.ZoomCamera, new Godot.Collections.Array{
				from, to, (int)easeType, useCameraSens, duration
			});	
		}
	}

	private void PlayerZoomAndLockCamera(float from, float to, Vector3 position,
	Tween.EaseType easeType, bool useSens, bool lockCam,
	float duration = 0.3F)
	{
		if (Player.HasMethod("ZoomAndLockCamera"))
		{
			Player.Callv(Player.MethodName.ZoomAndLockCamera, new Godot.Collections.Array{
				from, to, position, (int)easeType, useSens, lockCam, duration
			});	
		}
	}

	private void StoreTableItem(InteractableObject.InteractableObjectType type)
	{
		// if ( > (int)(tableItemsAllowedRow + tableItemsAllowedCollumn)) return;


	}


	private void OnItemHovered(InteractableObject.InteractableObjectType type)
	{

	}

	private void OnItemInteracted(InteractableObject.InteractableObjectType type)
	{
		switch (type)
		{
			/*
					GAMEPLAY RELATED
			*/
			case InteractableObject.InteractableObjectType.None:

				//Code

			break;

			case InteractableObject.InteractableObjectType.CherrySpoiled:
			case InteractableObject.InteractableObjectType.Cherry:

				//Code

			break;

			case InteractableObject.InteractableObjectType.Notebook:

				//Code

			break;

			/*
					HEALTH RELATED:
			*/
			case InteractableObject.InteractableObjectType.Bandage:

				//Code

			break;

			case InteractableObject.InteractableObjectType.Pills:

				//Code

			break;

			case InteractableObject.InteractableObjectType.MedicalPills:

				//Code

			break;

			case InteractableObject.InteractableObjectType.MedicalKit:

				//Code

			break;


			default: break;
		}
	}

	private void OnItemUsed(InteractableObject.InteractableObjectType type)
	{
		switch (type)
		{
			/*
					GAMEPLAY RELATED
			*/
			case InteractableObject.InteractableObjectType.None:

				//Code

			break;

			case InteractableObject.InteractableObjectType.CherrySpoiled:
			case InteractableObject.InteractableObjectType.Cherry:

				//Code

			break;

			case InteractableObject.InteractableObjectType.Notebook:

				//Code

			break;

			/*
					HEALTH RELATED:
			*/
			case InteractableObject.InteractableObjectType.Bandage:

				// Player health goes up by 5-10.

			break;

			case InteractableObject.InteractableObjectType.Pills:

				// Health goes up by 15-20.

			break;

			case InteractableObject.InteractableObjectType.MedicalPills:

				// Health goes up 30-55.

			break;

			case InteractableObject.InteractableObjectType.MedicalKit:

				// Max health
				Player.playerHealth = Player.playerMaxHealth;

			break;


			default: break;
		}
	}

	private void AddInteractableObject(InteractableObject.InteractableObjectType type, Vector3 position = default, Vector3 rotation = default,
	string nodePath = "Interactables")
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
