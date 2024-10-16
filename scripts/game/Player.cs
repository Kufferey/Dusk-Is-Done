using Godot;
using System;

public partial class Player : Node3D
{
	public static float playerCameraTurnSpeed = ((Godot.Collections.Dictionary<string, float>)Globals.settings["sens"])["value"];
	public static float playerCameraTurnSpeedStart = playerCameraTurnSpeed;

	public static bool playerCanMoveCamera = true;
	public static bool playerCanInteract = true;
	public static bool playerCanHoldItem = true;

	public static InteractableObject playerCurrentHoveredObject;
	public static InteractableObject playerCurrentHeldItem;

	public static float playerHealth = 10.0F;
	public static int playerScore;

	[Export]
	public float playerItemSway = 7F;

	[Export]
	public Node3D playerCameraNode {get; set;}
	[Export]
	public Camera3D playerCamera {get; set;}
	[Export]
	public RayCast3D playerRaycast {get; set;}
	[Export]
	public Node3D playerHand {get; set;}

	[Export]
	public GameUi playerUi {get; set;}

	public override void _Ready()
	{
		if (playerCanMoveCamera)
		{
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
		}
	}

	public override void _Process(double delta)
	{
		playerCurrentHoveredObject = GetHoveredItem();

		if (playerCurrentHeldItem != null)
		{
			playerCurrentHeldItem.Position = new Vector3(
				Mathf.Lerp(playerCurrentHeldItem.Position.X, playerHand.GlobalPosition.X, playerItemSway * (float)delta),
				Mathf.Lerp(playerCurrentHeldItem.Position.Y, playerHand.GlobalPosition.Y, playerItemSway * (float)delta),
				Mathf.Lerp(playerCurrentHeldItem.Position.Z, playerHand.GlobalPosition.Z, playerItemSway * (float)delta)
			);
			playerCurrentHeldItem.Rotation = playerHand.GlobalRotation;
		}
	}

    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseMotion mouseMovement && playerCanMoveCamera)
		{
			RotateY(-Mathf.DegToRad(mouseMovement.Relative.X * playerCameraTurnSpeed));
			playerCameraNode.RotateX(-Mathf.DegToRad(mouseMovement.Relative.Y * playerCameraTurnSpeed));
			playerCameraNode.Rotation = new Vector3(
				Math.Clamp(playerCameraNode.Rotation.X, Mathf.DegToRad(-85F), Mathf.DegToRad(85F)),
				playerCameraNode.Rotation.Y,
				playerCameraNode.Rotation.Z
			);
		}

		if (@event is InputEventKey && @event.IsActionPressed("interact") && playerCanHoldItem && playerCurrentHeldItem == null)
		{
			SetHoveredToHeld();
			playerCurrentHeldItem.EmitSignal(InteractableObject.SignalName.ItemInteracted, (InteractableObject)playerCurrentHeldItem);
		}
    }

	public InteractableObject GetHoveredItem()
	{
		if (!playerCanHoldItem || !playerCanInteract || playerCurrentHeldItem != null) return null;

		if (playerRaycast.IsColliding())
		{
			GodotObject hoveredObject = playerRaycast.GetCollider();
			if (hoveredObject is Node3D item) if (item.GetParent() is InteractableObject interactableItem)
			{
				interactableItem.EmitSignal(InteractableObject.SignalName.ItemHovered, (int)interactableItem.objectType);

				return interactableItem;
			}
		}
		return null;
	}

	public void SetHoveredToHeld()
	{
		playerCurrentHeldItem = playerCurrentHoveredObject;
		playerCanHoldItem = false;
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
			playerCanMoveCamera = false;
			
			playerCameraNode.LookAt(position);
		}
	}

	public float GetCameraZoom()
	{
		return playerCamera.Fov;
	}
}
