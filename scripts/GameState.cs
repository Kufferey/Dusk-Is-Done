using Godot;
using System;

public partial class GameState : Node3D
{
	[Signal]
	public delegate void NewDayEventHandler();

	[Export]
	public int playerScore = 0;
	[Export]
	public InteractableObject playerCurrentHoveredObject;
	[Export]
	public InteractableObject playerCurrentHeldItem;
	
	[Export]
	public int currentDay;

	private InteractableObjectManager interactableObjectManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		interactableObjectManager = GetNode<InteractableObjectManager>("/root/InteractableObjectManager");
		GD.Print(interactableObjectManager.interactableObjectPrefabs);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void AddTableItem(InteractableObject.InteractableObjectType type)
	{
		Node tableItem = interactableObjectManager.interactableObjectPrefabs[type].Instantiate();
		GetNode<Node>("Interactables").AddChild(tableItem);
	}
}
