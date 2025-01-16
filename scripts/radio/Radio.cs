using Godot;

public partial class Radio : Node3D
{
	/// Make it play chars themes
	/// Glitch Overlays cur song.
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
		{"Song11", new Godot.Collections.Array{{ResourceLoader.Load("res://assets/music/radio/Song 11.ogg")}, {140}}},
	};

	public Godot.Collections.Dictionary<string, AudioStream> radioGlitchSounds {get; set;} = new Godot.Collections.Dictionary<string, AudioStream>
	{
		// KEY:
		{"Glitch1", (AudioStream)ResourceLoader.Load("res://assets/music/electroc/Distance.ogg")},
	};

	[Export] public AudioStreamPlayer3D audioStreamPlayer3D;
	[Export] public int bpm = 120;
	[Export] public float randomGlitchChance = 100;
	[Export] public float glitchTime = 0.2F;
	[Export] public bool canRepeat;
	[Export] public bool canPlay = true;
	[Export] public bool isInGlitch = false;
	[Export] public bool canGlitch = true;
	[Export] public float glitchWaitTime = 5;

	[Export] public Timer glitchTimer = new Timer();
	[Export] public AnimationPlayer animationPlayer;

	public string lastSongName;
	public string currentSongName;

	private float interval;
	private float timer;

	private int maxSongs;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		maxSongs = GetMaxAudio(radioSongs.Count);
		PickRandomSong(canRepeat);

		glitchTimer.WaitTime = glitchWaitTime;
		glitchTimer.Timeout += () => {canGlitch = true;};
		AddChild(glitchTimer);

		audioStreamPlayer3D.Finished += SongFinshed;
		interval = 60.0F / bpm;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (audioStreamPlayer3D.Playing && canPlay && !isInGlitch)
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
		float chanceOfGlitch = (float)GD.RandRange(0.0F, randomGlitchChance + 0.2F);
		if (chanceOfGlitch > randomGlitchChance && !isInGlitch && canGlitch)
		{
			SongGlitch(75F, glitchTime);
			return;
		}

		animationPlayer.Play("beat");
	}

	private void SongGlitch(float fromVolume, float time)
	{
		isInGlitch = true;
		canGlitch = false;

		float radioOriginalVolume = audioStreamPlayer3D.VolumeDb;
		float radioOriginalPosition = audioStreamPlayer3D.GetPlaybackPosition();
		AudioStream radioOriginalAudio = audioStreamPlayer3D.Stream;

		Timer timer = new Timer();
		timer.Autostart = true;
		timer.WaitTime = time + 10;
		timer.Timeout += () => {
			EndSongGlitch(fromVolume, time, radioOriginalAudio, radioOriginalPosition, radioOriginalVolume);

			timer.QueueFree();
		};
		AddChild(timer);

		int maxGlitchSounds = GetMaxAudio(radioGlitchSounds.Count);
		int randomGlitchSound = (int)GD.RandRange(1, maxGlitchSounds);
		string randomSound = ("Glitch" + randomGlitchSound.ToString());

		// Begining of the pre glitch sound.
		Tween preGlitchTween = GetTree().CreateTween();
		preGlitchTween.SetEase(Tween.EaseType.In);
		preGlitchTween.TweenProperty(audioStreamPlayer3D, "volume_db", radioOriginalVolume - fromVolume, time);
		preGlitchTween.TweenCallback( Callable.From(() => {
			// Load glitch sound.
			AudioStream audioStream = radioGlitchSounds[randomSound];
			audioStreamPlayer3D.Stream = audioStream;
			audioStreamPlayer3D.Play();

			Tween postGlitchTween = GetTree().CreateTween();
			postGlitchTween.TweenCallback(Callable.From(() => {
				preGlitchTween = null;
				postGlitchTween = null;
			}));
			postGlitchTween.SetEase(Tween.EaseType.Out);
			postGlitchTween.TweenProperty(audioStreamPlayer3D, "volume_db", radioOriginalVolume, time);
		}) );
	}

	private void EndSongGlitch(float fromVolume, float time, AudioStream originalAudio, float originalPosition, float originalVolume)
	{
		Tween tween = GetTree().CreateTween();
		tween.SetEase(Tween.EaseType.In);
		tween.TweenProperty(audioStreamPlayer3D, "volume_db", originalVolume - fromVolume, time);

		tween.TweenCallback( Callable.From(() => {
			audioStreamPlayer3D.Stream = originalAudio;
			audioStreamPlayer3D.Play(originalPosition);

			Tween endGlitchTween = GetTree().CreateTween();
			endGlitchTween.TweenCallback(Callable.From(() => {
				endGlitchTween = null;
				tween = null;

				glitchTimer.Start();

				isInGlitch = false;
			}));
			endGlitchTween.SetEase(Tween.EaseType.Out);
			endGlitchTween.TweenProperty(audioStreamPlayer3D, "volume_db", originalVolume, time);
		}) );
	}

	public void LoadAudioAndPlay(AudioStream stream, bool canRepeat)
	{
		if (!canRepeat && currentSongName == lastSongName)
		{
			PickRandomSong(canRepeat);
			return;
		}

		audioStreamPlayer3D.Stream = stream;
		audioStreamPlayer3D.Play();
	}

	public void SongFinshed()
	{
		if (isInGlitch) return;
		
		PickRandomSong(canRepeat);
	}

	private void PickRandomSong(bool canRepeat)
	{
		string songName;
		AudioStream songPath;
		int songBpm;

		int randomSongNumber = GD.RandRange(1, maxSongs);
		songName = ("Song" + randomSongNumber.ToString());
		songPath = ((Godot.Collections.Array<AudioStream>)radioSongs[songName])[0];
		songBpm = ((Godot.Collections.Array<int>)radioSongs[songName])[1];
		
		bpm = songBpm;
		lastSongName = currentSongName;
		currentSongName = songName;
		
		LoadAudioAndPlay(songPath, canRepeat);
	}

	private int GetMaxAudio(int number)
	{
		int maxSongsInList = 0;
		for (int song = 1; song < number + 1; song++) maxSongsInList = song;
		return maxSongsInList;
	}
}
