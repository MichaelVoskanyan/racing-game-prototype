
// using System;
// using Godot;
// using Dictionary = Godot.Collections.Dictionary;
// using Array = Godot.Collections.Array;
// using System.Linq;


// public partial class Wheel : RayCast3D
// {
	 
// 	[Export] Dictionary RealismOptions = new Dictionary(){
// 	};
	
// 	[Export] public bool Steer = true;
// 	[Export] public string Differed_Wheel = "";
// 	[Export] public string SwayBarConnection = "";
	
// 	[Export] public float W_PowerBias = 1.0f;
// 	[Export] Dictionary TyreSettings = new Dictionary(){
// 		{"GripInfluence", 1.0},
// 		{"Width (mm)", 185.0},
// 		{"Aspect Ratio", 60.0},
// 		{"Rim Size (in)", 14.0
// 		}};
// 	[Export] public float TyrePressure = 30.0f;
// 	[Export] public float Camber = 0.0f;
// 	[Export] public float Caster = 0.0f;
// 	[Export] public float Toe = 0.0f;
	
// 	[Export] Dictionary CompoundSettings = new Dictionary(){
// 		{"OptimumTemp", 50.0},
// 		{"Stiffness", 1.0},
// 		{"TractionFactor", 1.0},
// 		{"DeformFactor", 1.0},
// 		{"ForeFriction", 0.125},
// 		{"ForeStiffness", 0.0},
// 		{"GroundDragAffection", 1.0},
// 		{"BuildupAffection", 1.0},
// 		{"CoolRate", 0.000075}};
	
// 	[Export] public float S_Stiffness = 47.0f;
// 	[Export] public float S_Damping = 3.5f;
// 	[Export] public float S_ReboundDamping = 3.5f;
// 	[Export] public float S_RestLength = 0.0f;
// 	[Export] public float S_MaxCompression = 0.5f;
// 	[Export] public float A_InclineArea = 0.2f;
// 	[Export] public float A_ImpactForce = 1.5f;
// 	[Export] public float AR_Stiff = 0.5f;
// 	[Export] public float AR_Elast = 0.1f;
// 	[Export] public float B_Torque = 15.0f;
// 	[Export] public float B_Bias = 1.0f;
// 	[Export] public float B_Saturation = 1.0f ;// leave this at 1.0 unless you have a heavy vehicle with large wheels, set it higher depending on how big it is
// 	[Export] public float HB_Bias = 0.0f;
// 	[Export] public float A_Geometry1 = 1.15f;
// 	[Export] public float A_Geometry2 = 1.0f;
// 	[Export] public float A_Geometry3 = 0.0f;
// 	[Export] public float A_Geometry4 = 0.0f;
// 	[Export] public NodePath Solidify_Axles = new NodePath();
// 	[Export] public bool ContactABS = true;
// 	[Export] public string ESP_Role = "";
// 	[Export] public bool ContactBTCS = false;
// 	[Export] public bool ContactTTCS = false;
	
// 	public float dist = 0.0f;
// 	public float w_size = 1.0f;
// 	public float w_size_read = 1.0f;
// 	public float w_weight_read = 0.0f;
// 	public float w_weight = 0.0f;
// 	public float wv = 0.0f;
// 	public float wv_ds = 0.0f;
// 	public float wv_diff = 0.0f;
// 	public float c_tp = 0.0f;
// 	public float effectiveness = 0.0f;
	
// 	public float angle = 0.0f;
// 	public float snap = 0.0f;
// 	public float absolute_wv = 0.0f;
// 	public float absolute_wv_brake = 0.0f;
// 	public float absolute_wv_diff = 0.0f;
// 	public float output_wv = 0.0f;
// 	public float offset = 0.0f;
// 	public float c_p = 0.0f;
// 	public float wheelpower = 0.0f;
// 	public float wheelpower_global = 0.0f;
// 	public float stress = 0.0f;
// 	public float rolldist = 0.0f;
// 	public float rd = 0.0f;
// 	public float c_camber = 0.0f;
// 	public float cambered = 0.0f;
	
