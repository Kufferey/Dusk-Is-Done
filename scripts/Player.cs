using Godot;
using System;

public partial class Player : Node3D
{
	//TODO : Make the "ZoomCamera" a signal.

	public static float playerCameraTurnSpeed = 2F;

	[Export]
	public Node3D playerCameraNode {get; set;}
	[Export]
	public Camera3D playerCamera {get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Globals.canMoveCamera)
		{
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseMotion mouseMovement && Globals.canMoveCamera)
		{
			RotateY(-Mathf.DegToRad(mouseMovement.Relative.X * playerCameraTurnSpeed));
			playerCameraNode.RotateX(-Mathf.DegToRad(mouseMovement.Relative.Y * playerCameraTurnSpeed));
			playerCameraNode.Rotation =  new Vector3(
				Math.Clamp(playerCameraNode.Rotation.X, Mathf.DegToRad(-85F), Mathf.DegToRad(85F)),
				playerCameraNode.Rotation.Y,
				playerCameraNode.Rotation.Z
			);
		}
    }

	public void ZoomCamera(float from, float to, Tween.EaseType easeType, float duration = 0.3F)
	{
		Tween tween = GetTree().CreateTween();
		tween.SetEase(easeType);
		tween.TweenProperty(playerCamera, "fov", to, duration);
	}

	public void ZoomCameraWithSens(float from, float to, Tween.EaseType easeType, float duration = 0.3F)
	{
		ZoomCamera(from, to, easeType, duration);
		playerCameraTurnSpeed = Mathf.Lerp(playerCameraTurnSpeed, Mathf.Abs(
			(playerCameraTurnSpeed - to / 25)), 1.0F + duration);
	}

	public float GetCameraZoom()
	{
		return playerCamera.Fov;
	}
}
