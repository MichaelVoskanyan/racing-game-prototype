using Godot;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

public partial class Drivetrain : Node
{
	#region References
	public Controller controller;
	#endregion
	#region Engine
	[ExportCategory("Engine Configuration")]
	[Export] public float torque_max;
	[Export] public float rpm_max;
	[Export] public string power_band_file_path = "main/power.json";
	[Export] public float rpm_climb_rate = 1000f;   // revs per second
	[Export] public Dictionary powerband = new Dictionary();
	public float rpm_current = 1505f;
	#endregion

	#region Transmission
	[ExportCategory("Transmission Configuration")]
	[Export] public Array gear_ratios = new Array();
	[Export] public float reverse_ratio;
	[Export] public float diff_ratio;
	public int gear_current;
	public float clutch = 1f;
	#endregion

	#region Brakes
	[ExportCategory("Brake Configuration")]
	[Export] public float brake_force_max;
	[Export] public float brake_bias;   // bias towards front
	#endregion



	[Export] Wheel[] driving_wheels;



	public void ImportEngineTorqueCurve()
	{
		var jsonString = System.IO.File.ReadAllText(power_band_file_path);
		powerband = (Dictionary)Json.ParseString(jsonString);
		foreach (var i in powerband)
		{
			GD.Print("" + (float)i.Key + ' ' + i.Value);
		}
	}


	public float EngineTorque(float throttle)
	{
		Array k = (Array)powerband.Keys;
		Array v = (Array)powerband.Values;


		for (int i = 0; i < k.Count(); i++)
		{
			if (rpm_current > (float)k[i])
				continue;

			if (i > 0)
			{
				float p = (rpm_current - (float)k[i - 1]) / ((float)k[i] - (float)k[i - 1]);
				float pow = (float)v[i] - (float)v[i - 1];
				pow *= p;
				return (pow + (float)v[i - 1]) * throttle;
			}
			else
			{
				return ((rpm_current / (float)k[i]) * (float)v[i]) * throttle;
			}
		}

		return 0.0f;
	}

	public float EngineRPM(float v, double delta)
	{
		if (clutch == 1 && v == 0)
		{
			return 0f;
		}
		else if (Mathf.Sign(v) == 1 && clutch == 1)
		{
			return v * (float)gear_ratios[gear_current] * diff_ratio * (60 / (2 * Mathf.Pi));
		}
		else if (Mathf.Sign(v) == -1 && clutch == 1)
		{
			return v * reverse_ratio * diff_ratio * (60 / (2 * Mathf.Pi));
		}
		else
		{
			float revRate = controller.throttle > 0f ? rpm_climb_rate * controller.throttle * (float)delta :
					-rpm_climb_rate * 0.5f * (float)delta;
			return rpm_current += revRate;
		}
	}

	public float BrakeForce(float brake)
	{
		return brake_force_max * brake;
	}

	public override void _Ready()
	{
		base._Ready();
		ImportEngineTorqueCurve();
		GD.Print("Current torque: " + EngineTorque(0.5f));
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

	}

}
