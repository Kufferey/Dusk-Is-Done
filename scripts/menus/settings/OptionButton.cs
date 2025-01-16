using Godot;
using System;

[Tool]
public partial class OptionButton : Node
{
	public enum Types{Boolean, Int, Float, MultiSelect}

	[Export(PropertyHint.MultilineText)]
	public string settingText;

	[Export]
	public Types type {get; set;}

	[Export]
	public Label textNode;

	[ExportSubgroup("Boolean Settings")]
	[Export]
	public bool valueBoolean;
	[ExportSubgroup("Int Settings")]
	[Export]
	public int normalValueInt;
	[Export]
	public int minValueInt;
	[Export]
	public int maxValueInt;
	[ExportSubgroup("Float Settings")]
	[Export]
	public int normalValueFot;
	[Export]
	public int minValueFot;
	[Export]
	public int maxValueFot;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			textNode.Text = settingText;
		}
	}
}
