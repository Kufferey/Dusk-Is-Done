using Godot;
using System;

public partial class InteractableObjectManager : Node
{
	// List of all items in the game.
    public static Godot.Collections.Dictionary<InteractableObject.InteractableObjectType, PackedScene> interactableObjectPrefabs {get; set;} = new Godot.Collections.Dictionary<InteractableObject.InteractableObjectType, PackedScene>
	{
		{InteractableObject.InteractableObjectType.Cherry,        (Godot.PackedScene)ResourceLoader.Load<PackedScene>("")},
		{InteractableObject.InteractableObjectType.CherrySpoiled, (Godot.PackedScene)ResourceLoader.Load<PackedScene>("")},
		
		{InteractableObject.InteractableObjectType.Pills,         (Godot.PackedScene)ResourceLoader.Load<PackedScene>("")},
		{InteractableObject.InteractableObjectType.Bandage,       (Godot.PackedScene)ResourceLoader.Load<PackedScene>("")},
		{InteractableObject.InteractableObjectType.MedicalPills,  (Godot.PackedScene)ResourceLoader.Load<PackedScene>("")},
		{InteractableObject.InteractableObjectType.MedicalKit,    (Godot.PackedScene)ResourceLoader.Load<PackedScene>("")},
		
		{InteractableObject.InteractableObjectType.Notebook,      (Godot.PackedScene)ResourceLoader.Load<PackedScene>("")},
	};

	// Add a interaction item to "interactableObjectPrefabs".
    public static void AddInteractableItemToPrefabs(InteractableObject.InteractableObjectType type, String scenePath)
	{
		interactableObjectPrefabs.Add((InteractableObject.InteractableObjectType)type, (Godot.PackedScene)ResourceLoader.Load<PackedScene>(scenePath));
	}
}