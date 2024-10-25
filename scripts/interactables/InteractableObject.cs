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
	[Export] public AudioStream objectInteractedSound;
	[Export] public bool playerInteractedSoundOnce;
	[Export] public bool hasPlayedInteractedSound;
	[Export] public AudioStream objectUsedSound;

	[ExportGroup("Item Settings")]
	[Export] public bool IsInteractable {get; set;} = true;
	[Export] public bool IsHolding {get; set;} = false;
	[Export] public Godot.Vector3 interactionBoxScale = new Godot.Vector3(1.5F, 1.5F, 1.5F);
	[Export] public Godot.Vector3 holdingScale = new Vector3(1.5F, 1.5F, 1.5F);
	[Export] public Godot.Vector3 holdingOffset;

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

	private CollisionShape3D interactionBox;

    public override void _Ready()
    {
		if (interactionBox == null) interactionBox = GetNode<CollisionShape3D>("Area3D/CollisionShape3D");

		ItemHovered += OnItemHovered;
		ItemInteracted += OnItemInteracted;
		ItemUsed += OnItemUsed;
    }

    public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			if (interactionBox == null) interactionBox = GetNode<CollisionShape3D>("Area3D/CollisionShape3D");
			interactionBox.Scale = interactionBoxScale;
		}
	}

	public void PlaySound(bool condition, AudioStream sound)
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
		
		switch (type)
		{
			case InteractableObjectType.Pills:

			// Code for Pills

			break;

			case InteractableObjectType.Bandage:

			// Code for Bandage

			break;

			case InteractableObjectType.MedicalPills:

			// Code for MedicalPills

			break;

			case InteractableObjectType.MedicalKit:

			// Code for MedicalKit

			break;

			case InteractableObjectType.Notebook:
			
			// Code for Notebook

			break;

			default:
			break;
		}
	}
}
