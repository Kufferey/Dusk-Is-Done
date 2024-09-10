using Godot;
using System;

public partial class GameState : Node3D
{
	[Signal]
	public delegate void NewDayEventHandler();

	public static int currentDay;

	public static int playerScore = 0;

	[Export]
	public InteractableObject playerCurrentHoveredObject;
	[Export]
	public InteractableObject playerCurrentHeldItem;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
		
	}

	public void AddTableItem(InteractableObject.InteractableObjectType type)
	{
		Node tableItem = InteractableObjectManager.interactableObjectPrefabs[type].Instantiate();
		GetNode<Node>("Interactables").AddChild(tableItem);
	}
}
