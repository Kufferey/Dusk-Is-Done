using Godot;

public partial class Lock : Control
{
	[Export(PropertyHint.MultilineText)] public string LockText {get; set;}

	private ColorRect ToolTip {get; set;}
	
	private Label label;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ToolTip = GetNode<ColorRect>("ColorRect");
		label = GetNode<Label>("ColorRect/Label");

		label.Text = LockText;

		MouseEntered += () =>
		{
			ToolTip.Show();
		};
		MouseExited += () =>
		{
			ToolTip.Hide();
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ToolTip.Position = new Vector2(
			GetLocalMousePosition().X + 55,
			GetLocalMousePosition().Y
		);
	}
}
