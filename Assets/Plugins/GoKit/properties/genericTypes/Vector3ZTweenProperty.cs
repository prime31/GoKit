using UnityEngine;
using System;
using System.Collections;


public class Vector3ZTweenProperty : Vector3XTweenProperty
{
	public Vector3ZTweenProperty( string propertyName, float endValue, bool isRelative = false ) : base( propertyName, endValue, isRelative )
	{}

	
	public override void prepareForUse()
	{
		// retrieve the getter
		_getter = GoTweenUtils.getterForProperty<Func<Vector3>>( _ownerTween.target, propertyName );
		
		_endValue = _originalEndValue;
		
		// if this is a from tween we need to swap the start and end values
		if( _ownerTween.isFrom )
		{
			_startValue = _endValue;
			_startValue = _getter().z;
		}
		else
		{
			_startValue = _getter().z;
		}
		
		base.prepareForUse();
	}
	
	
	public override void tick( float totalElapsedTime )
	{
		var currentValue = _getter();
		currentValue.z = _easeFunction( totalElapsedTime, _startValue, _endValue, _ownerTween.duration );
		
		_setter( currentValue );
	}

}
