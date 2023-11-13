
// using System;
// using Godot;
// using Dictionary = Godot.Collections.Dictionary;
// using Array = Godot.Collections.Array;
// using System.Linq;


// public partial class Car : RigidBody3D
// {
	 
// 	[Export] bool Debug_Mode = false;
	
// 	// controls
// 	[Export] bool Use_Global_Control_Settings = false;
// 	[Export] bool UseMouseSteering = false;
// 	[Export] bool UseWheelSteering = true;
// 	[Export] bool UseAccelerometreSteering = false;
// 	[Export] float SteerSensitivity = 1.0f;
// 	[Export] float KeyboardSteerSpeed = 0.025f;
// 	[Export] float KeyboardReturnSpeed = 0.05f;
// 	[Export] float KeyboardCompensateSpeed = 0.1f;
	
// 	[Export] float SteerAmountDecay = 0.015f ;// understeer help
// 	[Export] float SteeringAssistance = 1.0f;
// 	[Export] float SteeringAssistanceAngular = 0.12f;
	
// 	[Export] bool LooseSteering = false ;//simulate rack && pinion steering physics (EXPERIMENTAL)
	
// 	[Export] float OnThrottleRate = 0.2f;
// 	[Export] float OffThrottleRate = 0.2f;
	
// 	[Export] float OnBrakeRate = 0.05f;
// 	[Export] float OffBrakeRate = 0.1f;
	
// 	[Export] float OnHandbrakeRate = 0.2f;
// 	[Export] float OffHandbrakeRate = 0.2f;
	
// 	[Export] float OnClutchRate = 0.2f;
// 	[Export] float OffClutchRate = 0.2f;
	
// 	[Export] float MaxThrottle = 1.0f;
// 	[Export] float MaxBrake = 1.0f;
// 	[Export] float MaxHandbrake = 1.0f;
// 	[Export] float MaxClutch = 1.0f;
	
// 	[Export] Array GearAssistant = new Array(){
// 	20, // Shift delay
// 	2, // Assistance Level (0 - 2)
// 	0.944087, // Speed Influence (will be automatically set)
// 	6000.0, // Downshift RPM Iteration
// 	6200.0, // Upshift RPM
// 	3000.0, // Clutch-Out RPM
// 	5, // throttle input allowed after shiting delay
// 	};
	
// 	// meta
// 	[Export] bool Controlled = true;
	
// 	// chassis
// 	[Export] float Weight = 900.0f ;// kg
	
// 	// body
// 	[Export] float LiftAngle = 0.1f;
// 	[Export] float DragCoefficient = 0.25f;
// 	[Export] float Downforce = 0.0f;
	
	
// 	//steering
// 	[Export] public float AckermannPoint = -3.8f;
// 	[Export] public float Steer_Radius = 13.0f;
	
// 	//drivetrain
// 	[Export] Array Powered_Wheels = new Array(){"fl","fr"};

	
// 	[Export] float FinalDriveRatio = 4.250f;
// 	[Export] Array GearRatios = new Array(){ 3.250f, 1.894f, 1.259f, 0.937f, 0.771f };
// 	[Export] float ReverseRatio = 3.153f;
	
// 	[Export] float RatioMult = 9.5f;
// 	[Export] float StressFactor = 1.0f;
// 	[Export] float GearGap = 60.0f;
// 	[Export] float DSWeight = 150.0f ;// Leave this be, unless you know what you're doing.
	
// 	public enum TransmissionType {FullyManual, Automatic, ContinuouslyVariable, SemiAuto};
// 	[Export] TransmissionType transmissionType = TransmissionType.FullyManual;
	
// 	[Export] Array AutoSettings = new Array(){
// 	6500.0, // shift rpm (auto)
// 	300.0, // downshift threshold (auto)
// 	0.5, // throttle efficiency threshold (range: 0 - 1) (auto/dct)
// 	0.0, // engagement rpm threshold (auto/dct/cvt)
// 	4000.0, // engagement rpm (auto/dct/cvt)
// 	};
	
// 	[Export] Array CVTSettings = new Array(){
// 	0.75, // throttle efficiency threshold (range: 0 - 1)
// 	0.025, // acceleration rate (range: 0 - 1)
// 	0.9, // iteration 1 (higher = higher rpm)
// 	500.0, // iteration 2 (higher = better acceleration from standstill but unstable)
// 	2.0, // iteration 3 (higher = longer it takes to "lock" the rpm)
// 	0.2, // iteration 4 (keep it over 0.1)
// 	};
	
	
	
	
// 	//stability
// 	[Export] public Array ABS = new Array(){ // anti-lock braking system
// 	2500.0, // threshold
// 	1, // pump time
// 	10, // vehicle speed before activation
// 	true, // enabled
// 	0.5, // pump force (0.0 - 1.0)
// 	500.0, // lateral threshold
// 	2, // lateral pump time
// 	};
	
// 	[Export] Array ESP = new Array(){ // electronic stability program
// 	0.5, // stabilisation theshold
// 	1.5, // stabilisation rate (higher = understeer, understeer = inefficient)
// 	1, // yaw threshold
// 	3.0, // yaw rate
// 	false, // enableda
// 	};
	
// 	[Export] Array BTCS = new Array(){ // brake-based traction control system
// 	10, // threshold
// 	0.05, // sensitivity
// 	false, // enabled
// 	};
	
// 	[Export] Array TTCS = new Array(){ // throttle-based traction control system
// 	5, // threshold
// 	1.0, // sensitivity
// 	false, // enabled
// 	};
	
	
// 	//differentials
// 	[Export] public float Locking = 0.1f;
// 	[Export] public float CoastLocking = 0.0f;
// 	[Export] public float Preload = 0.0f;
	
// 	[Export] public float Centre_Locking = 0.5f;
// 	[Export] public float Centre_CoastLocking = 0.5f;
// 	[Export] public float Centre_Preload = 0.0f;
	
// 	//engine
// 	[Export] public float RevSpeed = 2.0f ;// Flywheel lightness
// 	[Export] public float EngineFriction = 18000.0f;
// 	[Export] public float EngineDrag = 0.006f;
// 	[Export] public float ThrottleResponse = 0.5f;
// 	[Export] public float DeadRPM = 100.0f;
	
// 	//ECU
// 	[Export] float RPMLimit = 7000.0f;
// 	[Export] int LimiterDelay = 4;
// 	[Export] float IdleRPM = 800.0f;
// 	[Export] float ThrottleLimit = 0.0f;
// 	[Export] float ThrottleIdle = 0.25f;
// 	[Export] float VVTRPM = 4500.0f ;// set this beyond the rev range to disable it, set it to 0 to use this vvt state permanently
	
// 	//torque normal state
// 	[Export] float BuildUpTorque = 0.0035f;
// 	[Export] float TorqueRise = 30.0f;
// 	[Export] float RiseRPM = 1000.0f;
// 	[Export] float OffsetTorque = 110.0f;
// 	[Export] float FloatRate = 0.1f;
// 	[Export] float DeclineRate = 1.5f;
// 	[Export] float DeclineRPM = 3500.0f;
// 	[Export] float DeclineSharpness = 1.0f;
	
