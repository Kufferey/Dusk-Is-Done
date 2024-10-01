using Godot;
using System;
using System.IO;

public partial class Radio : Node3D
{
	public Godot.Collections.Dictionary<string, Variant> radioSongs {get; set;} = new Godot.Collections.Dictionary<string, Variant>
	{
		// KEY: Name of song, VALUE: 1: song path, 2: BPM
		{"Song1", new Godot.Collections.Array{{ResourceLoader.Load("res://assets/music/radio/Song 1.ogg")},   {175}}},
		{"Song2", new Godot.Collections.Array{{ResourceLoader.Load("res://assets/music/radio/Song 2.ogg")},   {130}}},
		{"Song3", new Godot.Collections.Array{{ResourceLoader.Load("res://assets/music/radio/Song 3.ogg")},   {215}}},
		{"Song4", new Godot.Collections.Array{{ResourceLoader.Load("res://assets/music/radio/Song 4.ogg")},   {145}}},
		{"Song5", new Godot.Collections.Array{{ResourceLoader.Load("res://assets/music/radio/Song 5.ogg")},   {140}}},
		{"Song6", new Godot.Collections.Array{{ResourceLoader.Load("res://assets/music/radio/Song 6.ogg")},   {155}}},
		{"Song7", new Godot.Collections.Array{{ResourceLoader.Load("res://assets/music/radio/Song 7.ogg")},   {240}}},
		{"Song8", new Godot.Collections.Array{{ResourceLoader.Load("res://assets/music/radio/Song 8.ogg")},   {120}}},
		{"Song9", new Godot.Collections.Array{{ResourceLoader.Load("res://assets/music/radio/Song 9.ogg")},   {120}}},
		{"Song10", new Godot.Collections.Array{{ResourceLoader.Load("res://assets/music/radio/Song 10.ogg")}, {130}}},
	};


	public string lastSongName;
	public string currentSongName;

	private float interval;
	private float timer;

	[Export]
	public AudioStreamPlayer3D audioStreamPlayer3D;
	[Export]
	public int bpm = 120;
	[Export]
	public bool canRepeat;

	[Export]
	public AnimationPlayer animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PickRandomSong(canRepeat);
		audioStreamPlayer3D.Finished += SongFinshed;
		interval = 60.0F / bpm;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (audioStreamPlayer3D.Playing)
		{
			timer += (float)delta;
			if (timer >= interval)
			{
				timer -= interval;
				BeatHit();
			}
		}
	}

	public void BeatHit()
	{
		animationPlayer.Play("beat");
	}

	public void LoadAudio(AudioStream stream)
	{
		audioStreamPlayer3D.Stream = stream;
	}

	public void SongFinshed()
	{
		PickRandomSong(canRepeat);
	}

	private void PickRandomSong(bool canRepeat)
	{
		string songName;
		AudioStream songPath;
		int songBpm;

		int maxSongs = 0;

		for (int song = 1; song < radioSongs.Count + 1; song++)
		{maxSongs = song;}

		int randomSongNumber = GD.RandRange(1, maxSongs);
		songName = ("Song" + randomSongNumber.ToString());
		songPath = ((Godot.Collections.Array<AudioStream>)radioSongs[songName])[0];
		songBpm = ((Godot.Collections.Array<int>)radioSongs[songName])[1];
		

		bpm = songBpm;
		lastSongName = currentSongName;
		currentSongName = songName;

		GD.Print(songName);
		GD.Print(bpm);

		if (!canRepeat && currentSongName == lastSongName)
		{
			LoadAudio(songPath);
			audioStreamPlayer3D.Play();
		}
		else
		{
			LoadAudio(songPath);
			audioStreamPlayer3D.Play();
		}
	}
}
