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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
