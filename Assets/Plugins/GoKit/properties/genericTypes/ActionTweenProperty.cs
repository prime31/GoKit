using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// simple class that takes in an Action and calls it each tick with the eased value from startValue to endValue
/// </summary>
public class ActionTweenProperty : AbstractTweenProperty
{
	System.Action<float> _action;
	
	float _startValue;
	float _endValue;


	public ActionTweenProperty( System.Action<float> action, float startValue = 0, float endValue = 1 )
	{
		_action = action;
		_startValue = startValue;
		_endValue = endValue;
	}


	public override void prepareForUse()
	{}


	public override void tick( float totalElapsedTime )
	{
		var easedValue = _easeFunction( totalElapsedTime, _startValue, _endValue, _ownerTween.duration );
		_action( easedValue );
	}
}