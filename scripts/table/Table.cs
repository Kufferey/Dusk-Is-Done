using Godot;
using System;

public partial class Table : Node3D
{
	public static Godot.Collections.Array<InteractableObject> tableItemsRow = new Godot.Collections.Array<InteractableObject>{};
	public static Godot.Collections.Array<InteractableObject> tableItemsCollumn = new Godot.Collections.Array<InteractableObject>{};
	public static int tableItemsAllowedRow = 6;
	public static int tableItemsAllowedCollumn = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
