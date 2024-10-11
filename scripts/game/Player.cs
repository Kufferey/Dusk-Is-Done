using Godot;
using System;

public partial class Player : Node3D
{
	public static float playerCameraTurnSpeed = ((Godot.Collections.Dictionary<string, float>)Globals.settings["sens"])["value"];
	public static float playerCameraTurnSpeedStart = playerCameraTurnSpeed;

	public static InteractableObject playerCurrentHoveredObject;
	public static InteractableObject playerCurrentHeldItem;

	public static bool canHoldItem;

	[Export]
	public Node3D playerCameraNode {get; set;}
	[Export]
	public Camera3D playerCamera {get; set;}
	[Export]
	public RayCast3D playerRaycast {get; set;}
	[Export]
	public Node3D playerHand {get; set;}

	public override void _Ready()
	{
		if (Globals.playerCanMoveCamera)
		{
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
		}
	}

	public override void _Process(double delta)
	{
		playerCurrentHoveredObject = GetHoveredItem();

		if (playerCurrentHeldItem != null)
		{
			// TODO: Get off your lazy ass and add lerping.
			playerCurrentHeldItem.Position = playerHand.GlobalPosition;
			playerCurrentHeldItem.Rotation = playerHand.GlobalRotation;
		}
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

	public void UseItem(InteractableObject interactableObject)
	{
		switch (interactableObject.objectType)
		{
			// OTHER:
			case InteractableObject.InteractableObjectType.None:
			break;

			case InteractableObject.InteractableObjectType.Random:
			// Add Rando effect.
			break;

			// MEDICAL:

			case InteractableObject.InteractableObjectType.Pills:
			// Make where it heals player to ~3-10% more health.
			break;

			case InteractableObject.InteractableObjectType.Bandage:
			// Heals around 10-20%
			break;

			case InteractableObject.InteractableObjectType.MedicalPills:
			// Heals 30-70%
			break;

			case InteractableObject.InteractableObjectType.MedicalKit:
			// Always 100%
			break;

			// UTILITY:

			case InteractableObject.InteractableObjectType.Notebook:
			// Use to take notes and draw.
			break;

			default:
			break;
		}
	}

	public InteractableObject GetHoveredItem()
	{
		if (playerRaycast.IsColliding())
		{
			GodotObject hoveredObject = playerRaycast.GetCollider();
			if (hoveredObject is Node3D item)
			{
				if (item.GetParent() is InteractableObject interactableItem)
				{
					return interactableItem;
				}
			}
		}
		return null;
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
