using Godot;
using System;

public partial class CherrySpawnPoint : Node3D
{
	[Export] public int maxCherriesAllowed = 4;
	[Export] public bool canCherrySpawn = true;

	[Export] public int currentCherriesInArea = 0;

	[Export] private Area3D Area3D {get; set;}

    public override void _Ready()
    {
        Area3D.AreaEntered += OnAreaEntered;
        Area3D.AreaExited += OnAreaExited;
    }

	private void OnAreaEntered(Area3D area)
	{
		if (area.GetParent() is InteractableObject item) if (item.ObjectType == InteractableObject.InteractableObjectType.Cherry || item.ObjectType == InteractableObject.InteractableObjectType.CherrySpoiled)
		{
			if (currentCherriesInArea >= maxCherriesAllowed) {canCherrySpawn = false; return;};
			GD.Print("Cherry Spawned");
			currentCherriesInArea++;
		}
	}

	private void OnAreaExited(Area3D area)
	{
		if (area.GetParent() is InteractableObject item) if (item.ObjectType == InteractableObject.InteractableObjectType.Cherry || item.ObjectType == InteractableObject.InteractableObjectType.CherrySpoiled)
		{
			currentCherriesInArea--;
			GD.Print("Cherry Removed");
			canCherrySpawn = true;
		}
	}
}
