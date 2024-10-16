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

	public override void _Ready()
	{
		EnterGame();
	}

	public override void _Process(double delta)
	{
		
	}

	public void EnterGame()
	{

	}

	public void ExitGame()
	{

	}

    public void PlayerZoomCamera(float from, float to, Tween.EaseType easeType, bool useCameraSens, float duration = 0.3F)
	{
		if (GetNode<Node3D>("Player/Player").HasMethod("ZoomCamera"))
		{
			GetNode<Node3D>("Player/Player").Callv(Player.MethodName.ZoomCamera, new Godot.Collections.Array{
				from, to, (int)easeType, useCameraSens, duration
			});	
		}
	}

	public void PlayerZoomAndLockCamera(float from, float to, Vector3 position, Tween.EaseType easeType, bool useSens, bool lockCam, float duration = 0.3F)
	{
		if (GetNode<Node3D>("Player/Player").HasMethod("ZoomAndLockCamera"))
		{
			GetNode<Node3D>("Player/Player").Callv(Player.MethodName.ZoomAndLockCamera, new Godot.Collections.Array{
				from, to, position, (int)easeType, useSens, lockCam, duration
			});	
		}
	}

	public void StoreTableItem(InteractableObject.InteractableObjectType type)
	{
		// if ( > (int)(tableItemsAllowedRow + tableItemsAllowedCollumn)) return;


	}
}
