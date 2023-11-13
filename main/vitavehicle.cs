
// using System;
// using Godot;
// using Dictionary = Godot.Collections.Dictionary;
// using Array = Godot.Collections.Array;

// [Tool]
// public partial class vitavehicle : Node
// {
	 
// 	public bool misc_smoke = true;
	
// 	public int GearAssistant = 2 ;// 0 = manual, 1 = semi-manual, 2 = auto
	
// 	public bool UseMouseSteering = false;
// 	public bool UseAccelerometreSteering = false;
// 	public float SteerSensitivity = 1.0f;
// 	public float KeyboardSteerSpeed = 0.025f;
// 	public float KeyboardReturnSpeed = 0.05f;
// 	public float KeyboardCompensateSpeed = 0.1f;
	
// 	public __TYPE SteerAmountDecay = 0.0125 ;// understeer help
// 	public __TYPE SteeringAssistance = 1.0;
// 	public __TYPE SteeringAssistanceAngular = 0.25;
	
// 	public __TYPE OnThrottleRate = 0.2;
// 	public __TYPE OffThrottleRate = 0.2;
	
// 	public __TYPE OnBrakeRate = 0.05;
// 	public __TYPE OffBrakeRate = 0.1;
	
// 	public __TYPE OnHandbrakeRate = 0.2;
// 	public __TYPE OffHandbrakeRate = 0.2;
	
// 	public __TYPE OnClutchRate = 0.2;
// 	public __TYPE OffClutchRate = 0.2;
	
	
	
// 	public __TYPE multivariate(__TYPE RiseRPM,__TYPE TorqueRise,__TYPE BuildUpTorque,__TYPE EngineFriction,__TYPE EngineDrag,__TYPE OffsetTorque,__TYPE RPM,__TYPE DeclineRPM,__TYPE DeclineRate,__TYPE FloatRate,__TYPE PSI,__TYPE TurboAmount,__TYPE EngineCompressionRatio,__TYPE TEnabled,__TYPE VVTRPM,__TYPE VVT_BuildUpTorque,__TYPE VVT_TorqueRise,__TYPE VVT_RiseRPM,__TYPE VVT_OffsetTorque,__TYPE VVT_FloatRate,__TYPE VVT_DeclineRPM,__TYPE VVT_DeclineRate,__TYPE SCEnabled,__TYPE SCRPMInfluence,__TYPE BlowRate,__TYPE SCThreshold,__TYPE DeclineSharpness,__TYPE VVT_DeclineSharpness)
// 	{  
// 		float value = 0.0f;
		
// 		float maxpsi = 0.0f;
// 		float scrpm = 0.0f;
// 		float f = 0.0f;
// 		float j = 0.0f;
		
// 		if(SCEnabled)
// 		{
// 			maxpsi = PSI;
// 			scrpm = RPM*SCRPMInfluence;
// 			PSI = (scrpm/10000.0)*BlowRate -SCThreshold;
// 			if(PSI>maxpsi)
// 			{
// 				PSI = maxpsi;
// 			}
// 			if(PSI<0.0)
// 			{
// 				PSI = 0.0;
		
// 			}
// 		}
// 		if(!SCEnabled && !TEnabled)
// 		{
// 			PSI = 0.0;
	
// 		}
// 		if(RPM>VVTRPM)
// 		{
// 			value = (RPM*VVT_BuildUpTorque +VVT_OffsetTorque) + ( (PSI*TurboAmount) * (EngineCompressionRatio*0.609) );
// 			f = RPM-VVT_RiseRPM;
// 			if(f<0.0)
// 			{
// 				f = 0.0;
// 			}
// 			value += (f*f)*(VVT_TorqueRise/10000000.0);
// 			j = RPM-VVT_DeclineRPM;
// 			if(j<0.0)
// 			{
// 				j = 0.0;
// 			}
// 			value /= (j*(j*VVT_DeclineSharpness +(1.0-VVT_DeclineSharpness)))*(VVT_DeclineRate/10000000.0) +1.0
// 			value /= (RPM*RPM)*(VVT_FloatRate/10000000.0) +1.0
// 		}
// 		else
// 		{
// 			value = (RPM*BuildUpTorque +OffsetTorque) + ( (PSI*TurboAmount) * (EngineCompressionRatio*0.609) );
// 			f = RPM-RiseRPM;
// 			if(f<0.0)
// 			{
// 				f = 0.0;
// 			}
// 			value += (f*f)*(TorqueRise/10000000.0);
// 			j = RPM-DeclineRPM;
// 			if(j<0.0)
// 			{
// 				j = 0.0;
// 			}
// 			value /= (j*(j*DeclineSharpness +(1.0-DeclineSharpness)))*(DeclineRate/10000000.0) +1.0
// 			value /= (RPM*RPM)*(FloatRate/10000000.0) +1.0
	
