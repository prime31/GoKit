using UnityEngine;
using System.Collections;


public class MaterialFloatTweenProperty : AbstractMaterialFloatTweenProperty
{
	private string _materialPropertyName;
	
	
	public MaterialFloatTweenProperty( float endValue, string propertyName, bool isRelative = false ) : base( endValue, isRelative )
	{
		_materialPropertyName = propertyName;
	}
	
	
	#region Object overrides
	
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
	
	
	public override bool Equals( object obj )
	{
		// start with a base check and then compare our material names
		if( base.Equals( obj ) )
			return this._materialPropertyName == ((MaterialFloatTweenProperty)obj)._materialPropertyName;
		
		return false;
	}
	
	#endregion
	

	public override void prepareForUse()
	{
		_endValue = _originalEndValue;
		
		// if this is a from tween we need to swap the start and end values
		if( _ownerTween.isFrom )
		{
			_startValue = _endValue;
			_endValue = _target.GetFloat( _materialPropertyName );
		}
		else
		{
			_startValue = _target.GetFloat( _materialPropertyName );
		}
		
		base.prepareForUse();
	}
	
	
	public override void tick( float totalElapsedTime )
	{
		var easedTime = _easeFunction( totalElapsedTime, 0, 1, _ownerTween.duration );
		var value = Mathf.Lerp( _startValue, _diffValue, easedTime );
		
		_target.SetFloat( _materialPropertyName, value );
	}

}
