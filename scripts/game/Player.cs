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

	public static float playerMinHealth = 0.0F;
	public static float playerMaxHealth = 100.0F;
	public static float playerHealth = playerMaxHealth;
	public static int playerScore;

	[Export] public float playerItemSway = 7F;

	[Export] public Node3D playerCameraNode {get; set;}
	[Export] public Camera3D playerCamera {get; set;}
	[Export] public RayCast3D playerRaycast {get; set;}
	[Export] public Node3D playerHand {get; set;}

	[Export] public GameUi playerUi {get; set;}

	public override void _Ready()
	{
		if (playerCanMoveCamera)
		{
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
			// DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
	}

	public override void _Process(double delta)
	{
		playerCurrentHoveredObject = GetHoveredItem();

		if (IsPlayerHoldingItem())
		{
			if (playerCurrentHeldItem.Scale != playerCurrentHeldItem.holdingScale) playerCurrentHeldItem.Scale = new Vector3(
				Mathf.Lerp(playerCurrentHeldItem.Scale.X, playerCurrentHeldItem.holdingScale.X, playerItemSway * (float)delta),
				Mathf.Lerp(playerCurrentHeldItem.Scale.Y, playerCurrentHeldItem.holdingScale.Y, playerItemSway * (float)delta),
				Mathf.Lerp(playerCurrentHeldItem.Scale.Z, playerCurrentHeldItem.holdingScale.Z, playerItemSway * (float)delta)
			);
			playerCurrentHeldItem.Rotation = playerHand.GlobalRotation;

			Vector3 newOffset = playerHand.GlobalPosition + playerCurrentHeldItem.holdingOffset;
			playerCurrentHeldItem.Position = new Vector3(
				Mathf.Lerp(playerCurrentHeldItem.Position.X, newOffset.X, playerItemSway * (float)delta),
				Mathf.Lerp(playerCurrentHeldItem.Position.Y, newOffset.Y, playerItemSway * (float)delta),
				Mathf.Lerp(playerCurrentHeldItem.Position.Z, newOffset.Z, playerItemSway * (float)delta)
			);
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

		if (@event is InputEventKey && @event.IsActionPressed("interact") && playerCanHoldItem && playerCurrentHoveredObject.isInteractable && !IsPlayerHoldingItem())
		{
			SetHoveredToHeld();
			playerCurrentHeldItem.EmitSignal(InteractableObject.SignalName.ItemInteracted, (InteractableObject)playerCurrentHeldItem);
		}
    }

	public static bool IsPlayerDead()
	{
		if (playerHealth > playerMinHealth) return false;
		return true;
	}

	public static bool IsPlayerHoldingItemType(InteractableObject.InteractableObjectType type)
	{
		if (IsPlayerHoldingItem() && (InteractableObject.InteractableObjectType)playerCurrentHeldItem.objectType == type) return true;
		return false;
	}

	public static bool IsPlayerHoldingItem()
	{
		if (playerCurrentHeldItem != null) return true;
		return false;
	}

	public InteractableObject GetHoveredItem()
	{
		if (!playerCanHoldItem || !playerCanInteract || playerCurrentHeldItem != null || !playerRaycast.IsColliding()) {playerUi?.ChangeUi(GameUi.InteractionIconsEnum.None, ""); return null;}
		
		GodotObject hoveredObject = playerRaycast.GetCollider();
		if (hoveredObject is Node3D item) if (item?.GetParent() is InteractableObject interactableItem)
		{
			interactableItem.EmitSignal(InteractableObject.SignalName.ItemHovered, (int)interactableItem.objectType);
			playerUi?.ChangeUi(GameUi.InteractionIconsEnum.Normal, "Press [E] to Pickup: " + interactableItem.objectName?.ToString());

			return interactableItem;
		}

		return null;
	}

	public void UiToggle() => playerUi.Visible = !playerUi.Visible;
	
	public void SetHoveredToHeld()
	{
		playerCurrentHeldItem = playerCurrentHoveredObject;
		playerCurrentHeldItem.isHolding = true;
		playerCanHoldItem = false;
	}

	public void RemoveCurrentHeldItem()
	{
		playerCurrentHeldItem.QueueFree();
		playerCurrentHeldItem = null;

		playerCanHoldItem = true;
		playerCanInteract = true;
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

	public float GetCameraZoom() {return (float)playerCamera.Fov;}
	
}