// 		}
// 		value -= RPM/((Mathf.Abs(RPM*RPM))/EngineFriction +1.0);
// 		value -= RPM*EngineDrag;
		
		
		
// 		return value;
	
	
// 	}
	
// 	public __TYPE fastest_wheel(__TYPE array)
// 	{  
// 		float val = -10000000000000000000000000000000000.0f;
// 		var obj;
		
// 		foreach(var i in array)
// 		{
// 			val = Mathf.Max(val, Mathf.Abs(i.absolute_wv));
			
// 			if(val == Mathf.Abs(i.absolute_wv))
// 			{
// 				obj = i;
	
// 			}
// 		}
// 		return obj;
	
// 	}
	
// 	public __TYPE slowest_wheel(__TYPE array)
// 	{  
// 		float val = 10000000000000000000000000000000000.0f;
// 		var obj;
		
// 		foreach(var i in array)
// 		{
// 			val = Mathf.Min(val, Mathf.Abs(i.absolute_wv));
			
// 			if(val == Mathf.Abs(i.absolute_wv))
// 			{
// 				obj = i;
	
// 			}
// 		}
// 		return obj;
	
// 	}
	
// 	public __TYPE alignAxisToVector(__TYPE xform, __TYPE norm)
// 	{   // i named this literally out of blender
// 		xform.basis.y = norm;
// 		xform.basis.x = -xform.basis.z.cross(norm)
// 		xform.basis = xform.basis.orthonormalized();
// 		return xform;
	
	
// 	}
	
// 	public __TYPE suspension(__TYPE own,__TYPE maxcompression,__TYPE incline_free,__TYPE incline_impact,__TYPE rest,      __TYPE elasticity,__TYPE damping,__TYPE damping_rebound     ,__TYPE linearz,__TYPE g_range,__TYPE located,__TYPE hit_located,__TYPE weight,__TYPE ground_bump,__TYPE ground_bump_height)
// 	{  
// 		own.get_node("geometry").global_position = own.get_collision_point();
// 		own.get_node("geometry").position.y -= (ground_bump*ground_bump_height);
// 		if own.get_node("geometry").position.y<-g_range:
// 			own.get_node("geometry").position.y = -g_range
// 		own.get_node("velocity").global_transform = alignAxisToVector(own.get_node("velocity").global_transform,own.get_collision_normal());
// 		own.get_node("velocity2").global_transform = alignAxisToVector(own.get_node("velocity2").global_transform,own.get_collision_normal());
	
// 		own.angle = (own.get_node("geometry").rotation_degrees.z -(-own.c_camber*float(own.position.x>0.0) + own.c_camber*float(own.position.x<0.0)) +(-own.cambered*float(own.position.x>0.0) + own.cambered*float(own.position.x<0.0))*own.A_Geometry2)/90.0;
	
// 	//	var incline = (own.get_collision_normal()-own.global_transform.basis.orthonormalized().xform(new Vector3(0,1,0))).length();
// 		var incline = (own.get_collision_normal()-(own.global_transform.basis.orthonormalized() * new Vector3(0,1,0))).length();
			
// 		incline /= 1-incline_free
		
// 		incline -= incline_free;
	
// 		if(incline<0.0)
// 		{
// 			incline = 0.0;
	
// 		}
// 		incline *= incline_impact
	
// 		if(incline>1.0)
// 		{
// 			incline = 1.0;
	
// 		}
// 		if own.get_node("geometry").position.y>-g_range +maxcompression*(1.0-incline):
// 			own.get_node("geometry").position.y = -g_range +maxcompression*(1.0-incline)
	
// 		var damp_variant = damping_rebound;
// 		if(linearz<0)
// 		{
// 			damp_variant = damping;
	
// 		}
// 		var compressed = g_range -(located - hit_located).length() - (ground_bump*ground_bump_height);
// 		var compressed2 = g_range -(located - hit_located).length() - (ground_bump*ground_bump_height);
// 		compressed2 -= maxcompression + (ground_bump*ground_bump_height);
	
// 		var j = compressed-rest;
		
// 		if(j<0.0)
// 		{
// 			j = 0.0;
	
// 		}
// 		if(compressed2<0.0)
// 		{
// 			compressed2 = 0.0;
	
// 		}
// 		var elasticity2 = elasticity*(1.0-incline) + (weight)*incline;
// 		var damping2 = damp_variant*(1.0-incline) + (weight/10.0)*incline;
// 		var elasticity3 = weight;
// 		var damping3 = weight/10.0;
// 		var suspforce = j*elasticity2;
	
// 		if(compressed2>0.0)
// 		{
// 			suspforce -= linearz*damping3;
// 			suspforce += compressed2*elasticity3;
	
// 		}
// 		suspforce -= linearz*damping2;
	
// 		own.rd = compressed;
	
// 		if(suspforce<0.0)
// 		{
// 			suspforce = 0.0;
	
// 		}
// 		return suspforce;
	
	
	
// 	}
	
	
	
// }