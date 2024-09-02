using Godot;
using System;
using System.Numerics;

[Tool]
[GlobalClass]
public partial class InteractableObject : Node3D
{
	[Signal]
	public delegate void ItemHoveredEventHandler(InteractableObjectType item);
	[Signal]
	public delegate void ItemInteractedEventHandler(InteractableObjectType type);

	[Export]
	public String objectName;
	[Export(PropertyHint.MultilineText)]
	public String objectDesc;
	[Export(PropertyHint.Enum)]
	public InteractableObjectType objectType;

	[ExportGroup("Item Setup")]
	[Export]
	private CollisionShape3D interactionBox;

	[ExportGroup("Item Settings")]
	[Export]
	public Boolean isInteractable {get; set;} = true;
	[Export]
	public Godot.Vector3 interactionBoxScale {get; set;} = new Godot.Vector3(3, 3, 3);

	public enum InteractableObjectType
	{
		None,
		Random,
		RandomTableItem,
		Cherry,
		CherrySpoiled,
		Pills,
		MedicalPills,
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			interactionBox.Scale = interactionBoxScale;
		}
	}
}