// 	public float rollvol = 0.0f;
// 	public float sl = 0.0f;
// 	public float skvol = 0.0f;
// 	public float skvol_d = 0.0f;
// 	public Vector3 velocity = new Vector3(0,0,0);
// 	public Vector3 velocity2 = new Vector3(0,0,0);
// 	public float compress = 0.0f;
// 	public float compensate = 0.0f;
// 	public float axle_position = 0.0f;
	
// 	public float heat_rate = 1.0f;
// 	public float wear_rate = 1.0f;
	
// 	public float ground_bump = 0.0f;
// 	public bool ground_bump_up = false;
// 	public float ground_bump_frequency = 0.0f;
// 	public float ground_bump_frequency_random = 1.0f;
// 	public float ground_bump_height = 0.0f;
	
// 	public float ground_friction = 1.0f;
// 	public float ground_stiffness = 1.0f;
// 	public float fore_friction = 0.0f;
// 	public float fore_stiffness = 0.0f;
// 	public float drag = 0.0f;
// 	public float ground_builduprate = 0.0f;
// 	public bool ground_dirt = false;
// 	public Vector3 hitposition = new Vector3(0,0,0);
	
// 	public float cache_tyrestiffness = 0.0f;
// 	public float cache_friction_action = 0.0f;
// 	Car car;
	
// 	public void _ready()
// 	{  
// 		c_tp = TyrePressure;
// 		car = GetParent<Car>();
// 	}
	
// 	public void power()
// 	{  
// 		if(c_p != 0)
// 		{
// 			dist *= (car.clutchpedal*car.clutchpedal)/(car.currentstable);
// 			float dist_cache = dist;
			
// 			var tol = (.1475f/1.3558f)*car.ClutchGrip;
	
// 			if(dist_cache>tol)
// 			{
// 				dist_cache = tol;
// 			}
// 			else if (dist_cache<-tol)
// 				dist_cache = -tol;
			
// 			var dist2 = dist_cache;
	
// 			car.dsweight += c_p;
// 			car.stress += stress*c_p;
			
// 			if(car.dsweightrun>0.0)
// 			{
// 				if(car.rpm>car.DeadRPM)
// 				{
// 					wheelpower -= (((dist2/car.ds_weight)/(car.dsweightrun/2.5f))*c_p)/w_weight;
// 				}
// 				car.resistance += (((dist_cache*(10.0f))/car.dsweightrun)*c_p);
	
// 			}
// 		}
// 	}
	
// 	public void diffs()
// 	{  
// 		if(car.locked>0.0)
// 		{
// 			if(Differed_Wheel != "")
// 			{
// 				var d_w = car.GetNode<Wheel>(Differed_Wheel);
// 				snap = Mathf.Abs(d_w.wheelpower_global)/(car.locked*16.0f) +1.0f;
// 				absolute_wv = output_wv+(offset*snap);
// 				var distanced2 = Mathf.Abs(absolute_wv - d_w.absolute_wv_diff)/(car.locked*16.0f);
// 				distanced2 += Mathf.Abs(d_w.wheelpower_global)/(car.locked*16.0f);
// 				if(distanced2<snap)
// 				{
// 					distanced2 = snap;
// 				}
// 				distanced2 += 1.0f/cache_tyrestiffness;
// 				if(distanced2>0.0)
// 				{
// 					wheelpower += -((absolute_wv_diff - d_w.absolute_wv_diff)/distanced2);
	
// 				}
// 			}
// 		}
// 	}
	
// 	public void sway()
// 	{  
// 		if(SwayBarConnection != "")
// 		{
// 			Wheel linkedwheel = car.GetNode<Wheel>(SwayBarConnection);
// 			rolldist = rd - linkedwheel.rd;
	
	
// 		}
// 	}
	
// 	public Vector3 directional_force = new Vector3(0,0,0);
// 	public Vector2 slip_perc = new Vector2(0,0);
// 	public float slip_perc2 = 0.0f;
// 	public float slip_percpre = 0.0f;
	
// 	public Vector3 velocity_last = new Vector3(0,0,0);
// 	public Vector3 velocity2_last = new Vector3(0,0,0);
	
// 	public void _physics_process(float _delta)
// 	{  
// 		var translation = Position;
// 		var cast_to = TargetPosition;
// 		var global_translation = GlobalPosition;
// 		var last_translation = Position;
		
