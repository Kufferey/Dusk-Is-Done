using Godot;
using System;

public partial class InteractableObjectManager : Node
{
	// List of all items in the game.
	
    public static Godot.Collections.Dictionary<InteractableObject.InteractableObjectType, PackedScene> interactableObjectPrefabs {get; set;} = new Godot.Collections.Dictionary<InteractableObject.InteractableObjectType, PackedScene>
	{
		{InteractableObject.InteractableObjectType.Test,          (Godot.PackedScene)ResourceLoader.Load<PackedScene>("res://scenes/interactables/table_items/test.tscn")},

		{InteractableObject.InteractableObjectType.Cherry,        (Godot.PackedScene)ResourceLoader.Load<PackedScene>("res://scenes/interactables/cherry/cherry.tscn")},
		{InteractableObject.InteractableObjectType.CherrySpoiled, (Godot.PackedScene)ResourceLoader.Load<PackedScene>("res://scenes/interactables/cherry/cherry_spoiled.tscn")},
		
		{InteractableObject.InteractableObjectType.Pills,         (Godot.PackedScene)ResourceLoader.Load<PackedScene>("res://scenes/interactables/table_items/test.tscn")},
		{InteractableObject.InteractableObjectType.Bandage,       (Godot.PackedScene)ResourceLoader.Load<PackedScene>("res://scenes/interactables/table_items/test.tscn")},
		{InteractableObject.InteractableObjectType.MedicalPills,  (Godot.PackedScene)ResourceLoader.Load<PackedScene>("res://scenes/interactables/table_items/test.tscn")},
		{InteractableObject.InteractableObjectType.MedicalKit,    (Godot.PackedScene)ResourceLoader.Load<PackedScene>("res://scenes/interactables/table_items/test.tscn")},
		
		{InteractableObject.InteractableObjectType.Notebook,      (Godot.PackedScene)ResourceLoader.Load<PackedScene>("res://scenes/interactables/table_items/test.tscn")},
	};

	// Add a interaction item to "interactableObjectPrefabs".
    public static void AddInteractableItemToPrefabs(InteractableObject.InteractableObjectType type, String scenePath)
	{
		interactableObjectPrefabs.Add((InteractableObject.InteractableObjectType)type, (Godot.PackedScene)ResourceLoader.Load<PackedScene>(scenePath));
	}
}