// 	//torque [Export] variable valve timing triggered
// 	[Export] float VVT_BuildUpTorque = 0.0f;
// 	[Export] float VVT_TorqueRise = 60.0f;
// 	[Export] float VVT_RiseRPM = 1000.0f;
// 	[Export] float VVT_OffsetTorque = 70.0f;
// 	[Export] float VVT_FloatRate = 0.1f;
// 	[Export] float VVT_DeclineRate = 2.0f;
// 	[Export] float VVT_DeclineRPM = 5000.0f;
// 	[Export] float VVT_DeclineSharpness = 1.0f;
	
// 	//clutch
// 	[Export] public float ClutchStable = 0.5f;
// 	[Export] public float GearRatioRatioThreshold = 200.0f;
// 	[Export] public float ThresholdStable = 0.01f;
// 	[Export] public float ClutchGrip = 176.125f;
// 	[Export] public float ClutchFloatReduction = 27.0f;
	
// 	[Export] float ClutchWobble = 2.5f*0;
// 	[Export] float ClutchElasticity = 0.2f*0;
// 	[Export] float WobbleRate = 0.0f;
	
// 	//forced inductions
// 	[Export] float MaxPSI = 9.0f ;// Maximum air generated by any forced inductions
// 	[Export] float EngineCompressionRatio = 8.0f ;// Piston travel distance
// 	//turbo
// 	[Export] bool TurboEnabled = false ;// Enables turbo
// 	[Export] int TurboAmount = 1 ;// Turbo power multiplication.
// 	[Export] float TurboSize = 8.0f ;// Higher = More turbo lag
// 	[Export] float Compressor = 0.3f ;// Higher = Allows more spooling on low RPM
// 	[Export] float SpoolThreshold = 0.1f ;// Range: 0 - 0.9999
// 	[Export] float BlowoffRate = 0.14f;
// 	[Export] float TurboEfficiency = 0.075f ;// Range: 0 - 1
// 	[Export] float TurboVacuum = 1.0f ;// Performance deficiency upon turbo idle
// 	//supercharger
// 	[Export] bool SuperchargerEnabled = false ;// Enables supercharger
// 	[Export] float SCRPMInfluence = 1.0f;
// 	[Export] float BlowRate = 35.0f;
// 	[Export] float SCThreshold = 6.0f;
	
// 	public float rpm = 0.0f;
// 	public float rpmspeed = 0.0f;
// 	public float resistancerpm = 0.0f;
// 	public float resistancedv = 0.0f;
// 	public int gear = 0;
// 	public int limdel = 0;
// 	public int actualgear = 0;
// 	public float gearstress = 0.0f;
// 	public float throttle = 0.0f;
// 	public float cvtaccel = 0.0f;
// 	public float sassistdel = 0f;
// 	public int sassiststep = 0;
// 	public bool clutchin = false;
// 	public bool gasrestricted = false;
// 	public bool revmatch = false;
// 	public float gaspedal = 0.0f;
// 	public float brakepedal = 0.0f;
// 	public float clutchpedal = 0.0f;
// 	public float clutchpedalreal = 0.0f;
// 	public float steer = 0.0f;
// 	public float steer2 = 0.0f;
// 	public float abspump = 0.0f;
// 	public float tcsweight = 0.0f;
// 	public bool tcsflash = false;
// 	public bool espflash = false;
// 	public float ratio = 0.0f;
// 	public bool vvt = false;
// 	public float brake_allowed = 0.0f;
// 	public float readout_torque = 0.0f;
	
// 	public float brakeline = 0.0f;
// 	public float handbrakepull = 0.0f;
// 	public float dsweight = 0.0f;
// 	public float dsweightrun = 0.0f;
// 	public float diffspeed = 0.0f;
// 	public float diffspeedun = 0.0f;
// 	public float locked = 0.0f;
// 	public float c_locked = 0.0f;
// 	public float wv_difference = 0.0f;
// 	public float rpmforce = 0.0f;
// 	public float whinepitch = 0.0f;
// 	public float turbopsi = 0.0f;
// 	public float scrpm = 0.0f;
// 	public float boosting = 0.0f;
// 	public float rpmcs = 0.0f;
// 	public float rpmcsm = 0.0f;
// 	public float currentstable = 0.0f;
// 	public Array steering_geometry = new Array(){0.0f,0.0f};
// 	public float resistance = 0.0f;
// 	public float wob = 0.0f;
// 	public float ds_weight = 0.0f;
// 	public float steer_torque = 0.0f;
// 	public float steer_velocity = 0.0f;
// 	public float drivewheels_size = 1.0f;
	
// 	public Array steering_angles = new Array(){};
// 	public float max_steering_angle = 0.0f;
// 	public float assistance_factor = 0.0f;
	
	
	
// 	public Vector3 pastvelocity = new Vector3(0,0,0);
// 	public Vector3 gforce = new Vector3(0,0,0);
// 	public float clock_mult = 1.0f;
// 	public float dist = 0.0f;
// 	public float stress = 0.0f;
	
	
	
// 	public bool su = false;
// 	public bool sd = false;
// 	public bool gas = false;
// 	public bool brake = false;
// 	public bool handbrake = false;
// 	public bool right = false;
// 	public bool left = false;
// 	public bool clutch = false;
// 	public Array c_pws = new Array(){};
	
// 	public Vector3 velocity = new Vector3(0,0,0);
// 	public Vector3 rvelocity = new Vector3(0,0,0);
	
// 	public float stalled = 0.0f;

// 	ForceFeedback ffb;
	
// 	public void bullet_fix()
// 	{  
// 		Vector3 offset = GetNode<Marker3D>("DRAG_CENTRE").Position;

// 		AckermannPoint -= offset.Z;
		
// 		foreach(Node3D i in GetChildren())
// 		{
// 			i.Position-= offset;
	
// 		}
// 	}
	
// 	public void _ready()
// 	{  
// 	//	bullet_fix()
// 		ffb = GetNode<ForceFeedback>("ffb");
// 		rpm = IdleRPM;
// 		foreach(String i in Powered_Wheels)
// 		{
// 			var wh = GetNode<Wheel>(i);
// 			c_pws.Append(wh);
// 		}
// 	}
	
// 	public void controls()
// 	{  
// 		float mouseposx = 0.0f;
		
// 		mouseposx = GetViewport().GetMousePosition().X/GetWindow().Size.X;
// 		if(UseMouseSteering)
// 		{
// 			gas = Input.IsActionPressed("gas_mouse");
// 			brake = Input.IsActionPressed("brake_mouse");
// 			su = Input.IsActionJustPressed("shiftup_mouse");
// 			sd = Input.IsActionJustPressed("shiftdown_mouse");
// 			handbrake = Input.IsActionPressed("handbrake_mouse");
// 		}
// 		else
// 		{
// 			gas = Input.IsActionPressed("gas");
// 			brake = Input.IsActionPressed("brake");
// 			su = Input.IsActionJustPressed("shiftup");
// 			sd = Input.IsActionJustPressed("shiftdown");
// 			handbrake = Input.IsActionPressed("handbrake");
		
// 		}
// 		left = Input.IsActionPressed("left");
// 		right = Input.IsActionPressed("right");
// 		if(!UseWheelSteering)
// 		{
// 			if(left)
// 			{
// 				steer_velocity -= 0.01f;
// 			}
// 			else if(right)
// 			{
// 				steer_velocity += 0.01f;
			
// 			}
// 			if(LooseSteering)
// 			{
// 				steer += steer_velocity;
	
