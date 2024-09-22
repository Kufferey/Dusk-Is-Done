using Godot;
using System;

[Tool]
[GlobalClass]
public partial class InteractableObject : Node3D
{
	[Signal]
	public delegate void ItemHoveredEventHandler(InteractableObjectType type);
	[Signal]
	public delegate void ItemInteractedEventHandler(InteractableObjectType type);

	[Export]
	public String objectName;
	[Export(PropertyHint.MultilineText)]
	public String objectDesc;
	[Export(PropertyHint.Enum)]
	public InteractableObjectType objectType;

	[ExportGroup("Item Settings")]
	[Export]
	public Boolean isInteractable {get; set;} = true;
	[Export]
	public Boolean isHolding {get; set;} = false;
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
    }

    public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			if (interactionBox == null) interactionBox = GetNode<CollisionShape3D>("Area3D/CollisionShape3D");
			interactionBox.Scale = interactionBoxScale;
		}
	}
}