// 		if(Steer && Mathf.Abs(car.steer)>0)
// 		{
// 			float form1 = 0.0f;
// 			float form2 = (float)car.steering_geometry[1] -translation.X;
// 			Transform3D lasttransform = GlobalTransform;
			
// 			LookAtFromPosition(translation,new Vector3((float)car.steering_geometry[0],0.0f,(float)car.steering_geometry[1]));
			
// 			// just making this use origin fixed it. lol
// 			GlobalTransform.Origin = lasttransform.Origin;
			
// 			if(car.steer > 0.0)
// 			{
// 				RotateObjectLocal(new Vector3(0,1,0),-Mathf.DegToRad(90.0f));
// 			}
// 			else
// 			{
// 				RotateObjectLocal(new Vector3(0,1,0),Mathf.DegToRad(90.0f));
			
// 			}
// 			var roter = GlobalRotation.Y;
			
// 			LookAtFromPosition(translation,new Vector3(car.Steer_Radius,0,(float)car.steering_geometry[1]),new Vector3(0,1,0));
// 			// this one too
// 			GlobalTransform.Origin = lasttransform.Origin;
// 			RotateObjectLocal(new Vector3(0,1,0),Mathf.DegToRad(90.0f));
// 			var roter_estimateed = Mathf.RadToDeg(GlobalRotation.Y);
			
// 			GetParent<Car>().steering_angles.Append(roter_estimateed);
			
// 			RotationDegrees = new Vector3(0,0,0);
// 			Rotation = new Vector3(0,0,0);
			
// 			Rotation.Y = roter;
			
// 			int xgt0 = (translation.X > 0) ? 1 : 0;
// 			int xlt0 = (translation.X < 0) ? 1 : 0;
// 			RotationDegrees += new Vector3(0,-((Toe*((xgt0)) -Toe*(float)(xlt0))),0);
			
// 		}
// 		else
// 		{
// 			int fchk = (translation.X > 0) ? 1 : 0;
// 			int schk = (translation.X < 0) ? 1 : 0;
// 			RotationDegrees = new Vector3(0,-((Toe*fchk) -(Toe*schk)),0);
		
// 		}
// 		translation = last_translation;
		
// 		int fchk = (translation.X>0.0) ? 1 : 0;
// 		int schk = (translation.X<0.0) ? 1 : 0;
// 		c_camber = Camber +Caster*Rotation.Y*fchk -Caster*Rotation.Y*schk;
		
// 		directional_force = new Vector3(0,0,0);
		
// 		GetNode<Marker3D>("velocity").Position = new Vector3(0,0,0);
		
		
// 		w_size = ((Mathf.Abs((int)(TyreSettings["Width (mm)"]))*((Mathf.Abs((int)(TyreSettings["Aspect Ratio"]))*2.0f)/100.0f) + Mathf.Abs((int)(TyreSettings["Rim Size (in)"]))*25.4f)*0.003269f)/2.0f;
// 		w_weight = Mathf.Pow(w_size,2);
		
// 		w_size_read = w_size;
// 		if(w_size_read<1.0)
// 		{
// 			w_size_read = 1.0f;
// 		}
// 		if(w_weight_read<1.0)
// 		{
// 			w_weight_read = 1.0f;
		
// 		}
// 		GetNode<Marker3D>("velocity2").GlobalPosition = GetNode<MeshInstance3D>("geometry").GlobalPosition;
		
// 		GetNode<Marker3D>("velocity/step").GlobalPosition = velocity_last;
// 		GetNode<Marker3D>("velocity2/step").GlobalPosition = velocity2_last;
// 		velocity_last = GetNode<Marker3D>("velocity").GlobalPosition;
// 		velocity2_last = GetNode<Marker3D>("velocity2").GlobalPosition;
		
// 		velocity = -GetNode<Marker3D>("velocity/step").Position*60.0f;
// 		velocity2 = -GetNode<Marker3D>("velocity2/step").Position*60.0f;
		
// 		GetNode<Marker3D>("velocity").Rotation = new Vector3(0,0,0);
// 		GetNode<Marker3D>("velocity2").Rotation = new Vector3(0,0,0);
		
