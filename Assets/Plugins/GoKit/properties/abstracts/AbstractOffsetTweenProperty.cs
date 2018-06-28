using UnityEngine;
using System.Collections;


public abstract class AbstractOffsetTweenProperty : AbstractTweenProperty
{
	private bool _useMax;
	
	private RectTransform _target;
	
	private Vector2 _originalEndValue;
	private Vector2 _startValue;
	private Vector2 _endValue;
	private Vector2 _diffValue;

	public AbstractOffsetTweenProperty( Vector2 endValue, bool isRelative = false, bool useMax = false ) : base( isRelative )
	{
		_originalEndValue = endValue;
		_useMax = useMax;
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
			if( _useMax )
			{  
				_startValue = _isRelative ? _endValue + _target.offsetMax : _endValue;
				_endValue = _target.offsetMax; 
			} 
			else
			{
				_startValue = _isRelative ? _endValue + _target.offsetMin : _endValue;
				_endValue = _target.offsetMin;
			}
		} 
		else
		{
			_startValue = _useMax ? _target.offsetMax : _target.offsetMin;
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
		
		if( _useMax )
			_target.offsetMax = vec;
		else
			_target.offsetMin = vec;
	}
	
	
	public void resetWithNewEndValue( Vector2 endValue )
	{
		_originalEndValue = endValue;
		prepareForUse();
	}
	
}