// 				if(Mathf.Abs(steer)>1.0)
// 				{
// 					steer_velocity *= -0.5f;
	
// 				}
// 				// foreach(var i in [GetNode("fl"),GetNode("fr")])
// 				// {
// 				// 	steer_velocity += (i.directional_force.x*0.00125)*i.Caster;
// 				// 	steer_velocity -= (i.stress*0.0025)*(Mathf.Atan2(Mathf.Abs(i.wv),1.0)*i.angle);
					
// 				// 	steer_velocity += steer*(i.directional_force.z*0.0005)*i.Caster;
	
// 				// 	if(i.position.x>0)
// 				// 	{
// 				// 		steer_velocity += i.directional_force.z*0.0001;
// 				// 	}
// 				// 	else
// 				// 	{
// 				// 		steer_velocity -= i.directional_force.z*0.0001;
				
// 				// 	}
// 				// 	steer_velocity /= i.stress/(i.slip_percpre*(i.slip_percpre*100.0) +1.0) +1.0
// 				// }
// 				Wheel fl = GetNode<Wheel>("fl");

// 				steer_velocity += (fl.directional_force.X*0.00125f)*(float)fl.Caster;
// 				steer_velocity -= (fl.stress*0.0025f)*(Mathf.Atan2(Mathf.Abs(fl.wv),1.0f)*fl.angle);
					
// 				steer_velocity += steer*(fl.directional_force.Z*0.0005f)*fl.Caster;

// 				if(fl.Position.X>0)
// 					steer_velocity += fl.directional_force.Z * 0.001f;
// 				else
// 					steer_velocity -= fl.directional_force.Z * 0.001f;
				
// 				steer_velocity /= fl.stress/(fl.slip_percpre*(fl.slip_percpre*100f) + 1f) +1f;

// 				Wheel fr = GetNode<Wheel>("fr");

// 				steer_velocity += (fr.directional_force.X*0.00125f)*(float)fr.Caster;
// 				steer_velocity -= (fr.stress*0.0025f)*(Mathf.Atan2(Mathf.Abs(fr.wv),1.0f)*fr.angle);
					
// 				steer_velocity += steer*(fr.directional_force.Z*0.0005f)*fr.Caster;

// 				if(fr.Position.X>0)
// 					steer_velocity += fr.directional_force.Z * 0.001f;
// 				else
// 					steer_velocity -= fr.directional_force.Z * 0.001f;
				
// 				steer_velocity /= fr.stress/(fr.slip_percpre*(fr.slip_percpre*100f) + 1f) +1f;
// 			}
// 		}
// 		else
// 		{
// 			InputMap.ActionSetDeadzone("left", 0.01f);
// 			InputMap.ActionSetDeadzone("right", 0.01f);
// 			steer = Input.GetAxis("left", "right") * SteerSensitivity;
		
// 		}
// 		if(Controlled)
// 		{
// 			if((float)GearAssistant[1] == 2)
// 			{
// 				if(gas && !gasrestricted && gear != -1 || brake && gear == -1 || revmatch)
// 				{
// 					gaspedal += OnThrottleRate/clock_mult;
// 				}
// 				else
// 				{
// 					gaspedal -= OffThrottleRate/clock_mult;
	
// 				}
// 				if(brake && gear != -1 || gas && gear == -1)
// 				{
// 					brakepedal += OnBrakeRate/clock_mult;
// 				}
// 				else
// 				{
// 					brakepedal -= OffBrakeRate/clock_mult;
// 				}
// 			}
// 			else
// 			{
// 				if((float)GearAssistant[1] == 0)
// 				{
// 					gasrestricted = false;
// 					clutchin = false;
// 					revmatch = false;
				
// 				}
// 				if(gas && !gasrestricted || revmatch)
// 				{
// 					gaspedal += OnThrottleRate/clock_mult;
// 				}
// 				else
// 				{
// 					gaspedal -= OffThrottleRate/clock_mult;
	
// 				}
// 				if(brake)
// 				{
// 					brakepedal += OnBrakeRate/clock_mult;
// 				}
// 				else
// 				{
// 					brakepedal -= OffBrakeRate/clock_mult;
	
// 				}
// 			}
// 			if(handbrake)
// 			{
// 				handbrakepull += OnHandbrakeRate/clock_mult;
// 			}
// 			else
// 			{
// 				handbrakepull -= OffHandbrakeRate/clock_mult;
	
// 			}
// 			var siding = Mathf.Abs(velocity.X);
	
// 			if(velocity.X>0 && steer2>0 || velocity.X<0 && steer2<0)
// 			{
// 				siding = 0.0f;
				
// 			}
// 			var going = velocity.Z/(siding +1.0);
// 			if(going<0)
// 			{
// 				going = 0;
	
// 			}
// 			if(!LooseSteering)
// 			{
// 				if(UseMouseSteering)
// 				{
// 					steer2 = (mouseposx-0.5f)*2.0f;
// 					steer2 *= SteerSensitivity;
// 					if(steer2>1.0)
// 					{
// 						steer2 = 1.0f;
// 					}
// 					else if(steer2<-1.0)
// 					{
// 						steer2 = -1.0f;
					
// 					}
// 					float s = Mathf.Abs(steer2)*1.0f +0.5f;
// 					if(s>1)
// 					{
// 						s = 1f;
					
// 					}
// 					steer2 *= s;
// 				}
// 				else if(UseAccelerometreSteering)
// 				{
// 					steer2 = Input.GetAccelerometer().X/10.0f;
// 					steer2 *= SteerSensitivity;
// 					if(steer2>1.0)
// 					{
// 						steer2 = 1.0f;
// 					}
// 					else if(steer2<-1.0)
// 					{
// 						steer2 = -1.0f;
					
// 					}
// 					float s = Mathf.Abs(steer2)*1.0f +0.5f;
// 					if(s>1)
// 					{
// 						s = 1f;
					
// 					}
// 					steer2 *= s;
// 				}
// 				else if(UseWheelSteering)
// 				{
// 					steer2 = Input.GetAxis("left", "right") * SteerSensitivity;
					
	
// 				}
// 				else
// 				{
// 					if(right)
// 					{
// 						if(steer2>0)
// 						{
// 							steer2 += KeyboardSteerSpeed;
// 						}
// 						else
// 						{
// 							steer2 += KeyboardCompensateSpeed;
// 						}
// 					}
// 					else if(left)
// 					{
// 						if(steer2<0)
// 						{
// 							steer2 -= KeyboardSteerSpeed;
// 						}
// 						else
// 						{
// 							steer2 -= KeyboardCompensateSpeed;
// 						}
// 					}
// 					else
// 					{
// 						if(steer2>KeyboardReturnSpeed)
// 						{
// 							steer2 -= KeyboardReturnSpeed;
// 						}
// 						else if (steer2<-KeyboardReturnSpeed)
// 							steer2 += KeyboardReturnSpeed;
// 						else
// 						{
// 							steer2 = 0.0f;
						
// 						}
// 					}
// 					if(steer2>1.0)
// 					{
// 						steer2 = 1.0f;
// 					}
// 					else if(steer2<-1.0)
// 					{
// 						steer2 = -1.0f;
						
// 					}
// 				}
// 				if(assistance_factor>0.0)
// 				{
// 					float maxsteer = (float) (1.0f/(going*(SteerAmountDecay/assistance_factor) +1.0f));
					
