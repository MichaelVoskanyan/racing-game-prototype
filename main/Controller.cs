using Godot;
using Array = Godot.Collections.Array;
using System;

public partial class Controller : Node
{
	/// <summary>
	/// Settings to check player assists and controller type
	/// </summary> 
	[ExportCategory("Player Assists")]
	[Export] public bool AutomaticOnly = false;
	[Export] public bool UseController = false;
	[Export] public bool UseWheel = true;
	[Export] public bool ABS = false;
	[Export] public bool TC = false;
	[Export] public bool SimulationSteering = true;
	[ExportCategory("Controller Settings")]
	[Export] public float ThrottleDeadzoneIn = 0.05f;
	[Export] public float ThrottleDeadzoneOut = 0.05f;
	[Export] public float BrakeDeadzoneIn = 0.05f;
	[Export] public float BrakeDeadzoneOut = 0.05f;
	[Export] public float ClutchDeadzoneIn = 0.05f;
	[Export] public float ClutchDeadzoneOut = 0.05f;

	[Export] public Label label;
	/* Input values */
	public float throttle;
	public float brake;
	public float clutch;
	public float steering;

	public override void _Ready()
	{
		base._Ready();
	}

	public void LogiWheelInit()
	{
		// Implement logitech wheel initialization code here
	}

	public override void _Process(double delta)

	{
		base._Process(delta);
		throttle = Input.GetActionRawStrength("throttle");
		brake = Input.GetActionRawStrength("brake");

		/* Scaling inputs with deadzones */
		throttle = (throttle - ThrottleDeadzoneIn) / ((1 - ThrottleDeadzoneOut) - ThrottleDeadzoneIn);
		brake = (brake - ThrottleDeadzoneIn) / ((1 - BrakeDeadzoneOut) - BrakeDeadzoneIn);

		// Engine.TimeScale = 0.1f;
	}



}
