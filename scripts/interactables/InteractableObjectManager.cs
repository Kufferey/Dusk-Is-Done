using Godot;
using System;

[GlobalClass]
public partial class InteractableObjectManager : Node
{
	// List of all items in the game.
    public Godot.Collections.Dictionary<InteractableObject.InteractableObjectType, PackedScene> interactableObjectPrefabs {get; set;} = new Godot.Collections.Dictionary<InteractableObject.InteractableObjectType, PackedScene>
	{
		// {InteractableObject.InteractableObjectType.Cherry,        (Godot.PackedScene)ResourceLoader.Load<PackedScene>("")},
		// {InteractableObject.InteractableObjectType.CherrySpoiled, (Godot.PackedScene)ResourceLoader.Load<PackedScene>("")},
		// {InteractableObject.InteractableObjectType.MedicalPills,  (Godot.PackedScene)ResourceLoader.Load<PackedScene>("")},
		// {InteractableObject.InteractableObjectType.Pills,         (Godot.PackedScene)ResourceLoader.Load<PackedScene>("")},
	};

	// Add a interaction item to "interactableObjectPrefabs".
    public void AddInteractableItemsToPrefabs(InteractableObject.InteractableObjectType type, String scenePath)
	{
		interactableObjectPrefabs.Add((InteractableObject.InteractableObjectType)type, (Godot.PackedScene)ResourceLoader.Load<PackedScene>(scenePath));
	}
}