// 		// VARS
// 		var elasticity = S_Stiffness;
// 		var damping = S_Damping;
// 		var damping_rebound = S_ReboundDamping;
		
// 		var swaystiff = AR_Stiff;
// 		var swayelast = AR_Elast;
		
// 		var s = rolldist;
// 		if(s<-1.0)
// 		{
// 			s = -1.0f;
// 		}
// 		else if(s>1.0)
// 		{
// 			s = 1.0f;
			
// 		}
// 		elasticity *= swayelast*s +1.0f;
// 		damping *= swaystiff*s +1.0f;
// 		damping_rebound *= swaystiff*s +1.0f;
		
// 		if(elasticity<0.0)
// 		{
// 			elasticity = 0.0f;
			
// 		}
// 		if(damping<0.0)
// 		{
// 			damping = 0.0f;
			
// 		}
// 		if(damping_rebound<0.0)
// 		{
// 			damping_rebound = 0.0f;
// 		}
// 		sway();
		
// 		var tyre_maxgrip = (float)TyreSettings["GripInfluence"]/(float)CompoundSettings["TractionFactor"];
	
	
// 		float tyre_stiffness2 = Mathf.Abs((int)(TyreSettings["Width (mm)"]))/(Mathf.Abs((int)(TyreSettings["Aspect Ratio"]))/1.5f);
	
// 		var deviding = (new Vector2(velocity.X,velocity.Z).Length()/50.0f +0.5f)*(float)CompoundSettings["DeformFactor"];
	
// 		deviding /= ground_stiffness +fore_stiffness*(float)CompoundSettings["ForeStiffness"];
// 		if(deviding<1.0)
// 		{
// 			deviding = 1.0f;
// 		}
// 		tyre_stiffness2 /= deviding;
		
	
// 		float tyre_stiffness = (tyre_stiffness2*((c_tp/30.0f)*0.1f +0.9f) )*(float)CompoundSettings["Stiffness"] +effectiveness;
// 		if(tyre_stiffness<1.0)
// 		{
// 			tyre_stiffness = 1.0f;
	
// 		}
// 		cache_tyrestiffness = tyre_stiffness;
			
// 		absolute_wv = output_wv+(offset*snap) -compensate*1.15296f;
// 		absolute_wv_brake = output_wv+((offset/w_size_read)*snap) -compensate*1.15296f;
// 		absolute_wv_diff = output_wv;
		
// 		wheelpower = 0.0f;
	
// 		var braked = car.brakeline*B_Bias + car.handbrakepull*HB_Bias;
// 		if(braked>1.0)
// 		{
// 			braked = 1.0f;
// 		}
// 		var bp = (B_Torque*braked)/w_weight_read;
	
// 		if(car.actualgear != 0)
// 		{
// 			if(car.dsweightrun>0.0)
// 			{
// 				bp += ((car.stalled*(c_p/car.ds_weight))*car.clutchpedal)*(((500.0f/(car.RevSpeed*100.0f))/(car.dsweightrun/2.5f))/w_weight_read);
// 			}
// 		}
// 		if(bp>0.0)
// 		{
// 			if(Mathf.Abs(absolute_wv)>0.0)
// 			{
// 				var distanced = Mathf.Abs(absolute_wv)/bp;
// 				distanced -= car.brakeline;
// 				if(distanced<snap*(w_size_read/B_Saturation))
// 				{
// 					distanced = snap*(w_size_read/B_Saturation);
// 				}
// 				wheelpower += -absolute_wv/distanced;
// 			}
// 			else
// 			{
// 				wheelpower += -absolute_wv;
	
// 			}
// 		}
// 		wheelpower_global = wheelpower;
		
// 		power();
// 		diffs();
	
// 		snap = 1.0f;
// 		offset = 0.0f;
	
