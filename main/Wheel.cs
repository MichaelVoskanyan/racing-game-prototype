using Godot;
using Dictionary = Godot.Collections.Dictionary;
using System;
using System.Diagnostics;

public partial class Wheel : RayCast3D
{
	public enum TireType { street_old, street, sport, sport_wet, race, race_wet };

	/* References */
	public Car car;
	public RigidBody3D carBody;
	public Drivetrain dr;

	#region TireData
	/* Tire Data */
	[ExportCategory("Tire Data")]
	[Export] public float tire_width = 225;
	[Export] public float tire_aspect = 45;
	[Export] public float rim = 17;
	[Export] public TireType tire_type = TireType.street;
	public float wheelRadius;
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
	[Export] float springStiffness;
	[Export] float rest;  // distance below the car that the wheel will rest
	[Export] float bump;
	[Export] float rebound;

	// youtube tutorial 
	// Length of spring = distance between raycast origin and whelehit minus the wheel radius
	// raycast full length (maxlength + wheelRadius)
	// maxlength = restlength + springtravel
	[ExportCategory("Suspension Unity")]
	[Export] public float restLength;
	[Export] public float springTravel;
	[Export] public float damperStiffness;

	private float minLength;
	private float maxLength;
	private float lastLength;
	private float springLength;
	public float springForce;
	private float springVelocity;
	private float damperForce;
	
	// Applied to car body
	public Vector3 suspensionForce;	

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

    public override void _Ready()
    {
        base._Ready();
		minLength = restLength - springTravel;
		maxLength = restLength + springTravel;
		car = (Car)GetParent();
		carBody = GetParent<RigidBody3D>();
		wheelRadius = WheelRadius();
    }

    public void SuspensionPosition(float delta) {
		TargetPosition = Vector3.Down * (maxLength + wheelRadius);
		bool hit = IsColliding();
		if (hit) {
			lastLength = springLength;
			springLength = Position.DistanceTo(GetCollisionPoint()) - wheelRadius;
			springLength = Mathf.Clamp(springLength, minLength, maxLength);
			springVelocity = (lastLength - springLength) / delta;
			springForce = springStiffness * (restLength - springLength);
			damperForce = damperStiffness * springVelocity;

			suspensionForce = (springForce + damperForce) * GetCollisionNormal();
			// GD.Print("Susp force: " + suspensionForce.ToString());
			carBody.ApplyForce(suspensionForce, GetCollisionPoint());
		} else {
			suspensionForce = Vector3.Zero;
		}
	}

	public float WheelRadius()
	{
		float rad = tire_width * (tire_aspect / 100f);
		return (rad + (25.4f * rim))/1000f;
	}

    public override void _Process(double delta)
    {
        base._PhysicsProcess(delta);
		SuspensionPosition((float) delta);
    }

}