// 					var assist_commence = LinearVelocity.Length()/10.0f;
// 					if(assist_commence>1.0)
// 					{
// 						assist_commence = 1.0f;
					
// 					}
// 					steer = (steer2*maxsteer) -(velocity.Normalized().X*assist_commence)*(SteeringAssistance*assistance_factor) +rvelocity.Y*(SteeringAssistanceAngular*assistance_factor);
// 				}
// 				else
// 				{
// 					steer = steer2;
// 				}
// 			}
// 		}
// 	}
	
// 	public void limits()
// 	{  
// 		if(gaspedal<0.0)
// 		{
// 			gaspedal = 0.0f;
// 		}
// 		else if(gaspedal>MaxThrottle)
// 		{
// 			gaspedal = MaxThrottle;
	
// 		}
// 		if(brakepedal<0.0)
// 		{
// 			brakepedal = 0.0f;
// 		}
// 		else if(brakepedal>MaxBrake)
// 		{
// 			brakepedal = MaxBrake;
	
// 		}
// 		if(handbrakepull<0.0)
// 		{
// 			handbrakepull = 0.0f;
// 		}
// 		else if(handbrakepull>MaxHandbrake)
// 		{
// 			handbrakepull = MaxHandbrake;
	
// 		}
// 		if(steer<-1.0)
// 		{
// 			steer = -1.0f;
// 		}
// 		else if(steer>1.0)
// 		{
// 			steer = 1.0f;
	
// 		}
// 	}
	
// 	public void transmission()
// 	{  
		
// 		su = Input.IsActionJustPressed("shiftup") && !UseMouseSteering || Input.IsActionJustPressed("shiftup_mouse") && UseMouseSteering;
// 		sd = Input.IsActionJustPressed("shiftdown") && !UseMouseSteering || Input.IsActionJustPressed("shiftdown_mouse") && UseMouseSteering;
		
// 		var clutch = Input.IsActionPressed("clutch") && !UseMouseSteering || Input.IsActionPressed("clutch_mouse") && UseMouseSteering;
// 		if((float)GearAssistant[1] != 0)
// 		{
// 			clutch = Input.IsActionPressed("handbrake") && !UseMouseSteering || Input.IsActionPressed("handbrake_mouse") && UseMouseSteering;
// 		}
// 		clutch = !clutch;
		
// 		if(transmissionType == 0)
// 		{
// 			if(clutch && !clutchin)
// 			{
// 				clutchpedalreal -= OffClutchRate/clock_mult;
// 			}
// 			else
// 			{
// 				clutchpedalreal += OnClutchRate/clock_mult;
		
// 			}
// 			if(clutchpedalreal<0)
// 			{
// 				clutchpedalreal = 0;
// 			}
// 			else if(clutchpedalreal>MaxClutch)
// 			{
// 				clutchpedalreal = MaxClutch;
		
// 			}
// 			clutchpedal = 1.0f-clutchpedalreal;
	
// 			if(gear>0)
// 			{
// 				ratio = (float)GearRatios[gear-1]*FinalDriveRatio*RatioMult;
// 			}
// 			else if(gear == -1)
// 			{
// 				ratio = ReverseRatio*FinalDriveRatio*RatioMult;
			
// 			}
// 			if((int)GearAssistant[1] == 0)
// 			{
// 				if(su)
// 				{
// 					su = false;
// 					if(gear<GearRatios.Count())
// 					{
// 						if(gearstress<GearGap)
// 						{
// 							actualgear += 1;
// 						}
// 					}
// 				}
// 				if(sd)
// 				{
// 					sd = false;
// 					if(gear>-1)
// 					{
// 						if(gearstress<GearGap)
// 						{
// 							actualgear -= 1;
// 						}
// 					}
// 				}
// 			}
// 			else if((int)GearAssistant[1] == 1)
// 			{
// 				if(rpm<(float)GearAssistant[5])
// 				{
// 					var irga_ca = ((float)GearAssistant[5]-rpm)/((float)GearAssistant[5]-IdleRPM);
// 					clutchpedalreal = irga_ca*irga_ca;
// 					if(clutchpedalreal>1.0)
// 					{
// 						clutchpedalreal = 1.0f;
// 					}
// 				}
// 				else
// 				{
// 					if(!gasrestricted && !revmatch)
// 					{
// 						clutchin = false;
// 					}
// 				}
// 				if(su)
// 				{
// 					su = false;
// 					if(gear<GearRatios.Count())
// 					{
// 						if(rpm<(float)GearAssistant[5])
// 						{
// 							actualgear += 1;
// 						}
// 						else
// 						{
// 							if(actualgear<1)
// 							{
// 								actualgear += 1;
// 								if(rpm>(float)GearAssistant[5])
// 								{
// 									clutchin = false;
// 								}
// 							}
// 							else
// 							{
// 								if(sassistdel>0)
// 								{
// 									actualgear += 1;
// 								}
// 								sassistdel = (float)GearAssistant[0]/2.0f;
// 								sassiststep = -4;
	
// 								clutchin = true;
// 								gasrestricted = true;
// 							}
// 						}
// 					}
// 				}
// 				else if(sd)
// 				{
// 					sd = false;
// 					if(gear>-1)
// 					{
// 						if(rpm<(float)GearAssistant[5])
// 						{
// 							actualgear -= 1;
// 						}
// 						else
// 						{
// 							if(actualgear == 0 || actualgear == 1)
// 							{
// 								actualgear -= 1;
// 								clutchin = false;
// 							}
// 							else
// 							{
// 								if(sassistdel>0)
// 								{
// 									actualgear -= 1;
// 								}
// 								sassistdel = (float)GearAssistant[0]/2.0f;
// 								sassiststep = -2;
	
// 								clutchin = true;
// 								revmatch = true;
// 								gasrestricted = false;
// 							}
// 						}
// 					}
// 				}
// 			}
// 			else if((int)GearAssistant[1] == 2)
// 			{
// 				var assistshiftspeed = ((float)GearAssistant[4]/ratio)*(float)GearAssistant[2];
// 				var assistdownshiftspeed = ((float)GearAssistant[3]/Mathf.Abs(((float)GearRatios[gear-2]*FinalDriveRatio)*RatioMult))*(float)GearAssistant[2];
// 				if(gear == 0)
// 				{
// 					if(gas)
// 					{
// 						sassistdel -= 1;
// 						if(sassistdel<0)
// 						{
// 							actualgear = 1;
// 						}
// 					}
// 					else if(brake)
// 					{
// 						sassistdel -= 1;
// 						if(sassistdel<0)
// 						{
// 							actualgear = -1;
// 						}
// 					}
// 					else
// 					{
// 						sassistdel = 60;
// 					}
// 				}
// 				else if(LinearVelocity.Length()<5)
// 				{
// 					if(!gas && gear == 1 || !brake && gear == -1)
// 					{
// 						sassistdel = 60;
// 						actualgear = 0;
// 					}
// 				}
// 				if(sassiststep == 0)
// 				{
// 					if(rpm<(float)GearAssistant[5])
// 					{
// 						var irga_ca = ((float)GearAssistant[5]-rpm)/((float)GearAssistant[5]-IdleRPM);
// 						clutchpedalreal = irga_ca*irga_ca;
// 						if(clutchpedalreal>1.0)
// 						{
// 							clutchpedalreal = 1.0f;
// 						}
// 					}
// 					else
// 					{
// 						clutchin = false;
// 					}
// 					if(gear != -1)
// 					{
// 						if(gear<GearRatios.Count() && LinearVelocity.Length()>assistshiftspeed)
// 						{
// 							sassistdel = (float)GearAssistant[0]/2.0f;
// 							sassiststep = -4;
	
