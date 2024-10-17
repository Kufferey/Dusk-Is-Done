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
	public static Difficulty.DifficultyTypes difficulty {get; set;}

	[Export]
	public Player player {get; set;}

	public override void _Ready()
	{
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
		// Unload all game assets.
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

	public void AddInteractableObject(InteractableObject.InteractableObjectType type, Vector3 position)
	{

	}
}
