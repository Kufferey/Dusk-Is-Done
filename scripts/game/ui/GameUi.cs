using Godot;

public partial class GameUi : CanvasLayer
{
	public Godot.Collections.Array<CompressedTexture2D> interactionIcons = new Godot.Collections.Array<CompressedTexture2D>
	{
		{null},
		{ResourceLoader.Load<CompressedTexture2D>("res://assets/images/icons/interaction/interactionicon-pregrab.png")}
	};

	public enum InteractionIconsEnum
	{
		None,
		Normal,
	}

	public static InteractionIconsEnum interactionIcon = InteractionIconsEnum.None;


	[Export]
	public Control interactionUiContainer {get; set;}

	[Export]
	public Label textBox {get; set;}
	[Export]
	public TextureRect iconBox {get; set;}

	[Export]
	public TextureProgressBar healthBar {get; set;}

	public float healthBarTransparency = 1F;
	private Color healthBarDefaultColor; // HEX: 57ff60

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		healthBarDefaultColor = new Color(0.341F, 1F, 0.376F, (float)((healthBar.Value - healthBar.MinValue) / (healthBar.MaxValue - healthBar.MinValue)));
		healthBar.TintProgress = healthBarDefaultColor;
		healthBar.Value = Player.playerHealth;
	}

	public void ChangeUi(InteractionIconsEnum interactionIcon, string text)
	{
		iconBox.Texture = interactionIcons[(int)interactionIcon];
		textBox.Text = text;
	}
}