// 							clutchin = true;
// 							gasrestricted = true;
// 						}
// 						if(gear>1 && LinearVelocity.Length()<assistdownshiftspeed)
// 						{
// 							sassistdel = (float)GearAssistant[0]/2.0f;
// 							sassiststep = -2;
	
// 							clutchin = true;
// 							gasrestricted = false;
// 							revmatch = true;
	
// 						}
// 					}
// 				}
// 			}
// 			if(sassiststep == -4 && sassistdel<0)
// 			{
// 				sassistdel = (int)GearAssistant[0]/2;
// 				if(gear<GearRatios.Count())
// 				{
// 					actualgear += 1;
// 				}
// 				sassiststep = -3;
// 			}
// 			else if(sassiststep == -3 && sassistdel<0)
// 			{
// 				if(rpm>(float)GearAssistant[5])
// 				{
// 					clutchin = false;
// 				}
// 				if (sassistdel<-(float)GearAssistant[6]) {
// 					sassiststep = 0;
// 					gasrestricted = false;
// 				}
// 			}
// 			else if(sassiststep == -2 && sassistdel<0)
// 			{
// 				sassiststep = 0;
// 				if(gear>-1)
// 				{
// 					actualgear -= 1;
// 				}
// 				if(rpm>(float)GearAssistant[5])
// 				{
// 					clutchin = false;
// 				}
// 				gasrestricted = false;
// 				revmatch = false;
	
	
// 			}
// 			gear = actualgear;
	
// 		}
// 		else if(transmissionType == TransmissionType.Automatic)
// 		{
	
			
// 			clutchpedal = (rpm- (float)(AutoSettings[3])*(gaspedal*(float)(AutoSettings[2]) +(1.0f-(float)(AutoSettings[2]))) )/(float)(AutoSettings[4]);
			
			
// 			if((float)GearAssistant[1] != 2)
// 			{
// 				if(su)
// 				{
// 					su = false;
// 					if(gear<1)
// 					{
// 						actualgear += 1;
// 					}
// 				}
// 				if(sd)
// 				{
// 					sd = false;
// 					if(gear>-1)
// 					{
// 						actualgear -= 1;
// 					}
// 				}
// 			}
// 			else
// 			{
// 				if(gear == 0)
// 				{
// 					if(gas)
// 					{
// 						sassistdel -= 1;
// 						if(sassistdel<0)
// 						{
// 							actualgear = 1;
// 						}
// 					}
// 					else if(brake)
// 					{
// 						sassistdel -= 1;
// 						if(sassistdel<0)
// 						{
// 							actualgear = -1;
// 						}
// 					}
// 					else
// 					{
// 						sassistdel = 60;
// 					}
// 				}
// 				else if(LinearVelocity.Length()<5)
// 				{
// 					if(!gas && gear == 1 || !brake && gear == -1)
// 					{
// 						sassistdel = 60;
// 						actualgear = 0;
					
// 					}
// 				}
// 			}
// 			if(actualgear == -1)
// 			{
// 				ratio = ReverseRatio*FinalDriveRatio*RatioMult;
// 			}
// 			else
// 			{
// 				ratio = (float)GearRatios[gear-1]*FinalDriveRatio*RatioMult;
// 			}
// 			if(actualgear>0)
// 			{
// 				var lastratio = (float)GearRatios[gear-2]*FinalDriveRatio*RatioMult;
// 				su = false;
// 				sd = false;
// 				foreach(Wheel i in c_pws)
// 				{
// 					if((i.wv/(float)GearAssistant[2])>((float)(AutoSettings[0])*(gaspedal*(float)(AutoSettings[2]) +(1.0f-(float)(AutoSettings[2]))))/ratio)
// 					{
// 						su = true;
// 					}
// 					else if((i.wv/(float)GearAssistant[2])<(((float)(AutoSettings[0])-(float)(AutoSettings[1]))*(gaspedal*(float)(AutoSettings[2]) +(1.0-(float)(AutoSettings[2])))) /lastratio)
// 					{
// 						sd = true;
						
// 					}
// 				}
// 				if(su)
// 				{
// 					gear += 1;
// 				}
// 				else if(sd)
// 				{
// 					gear -= 1;
// 				}
// 				if(gear<1)
// 				{
// 					gear = 1;
// 				}
// 				else if(gear>GearRatios.Count())
// 				{
// 					gear = GearRatios.Count();
// 				}
// 			}
// 			else
// 			{
// 				gear = actualgear;
// 			}
// 		}
// 		else if(transmissionType == TransmissionType.ContinuouslyVariable)
// 		{
	
			
// 			clutchpedal = (rpm- (float)(AutoSettings[3])*(gaspedal*(float)(AutoSettings[2]) +(1.0f-(float)(AutoSettings[2]))) )/(float)(AutoSettings[4]);
			
// 	//            clutchpedal = 1;
			
// 			if((float)GearAssistant[1] != 2)
// 			{
// 				if(su)
// 				{
// 					su = false;
// 					if(gear<1)
// 					{
// 						actualgear += 1;
// 					}
// 				}
// 				if(sd)
// 				{
// 					sd = false;
// 					if(gear>-1)
// 					{
// 						actualgear -= 1;
// 					}
// 				}
// 			}
// 			else
// 			{
// 				if(gear == 0)
// 				{
// 					if(gas)
// 					{
// 						sassistdel -= 1;
// 						if(sassistdel<0)
// 						{
// 							actualgear = 1;
// 						}
// 					}
// 					else if(brake)
// 					{
// 						sassistdel -= 1;
// 						if(sassistdel<0)
// 						{
// 							actualgear = -1;
// 						}
// 					}
// 					else
// 					{
// 						sassistdel = 60;
// 					}
// 				}
// 				else if(LinearVelocity.Length()<5)
// 				{
// 					if(!gas && gear == 1 || !brake && gear == -1)
// 					{
// 						sassistdel = 60;
// 						actualgear = 0;
					
// 					}
// 				}
// 			}
// 			gear = actualgear;
// 			float wv = 0.0f        ;
			
// 			foreach(Wheel i in c_pws)
// 			{
// 				wv += i.wv/c_pws.Count();
				
// 			}
// 			cvtaccel -= (cvtaccel - (gaspedal*(float)CVTSettings[0] +(1.0f-(float)CVTSettings[0])))*(float)CVTSettings[1];
	
// 			float a = (float)CVTSettings[4]/((Mathf.Abs(wv)/10.0f)*cvtaccel +1.0f);
			
// 			if(a<(float)CVTSettings[5])
// 			{
// 				a = (float)CVTSettings[5];
	
// 			}
// 			ratio = ((float)CVTSettings[2]*10000000.0f)/(Mathf.Abs(wv)*(rpm*a) +1.0f);
			
// 			if(ratio>(float)CVTSettings[3])
// 			{
// 				ratio = (float)CVTSettings[3];
	
