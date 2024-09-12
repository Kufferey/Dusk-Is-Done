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
		ZoomCamera(GetNode<Camera3D>("Player/Player/CameraNeck/Camera3D").Fov, GetNode<Camera3D>("Player/Player/CameraNeck/Camera3D").Fov / 8F, Tween.EaseType.InOut, true, 6.5F);
	}

	public void ZoomCamera(float from, float to, Tween.EaseType easeType, bool useCameraSens, float duration = 0.3F)
	{
		if (GetNode<Node3D>("Player/Player").HasMethod("ZoomCameraWithSens") && GetNode<Node3D>("Player/Player").HasMethod("ZoomCamera"))
		{
			if (useCameraSens)
			{
				GetNode<Node3D>("Player/Player").Callv(Player.MethodName.ZoomCameraWithSens, new Godot.Collections.Array{
					from, to, (int)easeType, duration
				});	
			}
			else
			{
				GetNode<Node3D>("Player/Player").Callv(Player.MethodName.ZoomCamera, new Godot.Collections.Array{
					from, to, (int)easeType, duration
				});	
			}
		}
	}

	public override void _Process(double delta)
	{
		
	}

	public void AddTableItem(InteractableObject.InteractableObjectType type)
	{
		Node tableItem = InteractableObjectManager.interactableObjectPrefabs[type].Instantiate();
		GetNode<Node>("Interactables").AddChild(tableItem);
	}
}
