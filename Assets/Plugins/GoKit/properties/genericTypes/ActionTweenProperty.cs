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


	public ActionTweenProperty( float startValue, float endValue, System.Action<float> action )
	{
		_startValue = startValue;
		_endValue = endValue;
		_action = action;
	}


	public override void prepareForUse()
	{}


	public override void tick( float totalElapsedTime )
	{
		var easedTime = _easeFunction( totalElapsedTime, 0, 1, _ownerTween.duration );
		var easedValue = Mathf.Lerp( _startValue, _endValue, easedTime );
		_action( easedValue );
	}
}