// 		// WHEEL
// 		if(IsColliding())
// 		{
// 			if(GetCollider().GetMetaList().Contains("drag"))
// 			{
// 				drag = (float)GetCollider().Get("drag")*(float)CompoundSettings["GroundDragAffection"]*(float)CompoundSettings["GroundDragAffection"];
// 			}
// 			if( GetCollider().GetMetaList().Contains("ground_friction"))
// 			{
// 				ground_friction = (float)GetCollider().Get("ground_friction");
// 			}
// 			if(GetCollider().GetMetaList().Contains("fore_friction"))
// 			{
// 				fore_friction = (float)GetCollider().Get("fore_friction");
// 			}
// 			if(GetCollider().GetMetaList().Contains("ground_stiffness"))
// 			{
// 				ground_stiffness = (float)GetCollider().Get("ground_stiffness");
// 			}
// 			if(GetCollider().GetMetaList().Contains("fore_stiffness"))
// 			{
// 				fore_stiffness = (float)GetCollider().Get("fore_stiffness");
// 			}
// 			if(GetCollider().GetMetaList().Contains("ground_builduprate"))
// 			{
// 				ground_builduprate = (float)GetCollider().Get("ground_builduprate")*(float)CompoundSettings["BuildupAffection"];
// 			}
// 			if(GetCollider().GetMetaList().Contains("ground_dirt"))
// 			{
// 				ground_dirt = (bool)GetCollider().Get("ground_dirt");
// 			}
// 			if(GetCollider().GetMetaList().Contains("ground_bump_frequency"))
// 			{
// 				ground_bump_frequency = (float)GetCollider().Get("ground_bump_frequency");
// 			}
// 			if(GetCollider().GetMetaList().Contains("ground_bump_frequency_random"))
// 			{
// 				ground_bump_frequency_random = (float)GetCollider().Get("ground_bump_frequency_random") +1.0f;
// 			}
// 			if(GetCollider().GetMetaList().Contains("ground_bump_height"))
// 			{
// 				ground_bump_height = (float)GetCollider().Get("ground_bump_height");
// 			}
// 			if(GetCollider().GetMetaList().Contains("wear_rate"))
// 			{
// 				wear_rate = (float)GetCollider().Get("wear_rate");
// 			}
// 			if(GetCollider().GetMetaList().Contains("heat_rate"))
// 			{
// 				heat_rate = (float)GetCollider().Get("heat_rate");
// 			}
// 			if(ground_bump_up)
// 			{
// 				ground_bump -= (float) GD.RandRange(ground_bump_frequency/ground_bump_frequency_random,ground_bump_frequency*ground_bump_frequency_random)*(velocity.Length()/1000.0f);
// 				if(ground_bump<0.0)
// 				{
// 					ground_bump = 0.0f;
// 					ground_bump_up = false;
// 				}
// 			}
// 			else         
// 			{
// 				ground_bump += (float) GD.RandRange(ground_bump_frequency/ground_bump_frequency_random,ground_bump_frequency*ground_bump_frequency_random)*(velocity.Length()/1000.0f);
// 				if(ground_bump>1.0)
// 				{
// 					ground_bump = 1.0f;
// 					ground_bump_up = true;
	
// 				}
// 			}
// 			var suspforce = VitaVehicleSimulation.suspension(this,S_MaxCompression,A_InclineArea,A_ImpactForce,S_RestLength, elasticity,damping,damping_rebound, velocity.y,Mathf.Abs(cast_to.Y),global_translation,GetCollisionPoint(),car.Mass,ground_bump,ground_bump_height);
// 			compress = suspforce;
			
			
// 			// FRICTION
// 			var grip = (suspforce*tyre_maxgrip)*(ground_friction +fore_friction*(float)CompoundSettings["ForeFriction"]);
// 			stress = grip;
// 			float rigidity = 0.67f;
	
// 			var distw = velocity2.Z - wv*w_size;
// 			wv += (wheelpower*(1.0f-(1.0f/tyre_stiffness)));
// 			var disty = velocity2.Z - wv*w_size;
	
// 			offset = disty/w_size;
// 			if(offset>grip)
// 			{
// 				offset = grip;
// 			}
// 			else if (offset<-grip)
// 				offset = -grip;
	
// 			var distx = velocity2.X;
	
