using System;
using Godot;

public partial class Game : Node3D
{
	[Signal]
	public delegate void NewDayEventHandler();
	[Signal]
	public delegate void EndDayEventHandler();

	[Signal]
	public delegate void TableItemAddedEventHandler(int atPos, InteractableObject.InteractableObjectType interactableType);
	[Signal]
	public delegate void TableItemRemovedEventHandler(int atPos, InteractableObject.InteractableObjectType interactableType);

	public static int currentDay {get; set;}
	public static Difficulty.DifficultyTypes? difficulty {get; set;}

	[Export]
	public Player player {get; set;}

	public override void _Ready()
	{
		InteractableObjectManager.AddInteractableItemToPrefabs(InteractableObject.InteractableObjectType.None, "res://scenes/interactables/table_items/test.tscn");
		// AddInteractableObject(InteractableObject.InteractableObjectType.None, new Vector3(2,2,2), new Vector3(2,2,2));
		
		EnterGame();
	}

	public override void _Process(double delta)
	{
		
	}

	public void EnterGame()
	{
		// Load all game assets and set variables.
	}

	public void ExitGame()
	{
		// Unload all game assets and null variables.
	}

	public bool IsSectionClear()
	{
		int cherryAmount = 0;
		for (int cherry = 0; cherry < GetNode<Node>("Interactables/Cherries").GetChildren().Count; cherry++) cherryAmount++;
		
		if (cherryAmount > 0) return false;
		return true;
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
		
	}

	public void OnItemUsed(InteractableObject.InteractableObjectType type)
	{

	}

	public void AddInteractableObject(InteractableObject.InteractableObjectType type, Vector3 position, Vector3 rotation)
	{
		InteractableObject interactableObject = InteractableObjectManager.interactableObjectPrefabs[type].Instantiate<InteractableObject>(PackedScene.GenEditState.Disabled);

		// Item signals
		interactableObject.ItemHovered += OnItemHovered;
		interactableObject.ItemInteracted += OnItemInteracted;
		interactableObject.ItemUsed += OnItemUsed;

		// Item position, and rotation
		interactableObject.Position = position;
		interactableObject.Rotation = rotation;

		GetNode<Node>("Interactables").AddChild(interactableObject);
	}
}
