using System;
using Godot;

[Tool]
[GlobalClass]
public partial class InteractableObject : Node3D
{
	[Signal] public delegate void ItemHoveredEventHandler(InteractableObject.InteractableObjectType type);
	[Signal] public delegate void ItemInteractedEventHandler(InteractableObject.InteractableObjectType type);
	[Signal] public delegate void ItemUsedEventHandler(InteractableObject.InteractableObjectType type);

	[Export] public string ObjectName {get; set;}
	[Export(PropertyHint.MultilineText)] public string ObjectDesc {get; set;}
	[Export(PropertyHint.Enum)] public InteractableObjectType ObjectType {get; set;}

	[ExportGroup("Item Audio")]
	[Export] public AudioStream objectInteractedSound;
	[Export] public bool playerInteractedSoundOnce;
	[Export] public bool hasPlayedInteractedSound;
	[Export] public AudioStream objectUsedSound;

	[ExportGroup("Item Settings")]
	[Export] public bool IsInteractable {get; set;} = true;
	[Export] public bool IsStatic {get; set;} = false;
	[Export] public bool IsHolding {get; set;} = false;
	[Export] public Godot.Vector3 interactionBoxScale = new Godot.Vector3(1.5F, 1.5F, 1.5F);
	[Export] public Godot.Vector3 holdingScale = new Vector3(1.5F, 1.5F, 1.5F);

	public enum InteractableObjectType
	{
		None,
		Test,
		Random,
		RandomTableItem,

		Cherry,
		CherrySpoiled,

		Pills,
		Bandage,
		MedicalPills,
		MedicalKit,
		
		Notebook,
	}

	private CollisionShape3D _interactionBox;

    public override void _Ready()
    {
		if (_interactionBox == null) _interactionBox = GetNode<CollisionShape3D>("Area3D/CollisionShape3D");

		ItemHovered += OnItemHovered;
		ItemInteracted += OnItemInteracted;
		ItemUsed += OnItemUsed;
    }

    public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			if (_interactionBox == null) _interactionBox = GetNode<CollisionShape3D>("Area3D/CollisionShape3D");
			_interactionBox.Scale = interactionBoxScale;
		}
	}

	private void PlaySound(bool condition, AudioStream sound)
	{
		if (condition)
		{
			if (playerInteractedSoundOnce && hasPlayedInteractedSound) return;

			if ((bool)(playerInteractedSoundOnce && !hasPlayedInteractedSound) || !playerInteractedSoundOnce)
			{
				AudioStreamPlayer audioStreamPlayer = new AudioStreamPlayer();
				audioStreamPlayer.Stream = sound;
				AddChild(audioStreamPlayer);
				audioStreamPlayer.Play();
				audioStreamPlayer.Finished += () => {
					audioStreamPlayer.QueueFree();
				};
			}
		}
	}

	public void OnItemHovered(InteractableObject.InteractableObjectType type)
	{
		
	}

	public void OnItemInteracted(InteractableObject.InteractableObjectType type)
	{
		IsHolding = true;

		// Scalling
		Scale = new Vector3(
			holdingScale.X,
			holdingScale.Y,
			holdingScale.Z
		);

        // Audio
        PlaySound(objectInteractedSound != null, objectInteractedSound);
	}

	public void OnItemUsed(InteractableObject.InteractableObjectType type)
	{
		PlaySound(objectUsedSound != null, objectUsedSound);
	}
}