// 			var compensate2 = suspforce;
// 	//		var grav_incline = $geometry.GlobalTransform.Basis.Orthonormalized().xform_inv(new Vector3(0,1,0)).x
// 			var grav_incline = (GetNode<MeshInstance3D>("geometry").GlobalTransform.Basis.Orthonormalized().Transposed() * (new Vector3(0,1,0))).X;
// 	//		var grav_incline2 = $geometry.GlobalTransform.Basis.Orthonormalized().xform_inv(new Vector3(0,1,0)).z
// 			var grav_incline2 = (GetNode<MeshInstance3D>("geometry").GlobalTransform.Basis.Orthonormalized().Transposed() * (new Vector3(0,1,0))).Z;
// 			compensate = grav_incline2*(compensate2/tyre_stiffness);
			
// 			distx -= (grav_incline*(compensate2/tyre_stiffness))*1.1;
	
// 			disty *= tyre_stiffness;
// 			distw *= tyre_stiffness;
// 			distx *= tyre_stiffness;
	
// 			distx -= Mathf.Atan2(Mathf.Abs(wv),1.0f)*((angle*10.0f)*w_size);
	
// 			if(grip>0)
// 			{
	
// 				var slip = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(disty),2.0) + Mathf.Pow(Mathf.Abs(distx),2.0))/grip;
				
// 				slip_percpre = slip/tyre_stiffness;
				
// 				slip /= slip*ground_builduprate +1;
// 				slip -= CompoundSettings["TractionFactor"];
// 				if(slip<0)
// 				{
// 					slip = 0;
	
// 				}
// 				var slip_sk = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(disty),2.0) + Mathf.Pow(Mathf.Abs((distx)*2.0),2.0))/grip;
// 				slip_sk /= slip*ground_builduprate +1;
// 				slip_sk -= CompoundSettings["TractionFactor"];
// 				if(slip_sk<0)
// 				{
// 					slip_sk = 0;
	
	
// 				}
// 				var slipw = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(0.0),2.0) + Mathf.Pow(Mathf.Abs(distx),2.0))/grip;
// 				slipw /= slipw*ground_builduprate +1.0f;
// 				var forcey = -disty/(slip +1.0);
// 				var forcex = -distx/(slip +1.0);
	
// 				if(Mathf.Abs(disty) /(tyre_stiffness/3.0)>(car.ABS[0]/grip)*(ground_friction*ground_friction) && car.ABS[3] && Mathf.Abs(velocity.Z)>(float)car.ABS[2] && ContactABS)
// 				{
// 					car.abspump = (float)car.ABS[1];
// 					if(Mathf.Abs(distx) /(tyre_stiffness/3.0)>(car.ABS[5]/grip)*(ground_friction*ground_friction))
// 					{
// 						car.abspump = (float)car.ABS[6];
			
// 					}
// 				}
// 				var yesx = Mathf.Abs(forcex);
// 				if(yesx>1.0)
// 				{
// 					yesx = 1.0;
// 				}
// 				var smoothx = yesx*yesx;
// 				if(smoothx>1.0)
// 				{
// 					smoothx = 1.0;
// 				}
// 				var yesy = Mathf.Abs(forcey);
// 				if(yesy>1.0)
// 				{
// 					yesy = 1.0;
// 				}
// 				var smoothy = yesy*1.0;
// 				if(smoothy>1.0)
// 				{
// 					smoothy = 1.0;
// 				}
// 				forcex /= (smoothx*(rigidity) +(1.0-rigidity));
// 				forcey /= (smoothy*(rigidity) +(1.0-rigidity));
					
// 				var distyw = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(disty),2.0) + Mathf.Pow(Mathf.Abs(distx),2.0));
// 				var tr2 = (grip/tyre_stiffness);
// 				var afg = tyre_stiffness*tr2;
// 				distyw /= (float)CompoundSettings["TractionFactor"];
// 				if(distyw<afg)
// 				{
// 					distyw = afg;
					
// 				}
// 				var ok = ((distyw/tyre_stiffness)/grip)/w_size;
				
// 				if(ok>1.0)
// 				{
// 					ok = 1.0f;
					
// 				}
// 				snap = ok*w_weight_read;
// 				if(snap>1.0)
// 				{
// 					snap = 1.0f;
				
// 				}
// 				wv -= forcey*ok;
				
// 				cache_friction_action = forcey*ok;
				
// 				wv += (wheelpower*(1.0f/tyre_stiffness));
	
// 				rollvol = velocity.Length()*grip;
	
