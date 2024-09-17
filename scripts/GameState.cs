using Godot;
using System;

public partial class GameState : Node3D
{
	[Signal]
	public delegate void NewDayEventHandler();

	public static int currentDay;

	public static int playerScore = 0;

	[Export]
	public InteractableObject playerCurrentHoveredObject;
	[Export]
	public InteractableObject playerCurrentHeldItem;

	public override void _Ready()
	{
		Globals.SettingsReload();
	}

	public override void _Process(double delta)
	{
		
	}

	public void ZoomCamera(float from, float to, Tween.EaseType easeType, bool useCameraSens, float duration = 0.3F)
	{
		if (GetNode<Node3D>("Player/Player").HasMethod("ZoomAndLockCamera") && GetNode<Node3D>("Player/Player").HasMethod("ZoomCamera"))
		{
			GetNode<Node3D>("Player/Player").Callv(Player.MethodName.ZoomAndLockCamera, new Godot.Collections.Array{
				from, to, (int)easeType, useCameraSens, duration
			});	
		}
	}

	public void ZoomAndLockCamera(float from, float to, Vector3 position, Tween.EaseType easeType, bool useSens, bool lockCam, float duration = 0.3F)
	{
		if (GetNode<Node3D>("Player/Player").HasMethod("ZoomAndLockCamera"))
		{
			GetNode<Node3D>("Player/Player").Callv(Player.MethodName.ZoomAndLockCamera, new Godot.Collections.Array{
				from, to, position, (int)easeType, useSens, lockCam, duration
			});	
		}
	}

	public void AddTableItem(InteractableObject.InteractableObjectType type)
	{
		Node tableItem = InteractableObjectManager.interactableObjectPrefabs[type].Instantiate();
		GetNode<Node>("Interactables").AddChild(tableItem);
	}
}
