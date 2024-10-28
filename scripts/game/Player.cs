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

	[Export] public Node3D PlayerCameraNode {get; set;}
	[Export] public Camera3D PlayerCamera {get; set;}
	[Export] public RayCast3D PlayerRaycast {get; set;}
	[Export] public Node3D PlayerHand {get; set;}

	[Export] public GameUi PlayerUi {get; set;}

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

			playerCurrentHeldItem.Rotation = PlayerHand.GlobalRotation;
			Vector3 newOffset = PlayerHand.GlobalPosition;

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
			PlayerCameraNode.RotateX(-Mathf.DegToRad(mouseMovement.Relative.Y * playerCameraTurnSpeed));
			PlayerCameraNode.Rotation = new Vector3(
				Math.Clamp(PlayerCameraNode.Rotation.X, Mathf.DegToRad(-85F), Mathf.DegToRad(85F)),
				PlayerCameraNode.Rotation.Y,
				PlayerCameraNode.Rotation.Z
			);
		}

		if (@event is InputEventKey && @event.IsActionPressed("interact") && playerCanHoldItem && playerCurrentHoveredObject.IsInteractable && !IsPlayerHoldingItem())
		{
			SetHoveredToHeld();
			playerCurrentHeldItem.EmitSignal(InteractableObject.SignalName.ItemInteracted, (InteractableObject)playerCurrentHeldItem);
		}
    }

	public void PlayerKill()
	{
		RemoveCurrentHeldItem();
		playerHealth = playerMinHealth;
	}

	public static bool IsPlayerDead()
	{
		if (playerHealth > playerMinHealth) return false;
		return true;
	}

	public static bool IsPlayerHoldingItemType(InteractableObject.InteractableObjectType type)
	{
		if (IsPlayerHoldingItem() && (InteractableObject.InteractableObjectType)playerCurrentHeldItem.ObjectType == type) return true;
		return false;
	}

	public static bool IsPlayerHoldingItem()
	{
		if (playerCurrentHeldItem != null) return true;
		return false;
	}

	public InteractableObject GetHoveredItem()
	{
		if (!playerCanHoldItem || !playerCanInteract || playerCurrentHeldItem != null || !PlayerRaycast.IsColliding()) {PlayerUi?.ChangeUi(GameUi.InteractionIconsEnum.None, ""); return null;}
		
		GodotObject hoveredObject = PlayerRaycast.GetCollider();
		if (hoveredObject is Node3D item) if (item?.GetParent() is InteractableObject interactableItem)
		{
			interactableItem.EmitSignal(InteractableObject.SignalName.ItemHovered, (int)interactableItem.ObjectType);
			PlayerUi?.ChangeUi(GameUi.InteractionIconsEnum.Normal, "Press [E] to Pickup: " + interactableItem.ObjectName?.ToString());

			return interactableItem;
		}

		return null;
	}

	public void UiToggle() => PlayerUi.Visible = !PlayerUi.Visible;
	
	public void SetHoveredToHeld()
	{
		playerCurrentHeldItem = playerCurrentHoveredObject;
		playerCurrentHeldItem.IsHolding = true;
		playerCanHoldItem = false;
	}

	public void RemoveCurrentHeldItem()
	{
		if (playerCurrentHeldItem != null) playerCurrentHeldItem.QueueFree();
		playerCurrentHeldItem = null;

		playerCanHoldItem = true;
		playerCanInteract = true;
	}

	public void ZoomCamera(float from, float to, Tween.EaseType easeType,
	bool useSens, float duration = 0.3F)
	{
		Tween tween = GetTree().CreateTween();
		tween.SetEase(easeType);
		tween.TweenProperty(PlayerCamera, "fov", to, duration);
		if (useSens)
		{
			playerCameraTurnSpeed = Mathf.Abs(Mathf.Lerp(playerCameraTurnSpeed, Mathf.Abs(
				playerCameraTurnSpeed - to / -25F), 1.0F + duration));

			if (playerCameraTurnSpeed > playerCameraTurnSpeedStart) playerCameraTurnSpeed = playerCameraTurnSpeed / to;
		}
	}

	public void ZoomAndLockCamera(float from, float to, Vector3 position,
	Tween.EaseType easeType, bool useSens, bool lockCam, float duration = 0.3F)
	{
		ZoomCamera(from, to, easeType, useSens, duration);
		if (lockCam)
		{
			playerCanMoveCamera = false;
			
			PlayerCameraNode.LookAt(position);
		}
	}

	public float GetCameraZoom() {return (float)PlayerCamera.Fov;}
}