// 				sl = slip_sk-tyre_stiffness;
// 				if(sl<0.0)
// 				{
// 					sl = 0.0f;
// 				}
// 				skvol = sl/4.0f;
				
// 	//			skvol *= skvol
	
// 				skvol_d = slip*25.0;
// 			}
// 		}
// 		else
// 		{
// 			wv += wheelpower;
// 			stress = 0.0f;
// 			rollvol = 0.0f;
// 			sl = 0.0f;
// 			skvol = 0.0f;
// 			skvol_d = 0.0f;
// 			compress = 0.0f;
// 			compensate = 0.0f;
		
// 		}
// 		slip_perc = new Vector2(0,0);
// 		slip_perc2 = 0.0f;
		
// 		wv_diff = wv;
// 		// FORCE
// 		if(IsColliding())
// 		{
// 			hitposition = GetCollisionPoint();
// 			directional_force.Y = VitaVehicleSimulation.suspension(this,S_MaxCompression,A_InclineArea,A_ImpactForce,S_RestLength, elasticity,damping,damping_rebound, velocity.Y,Mathf.Abs(cast_to.Y),global_translation,GetCollisionPoint(),car.Mass,ground_bump,ground_bump_height);
	
// 			// FRICTION
// 			var grip = (directional_force.Y*tyre_maxgrip)*(ground_friction +fore_friction*(float)CompoundSettings["ForeFriction"]);
// 			float rigidity = 0.67f;
// 			float r = 1.0f-rigidity;
			
// 			float patch_hardness = 1.0f;
			
			
// 			var disty = velocity2.Z - (wv*w_size)/(drag +1.0f);
// 			if(Differed_Wheel != "")
// 			{
// 				var d_w = car.GetNode<Wheel>(Differed_Wheel);
// 				disty = velocity2.Z - ((wv*(1.0f-GetParent<Car>().locked) +d_w.wv_diff*GetParent<Car>().locked)*w_size)/(drag +1);
			
// 			}
// 			var distx = velocity2.X;
			
// 			var compensate2 = directional_force.Y;
// 	//		var grav_incline = $geometry.GlobalTransform.Basis.Orthonormalized().xform_inv(new Vector3(0,1,0)).x
// 			var grav_incline = (GetNode<MeshInstance3D>("geometry").GlobalTransform.Basis.Orthonormalized().Transposed() * (new Vector3(0,1,0))).X;
			
// 			distx -= (grav_incline*(compensate2/tyre_stiffness))*1.1f;
			
// 			slip_perc = new Vector2(distx,disty);
			
// 			disty *= tyre_stiffness;
// 			distx *= tyre_stiffness;
		
// 			distx -= Mathf.Atan2(Mathf.Abs(wv),1.0f)*((angle*10.0f)*w_size);
			
// 			if(grip>0)
// 			{
			
// 				var slipraw = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(disty),2.0) + Mathf.Pow(Mathf.Abs(distx),2.0));
// 				if(slipraw>grip)
// 				{
// 					slipraw = grip;
					
// 				}
// 				var slip = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(disty),2.0) + Mathf.Pow(Mathf.Abs(distx),2.0))/grip;
// 				slip /= slip*ground_builduprate +1.0f;
// 				slip -= (float)CompoundSettings["TractionFactor"];
// 				if(slip<0)
// 				{
// 					slip = 0;
// 				}
// 				slip_perc2 = (float)slip;
					
// 				var forcey = -disty/(slip +1.0f);
// 				var forcex = -distx/(slip +1.0f);
				
// 				var yesx = Mathf.Abs(forcex);
// 				if(yesx>1.0)
// 				{
// 					yesx = 1.0f;
// 				}
// 				var smoothx = yesx*yesx;
// 				if(smoothx>1.0)
// 				{
// 					smoothx = 1.0f;
// 				}
// 				var yesy = Mathf.Abs(forcey);
// 				if(yesy>1.0)
// 				{
// 					yesy = 1.0f;
// 				}
// 				var smoothy = yesy*1.0;
// 				if(smoothy>1.0)
// 				{
// 					smoothy = 1.0f;
// 				}
// 				forcex /= (smoothx*(rigidity) +(1.0f-rigidity));
// 				forcey /= (smoothy*(rigidity) +(1.0f-rigidity));
					
