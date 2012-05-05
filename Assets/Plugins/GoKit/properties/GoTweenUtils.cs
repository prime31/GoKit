using UnityEngine;
using System;
using System.Collections;


public static class GoTweenUtils
{
	/// <summary>
	/// fetches the actual function for the given ease type
	/// </summary>
	public static Func<float,float,float,float,float> easeFunctionForType( EaseType easeType )
	{
		switch( easeType )
		{
			case EaseType.Linear:
				return Linear.EaseNone;
			
			case EaseType.BackIn:
				return Back.EaseIn;
			case EaseType.BackOut:
				return Back.EaseOut;
			case EaseType.BackInOut:
				return Back.EaseInOut;
			
			case EaseType.BounceIn:
				return Bounce.EaseIn;
			case EaseType.BounceOut:
				return Bounce.EaseOut;
			case EaseType.BounceInOut:
				return Bounce.EaseInOut;
			
			case EaseType.CircIn:
				return Circular.EaseIn;
			case EaseType.CircOut:
				return Circular.EaseOut;
			case EaseType.CircInOut:
				return Circular.EaseInOut;
			
			case EaseType.CubicIn:
				return Cubic.EaseIn;
			case EaseType.CubicOut:
				return Cubic.EaseOut;
			case EaseType.CubicInOut:
				return Cubic.EaseInOut;
			
			case EaseType.ElasticIn:
				return Elastic.EaseIn;
			case EaseType.ElasticOut:
				return Elastic.EaseOut;
			case EaseType.ElasticInOut:
				return Elastic.EaseInOut;
			
			case EaseType.ExpoIn:
				return Exponential.EaseIn;
			case EaseType.ExpoOut:
				return Exponential.EaseOut;
			case EaseType.ExpoInOut:
				return Exponential.EaseInOut;
			
			case EaseType.QuadIn:
				return Quadratic.EaseIn;
			case EaseType.QuadOut:
				return Quadratic.EaseOut;
			case EaseType.QuadInOut:
				return Quadratic.EaseInOut;
			
			case EaseType.QuartIn:
				return Quartic.EaseIn;
			case EaseType.QuartOut:
				return Quartic.EaseOut;
			case EaseType.QuartInOut:
				return Quartic.EaseInOut;
			
			case EaseType.QuintIn:
				return Quintic.EaseIn;
			case EaseType.QuintOut:
				return Quintic.EaseOut;
			case EaseType.QuintInOut:
				return Quintic.EaseInOut;
			
			case EaseType.SineIn:
				return Sinusoidal.EaseIn;
			case EaseType.SineOut:
				return Sinusoidal.EaseOut;
			case EaseType.SineInOut:
				return Sinusoidal.EaseInOut;
		}
		
		return Linear.EaseNone;
	}
	
	
	/// <summary>
	/// either returns a super fast Delegate to set the given property or null if it couldn't be found
	/// via reflection
	/// </summary>
	public static T setterForProperty<T>( System.Object targetObject, string propertyName )
	{
		// first get the property
		var propInfo = targetObject.GetType().GetProperty( propertyName );
		
		if( propInfo == null )
		{
			Debug.Log( "could not find property with name: " + propertyName );
			return default( T );
		}
		
		return (T)(object)Delegate.CreateDelegate( typeof( T ), targetObject, propInfo.GetSetMethod() );
	}
	
	
	/// <summary>
	/// either returns a super fast Delegate to get the given property or null if it couldn't be found
	/// via reflection
	/// </summary>
	public static T getterForProperty<T>( System.Object targetObject, string propertyName )
	{
		// first get the property
		var propInfo = targetObject.GetType().GetProperty( propertyName );
		
		if( propInfo == null )
		{
			Debug.Log( "could not find property with name: " + propertyName );
			return default( T );
		}
		
		return (T)(object)Delegate.CreateDelegate( typeof( T ), targetObject, propInfo.GetGetMethod() );
	}
	
	
	#region math functions
	
	/// <summary>
	/// note for all lerps: normally a lerp would be something like the following:
	/// val1 + ( val2 - val1 ) * t
	/// or in more familiar terms:
	/// start + ( end - start ) * t
	/// 
	/// when lerping relatively, the formula simplifies to:
	/// start + end * t
	/// 
	/// for all the unclamped lerps in this class the diff value is precalculated and cached. that means these arent like normal
	/// lerps where you pass in the start and end values. the "diff" paramter in each method should be either the cached
	/// ( end - start ) for non-relative tweens or just end for relative tweens (that are not "from" tweens)
	/// </summary>
	
	
	/// <summary>
	/// unclamped lerp from c1 to c2. diff should be c2 - c1 (or just c2 for relative lerps)
	/// </summary>
	public static Color unclampedColorLerp( Color c1, Color diff, float value )
	{
        return new Color
		(
			c1.r + diff.r * value,
			c1.g + diff.g * value,
			c1.b + diff.b * value,
			c1.a + diff.a * value
		);
    }
	
	
	/// <summary>
	/// unclamped lerp from v1 to v2. diff should be v2 - v1 (or just v2 for relative lerps)
	/// </summary>
    public static Vector2 unclampedVector2Lerp( Vector2 v1, Vector2 diff, float value )
	{
        return new Vector2
		(
			v1.x + diff.x * value,
            v1.y + diff.y * value
		);
    }

	
	/// <summary>
	/// unclamped lerp from v1 to v2. diff should be v2 - v1 (or just v2 for relative lerps)
	/// </summary>
    public static Vector3 unclampedVector3Lerp( Vector3 v1, Vector3 diff, float value )
	{
        return new Vector3
		(
			v1.x + diff.x * value,
            v1.y + diff.y * value,
            v1.z + diff.z * value
		);
		
		/*
        return new Vector3
		(
			v1.x + ( v2.x - v1.x ) * value, 
			v1.y + ( v2.y - v1.y ) * value, 
			v1.z + ( v2.z - v1.z ) * value
		);
		*/
    }

	
	/// <summary>
	/// unclamped lerp from v1 to v2. diff should be v2 - v1 (or just v2 for relative lerps)
	/// </summary>
    public static Vector4 unclampedVector4Lerp( Vector4 v1, Vector4 diff, float value )
	{
        return new Vector4
		(
			v1.x + diff.x * value,
            v1.y + diff.y * value,
            v1.z + diff.z * value,
			v1.w + diff.w * value
		);
    }

	#endregion
	
}
