using Godot;
using System;

public partial class WorldMusic : Node
{
	[Export] private AudioStreamPlayer audioStreamPlayer;
	[Export] private Timer timer;

	private Godot.Collections.Dictionary<string, AudioStream> music = new Godot.Collections.Dictionary<string, AudioStream>
	{
		{"normal", ResourceLoader.Load<AudioStream>("res://assets/music/ambience_songs/fighting_chance.ogg")},
		{"winter", ResourceLoader.Load<AudioStream>("res://assets/music/ambience_songs/fighting_chance.ogg")},
	};

    public override void _Process(double delta)
    {
        GD.Print(timer.TimeLeft);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		timer.Timeout += () =>
		{
			AudioStream audioStream = music["normal"];
			switch (Game.CurrentSeason)
			{
				case Seasons.SeasonType.Fall:
				case Seasons.SeasonType.Spring: audioStream = music["normal"]; break;

				case Seasons.SeasonType.Winter: audioStream = music["winter"]; break;

				default: break;
			}

			audioStreamPlayer.Stream = audioStream;
			audioStreamPlayer.Play();

			timer.Start();
		};
	}
}