// 				directional_force.X = (float)forcex;
// 				directional_force.Z = (float)forcey;
// 			}
// 		}
// 		else
// 		{
// 			GetNode<MeshInstance3D>("geometry").Position = cast_to;
	
// 		}
// 		output_wv = wv;
// 		GetNode<Marker3D>("animation/camber/wheel").RotateX(Mathf.DegToRad(wv));
	
// 		GetNode<MeshInstance3D>("geometry").Position.Y += w_size;
	
	
	
	
	
	
// 		var inned = (Mathf.Abs(cambered)+A_Geometry4)/90.0f;
		
// 		inned *= inned -A_Geometry4/90.0f;
	
// 		GetNode<MeshInstance3D>("geometry").Position.X = -inned*translation.X;
	
	
	
	
// 		int xlt0 = translation.X < 0.0 ? 1 : 0;
// 		int xgt0 = translation.X < 0.0 ? 1 : 0;
// 		GetNode<Marker3D>("animation/camber").Rotation.Z = -(Mathf.DegToRad(-c_camber*(xlt0) + c_camber*(xgt0)) -Mathf.DegToRad(-cambered*(xlt0) + cambered*(xgt0))*A_Geometry2);
	
	
	
	
	
	
// 		var g;
		
// 		axle_position = GetNode<MeshInstance3D>("geometry").Position.Y;
	
	
// 		if(Solidify_Axles.ToString() == "")
// 		{
// 			g = (GetNode<MeshInstance3D>("geometry").Position.Y+(Mathf.Abs(cast_to.Y) -A_Geometry1))/(Mathf.Abs(translation.X)+A_Geometry3 +1.0f);
// 			g /= Mathf.Abs(g) +1.0f;
// 			cambered = (g*90.0f) -A_Geometry4;
// 		}
// 		else
// 		{
// 			g = (GetNode<MeshInstance3D>("geometry").Position.Y - GetNode<Wheel>(Solidify_Axles).axle_position)/(Mathf.Abs(translation.X) +1.0f);
// 			g /= Mathf.Abs(g) +1.0f;
// 			cambered = (g*90.0);
		
// 		}
// 		GetNode<Marker3D>("animation").Position = GetNode<MeshInstance3D>("geometry").Position;
			
// 	//	var forces = $velocity2.GlobalTransform.Basis.Orthonormalized().xform(new Vector3(0,0,1))*directional_force.z + $velocity2.GlobalTransform.Basis.Orthonormalized().xform(new Vector3(1,0,0))*directional_force.x + $velocity2.GlobalTransform.Basis.Orthonormalized().xform(new Vector3(0,1,0))*directional_force.y
// 		var forces = (GetNode<Marker3D>("velocity2").GlobalTransform.Basis.Orthonormalized() * (new Vector3(0,0,1)))*directional_force.Z + (GetNode<Marker3D>("velocity2").GlobalTransform.Basis.Orthonormalized() * (new Vector3(1,0,0)))*directional_force.X + (GetNode<Marker3D>("velocity2").GlobalTransform.Basis.Orthonormalized() * (new Vector3(0,1,0)))*directional_force.Y;
		
		
// 	//	car.apply_impulse(hitposition-car.GlobalTransform.origin,forces)
// 		car.ApplyImpulse(forces,hitposition-car.GlobalTransform.Origin);
		
// 		// torque
		
// 		var torqed = (wheelpower*w_weight)/4.0;
		
// 		wv_ds = wv;
		
// 	//	car.apply_impulse($geometry.GlobalTransform.origin-car.GlobalTransform.origin +$velocity2.GlobalTransform.Basis.Orthonormalized().xform(new Vector3(0,0,1)),$velocity2.GlobalTransform.Basis.Orthonormalized().xform(new Vector3(0,1,0))*torqed)
// 	//	car.apply_impulse($geometry.GlobalTransform.origin-car.GlobalTransform.origin -$velocity2.GlobalTransform.Basis.Orthonormalized().xform(new Vector3(0,0,1)),$velocity2.GlobalTransform.Basis.Orthonormalized().xform(new Vector3(0,1,0))*-torqed)
		
		
	
	
	
	
	
	
	
// 	}
	
	
	
// }