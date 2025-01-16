using Godot;
using System;

public partial class MainMenu : Control
{
	[Export] public Label version;

	[Export] public Button button1;
	[Export] public Button button2;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		version.Text = "v" + Globals.gameVersion;

		button1.Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scenes/game_state.tscn");
		};
		button2.Pressed += () =>
		{
			GetTree().Quit();
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