// 			}
// 		}
// 		else if(transmissionType == TransmissionType.SemiAuto)
// 		{
// 			clutchpedal = (rpm- (float)(AutoSettings[3])*(gaspedal*(float)(AutoSettings[2]) +(1.0f-(float)(AutoSettings[2]))) )/(float)(AutoSettings[4]);
	
// 			if(gear>0)
// 			{
// 				ratio = (float)GearRatios[gear-1]*FinalDriveRatio*RatioMult;
// 			}
// 			else if(gear == -1)
// 			{
// 				ratio = ReverseRatio*FinalDriveRatio*RatioMult;
			
// 			}
// 			if((float)GearAssistant[1]<2)
// 			{
// 				if(su)
// 				{
// 					su = false;
// 					if(gear<GearRatios.Count())
// 					{
// 						actualgear += 1;
// 					}
// 				}
// 				if(sd)
// 				{
// 					sd = false;
// 					if(gear>-1)
// 					{
// 						actualgear -= 1;
// 					}
// 				}
// 			}
// 			else
// 			{
// 				var assistshiftspeed = ((float)GearAssistant[4]/ratio)*(float)GearAssistant[2];
// 				var assistdownshiftspeed = ((float)GearAssistant[3]/Mathf.Abs(((float)GearRatios[gear-2]*FinalDriveRatio)*RatioMult))*(float)GearAssistant[2];
// 				if(gear == 0)
// 				{
// 					if(gas)
// 					{
// 						sassistdel -= 1;
// 						if(sassistdel<0)
// 						{
// 							actualgear = 1;
// 						}
// 					}
// 					else if(brake)
// 					{
// 						sassistdel -= 1;
// 						if(sassistdel<0)
// 						{
// 							actualgear = -1;
// 						}
// 					}
// 					else
// 					{
// 						sassistdel = 60;
// 					}
// 				}
// 				else if(LinearVelocity.Length()<5)
// 				{
// 					if(!gas && gear == 1 || !brake && gear == -1)
// 					{
// 						sassistdel = 60;
// 						actualgear = 0;
// 					}
// 				}
// 				if(sassiststep == 0)
// 				{
// 					if(gear != -1)
// 					{
// 						if(gear<GearRatios.Count() && LinearVelocity.Length()>assistshiftspeed)
// 						{
// 							actualgear += 1;
// 						}
// 						if(gear>1 && LinearVelocity.Length()<assistdownshiftspeed)
// 						{
// 							actualgear -= 1;
	
// 						}
// 					}
// 				}
// 			}
// 			gear = actualgear;
	
			
	
// 		}
// 		if(clutchpedal<0)
// 		{
// 			clutchpedal = 0;
// 		}
// 		else if(clutchpedal>1.0)
// 		{
// 			clutchpedal = 1.0f;
	
// 		}
// 	}
	
// 	public void drivetrain()
// 	{  
				
// 			rpmcsm -= (rpmcs - resistance);
	
// 			rpmcs += rpmcsm*ClutchElasticity;
			
// 			rpmcs -= rpmcs*(1.0f-clutchpedal);
			
// 			wob = ClutchWobble*clutchpedal;
			
// 			wob *= ratio*WobbleRate;
			
// 			rpmcs -= (rpmcs - resistance)*(1.0f/(wob +1.0f));
			
// 	//		torquereadout = multivariate(RiseRPM,TorqueRise,BuildUpTorque,EngineFriction,EngineDrag,OffsetTorque,rpm,DeclineRPM,DeclineRate,FloatRate,turbopsi,TurboAmount,EngineCompressionRatio,TurboEnabled,VVTRPM,VVT_BuildUpTorque,VVT_TorqueRise,VVT_RiseRPM,VVT_OffsetTorque,VVT_FloatRate,VVT_DeclineRPM,VVT_DeclineRate,SuperchargerEnabled,SCRPMInfluence,BlowRate,SCThreshold);
// 			if(gear<0)
// 			{
// 				rpm -= ((rpmcs*1.0f)/clock_mult)*(RevSpeed/1.475f);
// 			}
// 			else
// 			{
// 				rpm += ((rpmcs*1.0f)/clock_mult)*(RevSpeed/1.475f);
					
// 			}
// 			// if("")
// 			// {
// 			// 	rpm = 7000.0f;
// 			// 	Locking = 0.0f;
// 			// 	CoastLocking = 0.0f;
// 			// 	Centre_Locking = 0.0f;
// 			// 	Centre_CoastLocking = 0.0f;
// 			// 	Preload = 1.0f;
// 			// 	Centre_Preload = 1.0f;
// 			// 	ClutchFloatReduction = 0.0f;
					
// 			// }
// 			gearstress = (Mathf.Abs(resistance)*StressFactor)*clutchpedal;
// 			float stabled = ratio*0.9f +0.1f;
// 			ds_weight = DSWeight/stabled;
			
// 			whinepitch = Mathf.Abs(rpm/ratio)*1.5f;
			
// 			if(resistance>0.0)
// 			{
// 				locked = Mathf.Abs(resistance/ds_weight)*(CoastLocking/100.0f) + Preload;
// 			}
// 			else
// 			{
// 				locked = Mathf.Abs(resistance/ds_weight)*(Locking/100.0f) + Preload;
			
// 			}
// 			if(locked<0.0)
// 			{
// 				locked = 0.0f;
// 			}
// 			else if(locked>1.0)
// 			{
// 				locked = 1.0f;
				
				
// 			}
// 			if(wv_difference>0.0)
// 			{
// 				c_locked = Mathf.Abs(wv_difference)*(Centre_CoastLocking/10.0f) + Centre_Preload;
// 			}
// 			else
// 			{
// 				c_locked = Mathf.Abs(wv_difference)*(Centre_Locking/10.0f) + Centre_Preload;
// 			}
// 			if(c_locked<0.0 || c_pws.Count()<4)
// 			{
// 				c_locked = 0.0f;
// 			}
// 			else if(c_locked>1.0)
// 			{
// 				c_locked = 1.0f;
				
// 			}
// 			var maxd = VitaVehicleSimulation.fastest_wheel(c_pws);
// 			var mind = VitaVehicleSimulation.slowest_wheel(c_pws);
// 			float what = 0.0f;
			
// 			var floatreduction = ClutchFloatReduction;
	
// 			if(dsweightrun>0.0)
// 			{
// 				floatreduction = ClutchFloatReduction/dsweightrun;
// 			}
// 			else
// 			{
// 				floatreduction = 0.0f;
					
// 			}
// 			var stabling = -(GearRatioRatioThreshold -ratio*drivewheels_size)*ThresholdStable;
// 			if(stabling<0.0)
// 			{
// 				stabling = 0.0f;
				
// 			}
// 			currentstable = ClutchStable + stabling;
// 			currentstable *= (RevSpeed/1.475f);
	
// 			if(dsweightrun>0.0)
// 			{
// 				what = (rpm-(((rpmforce*floatreduction)*Mathf.Pow(currentstable,1.0f))/(ds_weight/dsweightrun)));
// 			}
// 			else
// 			{
// 				what = rpm;
				
// 			}
// 			if(gear<0.0)
// 			{
// 				dist = maxd.wv + what/ratio;
// 			}
// 			else
// 			{
// 				dist = maxd.wv - what/ratio;
		
