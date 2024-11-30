using Godot;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();
	
	[Signal]
	public delegate void SelectLevelEventHandler(string level);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();
		
		GetNode<Timer>("MessageTimer").Start();
	}

	async public void ShowGameOver()
	{
		ShowMessage("Game Over");
		
		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, Timer.SignalName.Timeout);
		
		var message = GetNode<Label>("Message");
		message.Text = "Dodge the Creeps!";
		message.Show();
		
		await ToSignal(GetTree().CreateTimer(1.0), Timer.SignalName.Timeout);
		GetNode<Button>("StartButton").Show();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}

	public void HideMessage()
	{
		GetNode<Label>("Message").Hide();
	}

	public void HideStartButton()
	{
		GetNode<Button>("StartButton").Hide();
	}

	public void SetLevelUiVisibility(bool visibility)
	{
		switch (visibility)
		{
			case true: 
				GetNode<Label>("LevelMessage").Show();
				GetNode<Button>("EasyButton").Show();
				GetNode<Button>("MediumButton").Show();
				GetNode<Button>("HardButton").Show();
				break;
			case false:
				GetNode<Label>("LevelMessage").Hide();
				GetNode<Button>("EasyButton").Hide();
				GetNode<Button>("MediumButton").Hide();
				GetNode<Button>("HardButton").Hide();
				break;
		}
	}

	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
	}

	private void OnEasyButtonPressed()
	{
		SetLevelUiVisibility(false);
		EmitSignal(SignalName.SelectLevel, "easy");
	}
	
	private void OnMediumButtonPressed()
	{
		SetLevelUiVisibility(false);
		EmitSignal(SignalName.SelectLevel, "medium");
	}
	
	private void OnHardButtonPressed()
	{
		SetLevelUiVisibility(false);
		EmitSignal(SignalName.SelectLevel, "hard");
	}

	private void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}
}
