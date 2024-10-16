using Godot;
using System;

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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ChangeUi(InteractionIconsEnum interactionIcon, string text)
	{
		iconBox.Texture = interactionIcons[(int)interactionIcon];
		textBox.Text = text;
	}
}