// 			}
// 			dist *= (clutchpedal*clutchpedal);
			
// 			if(gear == 0)
// 			{
// 				dist *= 0.0f;
	
// 			}
// 			wv_difference = 0.0f;
// 			drivewheels_size = 0.0f;
// 			foreach(Wheel i in c_pws)
// 			{
// 				drivewheels_size += i.w_size/c_pws.Count();
// 				i.c_p = i.W_PowerBias;
// 				wv_difference += ((i.wv - what/ratio)/(c_pws.Count()))*(clutchpedal*clutchpedal);
// 				if(gear<0)
// 				{
// 					i.dist = dist*(1-c_locked) + (i.wv + what/ratio)*c_locked;
// 				}
// 				else
// 				{
// 					i.dist = dist*(1-c_locked) + (i.wv - what/ratio)*c_locked;
// 				}
// 				if(gear == 0)
// 				{
// 					i.dist *= 0.0f;
// 				}
// 			}
// 			GearAssistant[2] = drivewheels_size;
// 			resistance = 0.0f;
// 			dsweightrun = dsweight;
// 			dsweight = 0.0f;
// 			tcsweight = 0.0f;
// 			stress = 0.0f;
	
// 	}
	
// 	public void aero()
// 	{  
// 		var drag = DragCoefficient;
// 		var df = Downforce;
		
// 	//	var veloc = GlobalTransform.Basis.Orthonormalized().xform_inv(LinearVelocity);
// 		var veloc = GlobalTransform.Basis.Orthonormalized().Transposed() * (LinearVelocity);
		
// 	//	var torq = GlobalTransform.Basis.Orthonormalized().xform_inv(new Vector3(1,0,0));
// 		var torq = GlobalTransform.Basis.Orthonormalized().Transposed() * (new Vector3(1,0,0));
		
// 	//	apply_torque_impulse(GlobalTransform.Basis.Orthonormalized().xform( new Vector3(((-veloc.length()*0.3)*LiftAngle),0,0)  ) )
// 		ApplyTorqueImpulse(GlobalTransform.Basis.Orthonormalized() * ( new Vector3(((-veloc.Length()*0.3f)*LiftAngle),0,0)  ) );
		
// 		var vx = veloc.X*0.15;
// 		var vy = veloc.Z*0.15;
// 		var vz = veloc.Y*0.15;
// 		var vl = veloc.Length()*0.15f;
		
// 	//	var forc = GlobalTransform.Basis.Orthonormalized().xform(new Vector3(1,0,0))*(-vx*drag);
// 		var forc = GlobalTransform.Basis.Orthonormalized() * (new Vector3(1f,0f,0f))*(float)(-vx*drag);
// 	//	forc += GlobalTransform.Basis.Orthonormalized().xform(new Vector3(0,0,1))*(-vy*drag);
// 		forc += GlobalTransform.Basis.Orthonormalized() * (new Vector3(0,0,1))*(float)(-vy*drag);
// 	//	forc += GlobalTransform.Basis.Orthonormalized().xform(new Vector3(0,1,0))*(-vl*df -vz*drag);
// 		forc += GlobalTransform.Basis.Orthonormalized() * (new Vector3(0,1,0))*(float)(-vl*df -vz*drag);
		
// 		if(HasNode("DRAG_CENTRE"))
// 		{
// 	//		apply_impulse(GlobalTransform.Basis.Orthonormalized().xform($DRAG_CENTRE.position),forc)
// 			ApplyImpulse(forc, GlobalTransform.Basis.Orthonormalized() * (GetNode<Marker3D>("DRAG_CENTRE").Position));
// 		}
// 		else
// 		{
// 			ApplyCentralImpulse(forc);
			
	
// 		}
// 	}
	
// 	public void _physics_process(float delta)
// 	{  
		
		
// 		if(steering_angles.Count()>0)
// 		{
// 			max_steering_angle = 0.0f;
// 			foreach(var i in steering_angles)
// 			{
// 				max_steering_angle = Mathf.Max(max_steering_angle,i);
				
// 			}
// 			assistance_factor = 90.0f/max_steering_angle;
// 		}
// 		steering_angles = new Array(){};
		
// 		if(Use_Global_Control_Settings)
// 		{
// 			UseMouseSteering = VitaVehicleSimulation.UseMouseSteering;
// 			UseAccelerometreSteering = VitaVehicleSimulation.UseAccelerometreSteering;
// 			SteerSensitivity = VitaVehicleSimulation.SteerAmountDecay;
// 			KeyboardSteerSpeed = VitaVehicleSimulation.KeyboardSteerSpeed;
// 			KeyboardReturnSpeed = VitaVehicleSimulation.KeyboardReturnSpeed;
// 			KeyboardCompensateSpeed = VitaVehicleSimulation.KeyboardCompensateSpeed;
	
// 			SteerAmountDecay = VitaVehicleSimulation.SteerAmountDecay;
// 			SteeringAssistance = VitaVehicleSimulation.SteeringAssistance;
// 			SteeringAssistanceAngular = VitaVehicleSimulation.SteeringAssistanceAngular;
			
// 			GearAssistant[1] = VitaVehicleSimulation.GearAssistant;
	
		
// 		}
// 		if(Input.IsActionJustPressed("toggle_debug_mode"))
// 		{
// 			if(Debug_Mode)
// 			{
// 				Debug_Mode = false;
// 			}
// 			else
// 			{
// 				Debug_Mode = true;
		
// 			}
// 		}
// 		if(Input.IsActionJustPressed("eStop"))
// 		{
// 			ffb.Stop();
// 		}
// 		if(Input.IsActionPressed("ToggleFFB"))
// 		{
// 			ffb.TestFFB();
		
// 	//	velocity = GlobalTransform.Basis.Orthonormalized().xform_inv(LinearVelocity);
// 		}
// 		velocity = GlobalTransform.Basis.Orthonormalized().Transposed() * (LinearVelocity);
// 	//	rvelocity = GlobalTransform.Basis.Orthonormalized().xform_inv(AngularVelocity);
// 		rvelocity = GlobalTransform.Basis.Orthonormalized().Transposed() * (AngularVelocity);
	
// 		if(Mass != Weight/10.0)
// 		{
// 			Mass = Weight/10.0f;
// 		}
// 		aero();
		
// 		gforce = (LinearVelocity - pastvelocity)*((0.30592f/9.806f)*60.0f);
// 		pastvelocity = LinearVelocity;
		
// 	//	gforce = GlobalTransform.Basis.Orthonormalized().xform_inv(gforce);
// 		gforce = GlobalTransform.Basis.Orthonormalized().Transposed() * (gforce);
		
// 		controls();
		
// 		ratio = 10.0f;
	
// 		sassistdel -= 1;
	
// 		transmission();
		
// 		limits();
	
// 		var steeroutput = steer;
		
// 		float uhh = (max_steering_angle/90.0f)*(max_steering_angle/90.0f);
// 		uhh *= 0.5f;
// 		steeroutput *= Mathf.Abs(steer)*(uhh) +(1.0f-uhh);
		
// 		if(Mathf.Abs(steeroutput)>0.0)
// 		{
// 			steering_geometry = new Array(){-Steer_Radius/steeroutput,AckermannPoint};
	
	
// 		}
// 		abspump -= 1    ;
		
