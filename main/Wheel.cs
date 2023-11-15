using Godot;
using Dictionary = Godot.Collections.Dictionary;
using System;

public partial class Wheel : RayCast3D
{
	public enum TireType { street_old, street, sport, sport_wet, race, race_wet };


	/* References */
	public Car car;

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
	[Export] float ride_height;  // Height above the wheel that the car will rest
	[Export] float max_travel;   // maximum the wheel can move in either direction
	[Export] float bump;
	[Export] float rebound;

	[ExportCategory("Differential Configuration")]
	[Export] Wheel differential_connection;
	[Export] float diff_accel_perc;
	[Export] float diff_coast_perc;

	/* Wheel positioning */
	Vector3 lastPosition = new Vector3(0, 0, 0);
	Vector3 nextPosition = new Vector3(0, 0, 0);
	public float TireDiameter()
	{
		float diameter = 2 * (tire_width * (tire_aspect / 100f));
		return diameter + (25.4f * rim);
	}

	public void ApplyWheelForce(float wheelTorque)
	{

	}

	public void UpdateWheelPosition(double delta)
	{
		var ground = GetCollisionPoint();
		var groundDir = GetCollisionNormal().Normalized();
		try
		{
			nextPosition.X = Mathf.Clamp(ground.X + (groundDir.X * (TireDiameter() / 1000)), ground.X - groundDir.X * max_travel, ground.X + groundDir.X * max_travel);
			nextPosition.Y = Mathf.Clamp(ground.Y + (groundDir.Y * (TireDiameter() / 1000)), ground.Y - groundDir.Y * max_travel, ground.Y + groundDir.Y * max_travel);
			nextPosition.Z = Mathf.Clamp(ground.Z + (groundDir.Z * (TireDiameter() / 1000)), ground.Z - groundDir.Z * max_travel, ground.Z + groundDir.Z * max_travel);
		}
		catch (Exception e)
		{
			GD.PrintErr(e.Message);
			return;
		}
		if (nextPosition > Position)
			Position = Position.Lerp(nextPosition, bump * (float)delta);
		else
			Position = Position.Lerp(nextPosition, rebound * (float)delta);
	}

	public void UpdateCarBodyPosition(double delta)
	{
		float heightOverRest = (car.Position - Position).Length();
		var dir = GetCollisionNormal().Normalized();
		if (heightOverRest > ride_height)
			car.Position = car.Position.Lerp(car.Position - dir * heightOverRest, rebound * (float)delta);
		else if (heightOverRest < ride_height)
			car.Position = car.Position.Lerp(car.Position + dir * heightOverRest, rebound * (float)delta);

	}

	public override void _Ready()
	{
		base._Ready();
		car = GetParent<Car>();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		Engine.TimeScale = 0.01f;
		UpdateWheelPosition(delta);


	}

}
