using Godot;
using System;

[Tool]
[GlobalClass]
public partial class InteractableObject : Node3D
{
	[Signal]
	public delegate void ItemHoveredEventHandler(InteractableObject interactableObject);
	[Signal]
	public delegate void ItemInteractedEventHandler(InteractableObject interactableObject);
	[Signal]
	public delegate void ItemUsedEventHandler(InteractableObject interactableObject);

	[Export]
	public string objectName;
	[Export(PropertyHint.MultilineText)]
	public string objectDesc;
	[Export(PropertyHint.Enum)]
	public InteractableObjectType objectType;
	[Export]
	public AudioStream objectInteractedSound;
	[Export]
	public AudioStream objectUsedSound;

	[ExportGroup("Item Settings")]
	[Export]
	public bool isInteractable {get; set;} = true;
	[Export]
	public bool isHolding {get; set;} = false;
	[Export]
	public Godot.Vector3 interactionBoxScale {get; set;} = new Godot.Vector3(3, 3, 3);
	[Export]
	public Godot.Vector3 holdingScale {get; set;} = new Vector3(2, 2, 2);

	public enum InteractableObjectType
	{
		None,
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
			AudioStreamPlayer audioStreamPlayer = new AudioStreamPlayer();
			audioStreamPlayer.Stream = sound;
			AddChild(audioStreamPlayer);
			audioStreamPlayer.Play();
			audioStreamPlayer.Finished += () => {
				audioStreamPlayer.QueueFree();
			};
		}
	}

	public void OnItemHovered(InteractableObject interactableObject)
	{
		
	}

	public void OnItemInteracted(InteractableObject interactableObject)
	{
		isHolding = true;

		// Scalling
		Scale = new Vector3(
			holdingScale.X,
			holdingScale.Y,
			holdingScale.Z
		);
		
		// Audio
		PlaySound(objectInteractedSound != null, objectInteractedSound);
	}

	public void OnItemUsed(InteractableObject interactableObject)
	{
		PlaySound(objectUsedSound != null, objectUsedSound);
		
		switch (interactableObject.objectType)
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