// 		if(abspump<0)
// 		{
// 			brake_allowed += (float)ABS[4];
// 		}
// 		else
// 		{
// 			brake_allowed -= (float)ABS[4];
			
// 		}
// 		if(brake_allowed<0.0)
// 		{
// 			brake_allowed = 0.0f;
// 		}
// 		else if(brake_allowed>1.0)
// 		{
// 			brake_allowed = 1.0f;
		
// 		}
// 		brakeline = brakepedal*brake_allowed;
		
// 		if(brakeline<0.0)
// 		{
// 			brakeline = 0.0f;
		
// 		}
// 		limdel -= 1;
		
// 		if(limdel<0)
// 		{
// 			throttle -= (throttle - (gaspedal/(tcsweight*clutchpedal +1.0f)))*(ThrottleResponse/clock_mult);
// 		}
// 		else
// 		{
// 			throttle -= throttle*(ThrottleResponse/clock_mult);
	
// 		}
// 		if(rpm>RPMLimit)
// 		{
// 			if(throttle>ThrottleLimit)
// 			{
// 				throttle = ThrottleLimit;
// 				limdel = LimiterDelay;
// 			}
// 		}
// 		else if(rpm<IdleRPM)
// 		{
// 			if(throttle<ThrottleIdle)
// 			{
// 				throttle = ThrottleIdle;
	
	
// 			}
// 		}
// 		float stab = 300.0f;
		
	
// 		float thr = 0.0f;
	
// 		if(TurboEnabled)
// 		{
// 			thr = (throttle-SpoolThreshold)/(1-SpoolThreshold);
				
// 			if(boosting>thr)
// 			{
// 				boosting = thr;
// 			}
// 			else
// 			{
// 				boosting -= (boosting - thr)*TurboEfficiency;
			
// 			}
// 			turbopsi += (boosting*rpm)/((TurboSize/Compressor)*60.9f);
	
// 			turbopsi -= turbopsi*BlowoffRate;
				
// 			if(turbopsi>MaxPSI)
// 			{
// 				turbopsi = MaxPSI;
				
// 			}
// 			if (turbopsi<-TurboVacuum)
// 				turbopsi = -TurboVacuum;
// 		}
// 		else if(SuperchargerEnabled)
// 		{
// 			scrpm = rpm*SCRPMInfluence;
// 			turbopsi = (scrpm/10000.0f)*BlowRate -SCThreshold;
// 			if(turbopsi>MaxPSI)
// 			{
// 				turbopsi = MaxPSI;
// 			}
// 			if(turbopsi<0.0)
// 			{
// 				turbopsi = 0.0f;
// 			}
// 		}
// 		else
// 		{
// 			turbopsi = 0.0f;
	
// 		}
// 		vvt = rpm>VVTRPM;
		
// 		float torque = 0.0f;
		
			
// 		if(vvt)
// 		{
// 			float f = rpm-VVT_RiseRPM;
// 			if(f<0.0)
// 			{
// 				f = 0.0f;
// 			}
// 			torque = (rpm*VVT_BuildUpTorque +VVT_OffsetTorque + (f*f)*(VVT_TorqueRise/10000000.0f))*throttle;
// 			torque += ( (turbopsi*TurboAmount) * (EngineCompressionRatio*0.609f) );
// 			var j = rpm-VVT_DeclineRPM;
// 			if(j<0.0)
// 			{
// 				j = 0.0f;
// 			}
// 			torque /= (j*(j*VVT_DeclineSharpness +(1.0f-VVT_DeclineSharpness)))*(VVT_DeclineRate/10000000.0f) +1.0f;
// 			torque /= Mathf.Abs(rpm*Math.Abs(rpm))*(VVT_FloatRate/10000000.0f) +1.0f;
// 		}
// 		else
// 		{
// 			var f = rpm-RiseRPM;
// 			if(f<0.0)
// 			{
// 				f = 0.0f;
// 			}
// 			torque = (rpm*BuildUpTorque +OffsetTorque + (f*f)*(TorqueRise/10000000.0f))*throttle;
// 			torque += ( (turbopsi*TurboAmount) * (EngineCompressionRatio*0.609f) );
// 			var j = rpm-DeclineRPM;
// 			if(j<0.0)
// 			{
// 				j = 0.0f;
// 			}
// 			torque /= (j*(j*DeclineSharpness +(1.0f-DeclineSharpness)))*(DeclineRate/10000000.0f) +1.0f;
// 			torque /= Mathf.Abs(rpm*Mathf.Abs(rpm))*(FloatRate/10000000.0f) +1.0f;
			
// 		}
// 		rpmforce = (rpm/(Mathf.Abs(rpm*Mathf.Abs(rpm))/(EngineFriction/clock_mult) +1.0f))*1.0f;
// 		if(rpm<DeadRPM)
// 		{
// 			torque = 0.0f;
// 			rpmforce /= 5.0f;
// 			stalled = 1.0f -rpm/DeadRPM;
// 		}
// 		else
// 		{
// 			stalled = 0.0f;
// 		}
// 		rpmforce += (rpm*(EngineDrag/clock_mult))*1.0f;
// 		rpmforce -= (torque/clock_mult)*1.0f;
// 		rpm -= rpmforce*RevSpeed;
			
// 		drivetrain();
	
// 	}
	
// 	public Array front_wheels = new Array(){};
// 	public Array rear_wheels = new Array(){};
// 	public float front_load = 0.0f;
// 	public float total = 0.0f;
	
// 	public Array weight_dist = new Array(){0.0,0.0};
	
// 	public void _process(float delta)
// 	{  
// 		if(Debug_Mode)
// 		{
// 			front_wheels = new Array(){};
// 			rear_wheels = new Array(){};
// 			foreach(Wheel i in GetChildren())
// 			{
// 				if(i.Name == "TyreSettings")
// 				{
// 					if(i.Position.Z>0)
// 					{
// 						front_wheels.Append(i);
// 					}
// 					else
// 					{
// 						rear_wheels.Append(i);
						
// 					}
// 				}
// 			}
// 			front_load = 0.0f;
// 			total = 0.0f;
			
// 			foreach(Wheel f in front_wheels)
// 			{
// 				front_load += f.directional_force.Y;
// 				total += f.directional_force.Y;
// 			}
// 			foreach(Wheel r in rear_wheels)
// 			{
// 				front_load -= r.directional_force.Y;
// 				total += r.directional_force.Y;
				
// 			}
// 			if(total>0)
// 			{
// 				weight_dist[0] = (front_load/total)*0.5f + 0.5f;
// 				weight_dist[1] = 1.0f-(float)weight_dist[0];
		
// 			}
// 		}
// 		readout_torque = VitaVehicleSimulation.multivariate(RiseRPM,TorqueRise,BuildUpTorque,EngineFriction,EngineDrag,OffsetTorque,rpm,DeclineRPM,DeclineRate,FloatRate,MaxPSI,TurboAmount,EngineCompressionRatio,TurboEnabled,VVTRPM,VVT_BuildUpTorque,VVT_TorqueRise,VVT_RiseRPM,VVT_OffsetTorque,VVT_FloatRate,VVT_DeclineRPM,VVT_DeclineRate,SuperchargerEnabled,SCRPMInfluence,BlowRate,SCThreshold,DeclineSharpness,VVT_DeclineSharpness);
		
		
	
	
// 	}
	
	
	
// }