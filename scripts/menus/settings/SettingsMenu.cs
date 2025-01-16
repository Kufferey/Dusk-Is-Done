using Godot;
using System;
using System.Diagnostics.Contracts;

public partial class SettingsMenu : Control
{
	[Export] public Button graphicsButton;
	[Export] public Button displayButton;
	[Export] public Button gameplayButton;
	[Export] public Button audioButton;
	[Export] public Button backButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		graphicsButton.Pressed += () =>
		{
			SwitchMenus("Graphics");
		};

		displayButton.Pressed += () =>
		{
			SwitchMenus("Display");
		};

		gameplayButton.Pressed += () =>
		{
			SwitchMenus("Gameplay");
		};

		audioButton.Pressed += () =>
		{
			SwitchMenus("Audio");
		};

		backButton.Pressed += () =>
		{
			QueueFree();
		};
		SwitchMenus("Graphics");
	}

	public void SwitchMenus(string name)
	{
		foreach (Control controlNode in GetNode("Contain/SubSettings/").GetChildren())
		{
			if (controlNode is Panel) continue;
			if (controlNode.Name != name)
			{
				controlNode.Hide();
				continue;
			}
			controlNode.Show();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
