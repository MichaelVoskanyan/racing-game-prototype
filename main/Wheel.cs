using Godot;
using Dictionary = Godot.Collections.Dictionary;
using System;
using System.Runtime.CompilerServices;

public partial class Wheel : RayCast3D
{
	public enum TireType { street_old, street, sport, sport_wet, race, race_wet };

	/* References */
	public Car car;
	public Drivetrain dr;

	#region TireData
	/* Tire Data */
	[ExportCategory("Tire Data")]
	[Export] public float tire_width = 225;
	[Export] public float tire_aspect = 45;
	[Export] public float rim = 17;
	[Export] public TireType tire_type = TireType.street;
	#endregion

	#region WheelBehavior
	[ExportCategory("Wheel Behavior")]
	[Export] public bool Driving = true;
	[Export] public bool Steering = true;
	#endregion

	#region SuspensionConfig
	[ExportCategory("Suspension Configuration")]
	[Export] Wheel anti_roll_bar_connection;
	[Export] float anti_roll_rate;
	[Export] float spring_rate;
	[Export] float ride_height;  // Height above the wheel that the car will rest
	[Export] float max_travel;   // maximum the wheel can move in either direction
	[Export] float bump;
	[Export] float rebound;
	#endregion

	#region DiffConfig
	[ExportCategory("Differential Configuration")]
	[Export] Wheel differential_connection;
	[Export] float diff_accel_perc;
	[Export] float diff_coast_perc;
	#endregion

	/* Wheel physics */
	float longi_slip_ratio;
	float wheel_speed;

	/* Wheel positioning */
	Vector3 lastPosition = new Vector3(0, 0, 0);
	Vector3 nextPosition = new Vector3(0, 0, 0);

	public void CalcLongiSlip()
	{
		// delta = ( Wheel angular velocity * Radius (meters) - Vel Long) / (|Vel Long|)

	}

	public void CalcWheelSpeed()
	{

	}

	public float TireDiameter()
	{
		float diameter = 2 * (tire_width * (tire_aspect / 100f));
		return diameter + (25.4f * rim);
	}


	public override void _Ready()
	{
		base._Ready();
		car = GetParent<Car>();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

	}

}
