using Godot;
using Dictionary = Godot.Collections.Dictionary;
using System;

public partial class Wheel : RayCast3D
{
	public enum TireType { street_old, street, sport, sport_wet, race, race_wet };

	/* Tire Data */
	[ExportCategory("Tire Data")]
	[Export] public float tire_width = 225;
	[Export] public float tire_aspect = 45;
	[Export] public float rim = 17;
	[Export] public TireType tire_type = TireType.street;

	[ExportCategory("Suspension Configuration")]
	[Export] Wheel anti_roll_bar_connection;
	[Export] float anti_roll_rate;
	[Export] float spring_rate;
	[Export] float spring_rest_height;  // Height above the wheel that the car will rest
	[Export] float spring_max_travel;   // maximum the wheel can move in either direction
	[Export] float bump;
	[Export] float rebound;

	[ExportCategory("Differential Configuration")]
	[Export] Wheel differential_connection;
	[Export] float diff_accel_perc;
	[Export] float diff_coast_perc;

	public float TireDiameter()
	{
		float diameter = 2 * (tire_width * (tire_aspect / 100f));
		return diameter + (25.4f * rim);
	}

	public void ApplyWheelForce(float wheelTorque)
	{

	}


}
