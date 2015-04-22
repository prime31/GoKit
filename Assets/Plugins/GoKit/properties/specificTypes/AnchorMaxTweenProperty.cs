using UnityEngine;
using System.Collections;


public class AnchorMaxTweenProperty : AbstractTweenProperty
{
	protected RectTransform _target;
	
	protected Vector2 _originalEndValue;
	protected Vector2 _startValue;
	protected Vector2 _endValue;
	protected Vector2 _diffValue;
	
	public AnchorMaxTweenProperty( Vector2 endValue, bool isRelative = false) : base( isRelative )
	{
		_originalEndValue = endValue;
	}
	
	public override bool validateTarget( object target )
	{
		return target is RectTransform;
	}
	
	public override void prepareForUse()
	{
		_target = _ownerTween.target as RectTransform;
		
		_endValue = _originalEndValue;
		
		if( _ownerTween.isFrom ) 
		{
			_startValue = _isRelative ? _endValue + _target.anchorMax : _endValue;
			_endValue = _target.anchorMax; 
		} 
		else
		{
			_startValue = _target.anchorMax;
		}
		
		if( _isRelative && !_ownerTween.isFrom )
			_diffValue = _endValue;
		else
			_diffValue = _endValue - _startValue;
	}
	
	
	public override void tick( float totalElapsedTime )
	{
		var easedTime = _easeFunction( totalElapsedTime, 0, 1, _ownerTween.duration );
		var vec = GoTweenUtils.unclampedVector2Lerp( _startValue, _diffValue, easedTime );
		 
		_target.anchorMax = vec;
	}
	
	
	public void resetWithNewEndValue( Vector2 endValue )
	{
		_originalEndValue = endValue;
		prepareForUse();
	}
}
