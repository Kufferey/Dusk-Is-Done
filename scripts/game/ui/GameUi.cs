using Godot;

public partial class GameUi : CanvasLayer
{
	public Godot.Collections.Array<CompressedTexture2D> interactionIconTextures = new Godot.Collections.Array<CompressedTexture2D>
	{
		{null},
		{ResourceLoader.Load<CompressedTexture2D>("res://assets/images/icons/interaction/interactionicon-pregrab.png")}
	};

	public enum InteractionIcons
	{
		None,
		Normal,
	}

	public static InteractionIcons interactionIcon = InteractionIcons.None;

	[Export] public Control InteractionUiContainer {get; set;}

	[Export] public Label TextBox {get; set;}
	[Export] public TextureRect IconBox {get; set;}

	[Export] public TextureProgressBar HealthBar {get; set;}

	private Color healthBarDefaultColor; // HEX: 57ff60

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (HealthBar.Value != Player.playerHealth)
		{
			healthBarDefaultColor = new Color(0.341F, 1F, 0.376F, (float)((HealthBar.Value - HealthBar.MinValue) / (HealthBar.MaxValue - HealthBar.MinValue)));
			HealthBar.TintProgress = healthBarDefaultColor;
			HealthBar.Value = Mathf.Lerp(HealthBar.Value, Player.playerHealth, 0.1F);
		}

		GetNode<Label>("Main/FPS/Label").Text = "FPS: " + Engine.GetFramesPerSecond().ToString();
	}

	public void ChangeUi(InteractionIcons interactionIcon, string text)
	{
		IconBox.Texture = interactionIconTextures[(int)interactionIcon];
		TextBox.Text = text;
	}
}
