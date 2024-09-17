using Godot;
using System;

public partial class Player : Node3D
{
	public static float playerCameraTurnSpeed = ((Godot.Collections.Dictionary<string, float>)Globals.settings["sens"])["value"];
	public static float playerCameraTurnSpeedStart = playerCameraTurnSpeed;

	[Export]
	public Node3D playerCameraNode {get; set;}
	[Export]
	public Camera3D playerCamera {get; set;}

	public override void _Ready()
	{
		if (Globals.playerCanMoveCamera)
		{
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
		}
	}

	public override void _Process(double delta)
	{
		
	}

    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseMotion mouseMovement && Globals.playerCanMoveCamera)
		{
			RotateY(-Mathf.DegToRad(mouseMovement.Relative.X * playerCameraTurnSpeed));
			playerCameraNode.RotateX(-Mathf.DegToRad(mouseMovement.Relative.Y * playerCameraTurnSpeed));
			playerCameraNode.Rotation = new Vector3(
				Math.Clamp(playerCameraNode.Rotation.X, Mathf.DegToRad(-85F), Mathf.DegToRad(85F)),
				playerCameraNode.Rotation.Y,
				playerCameraNode.Rotation.Z
			);
		}
    }

	public void ZoomCamera(float from, float to, Tween.EaseType easeType, bool useSens, float duration = 0.3F)
	{
		Tween tween = GetTree().CreateTween();
		tween.SetEase(easeType);
		tween.TweenProperty(playerCamera, "fov", to, duration);

		if (useSens)
		{
			playerCameraTurnSpeed = Mathf.Abs(Mathf.Lerp(playerCameraTurnSpeed, Mathf.Abs(
				playerCameraTurnSpeed - to / -25F), 1.0F + duration));

			if (playerCameraTurnSpeed > playerCameraTurnSpeedStart) playerCameraTurnSpeed = playerCameraTurnSpeed / to;
		}
	}

	public void ZoomAndLockCamera(float from, float to, Vector3 position, Tween.EaseType easeType, bool useSens, bool lockCam, float duration = 0.3F)
	{
		ZoomCamera(from, to, easeType, useSens, duration);
		if (lockCam)
		{
			Globals.playerCanMoveCamera = false;
			
			playerCameraNode.LookAt(position);
		}
	}

	public float GetCameraZoom()
	{
		return playerCamera.Fov;
	}
}
