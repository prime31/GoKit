using UnityEngine;
using System.Collections;


public static class GoKitTweenExtensions
{
	#region Transform extensions
	
	// to tweens
	public static Tween eularAnglesTo( this Transform self, float duration, Vector3 endValue, bool isRelative = false )
	{
		return Go.to( self, duration, new TweenConfig().eulerAngles( endValue, isRelative ) );
	}
	
	
	public static Tween localEularAnglesTo( this Transform self, float duration, Vector3 endValue, bool isRelative = false )
	{
		return Go.to( self, duration, new TweenConfig().localEulerAngles( endValue, isRelative ) );
	}
	
	
	public static Tween positionTo( this Transform self, float duration, Vector3 endValue, bool isRelative = false )
	{
		return Go.to( self, duration, new TweenConfig().position( endValue, isRelative ) );
	}
	
	
	public static Tween localPositionTo( this Transform self, float duration, Vector3 endValue, bool isRelative = false )
	{
		return Go.to( self, duration, new TweenConfig().localPosition( endValue, isRelative ) );
	}
	
	
	public static Tween scaleTo( this Transform self, float duration, float endValue, bool isRelative = false )
	{
		return self.scaleTo( duration, new Vector3( endValue, endValue, endValue ), isRelative );
	}
	
	
	public static Tween scaleTo( this Transform self, float duration, Vector3 endValue, bool isRelative = false )
	{
		return Go.to( self, duration, new TweenConfig().scale( endValue, isRelative ) );
	}
	
	
	public static Tween shake( this Transform self, float duration, Vector3 shakeMagnitude, ShakeType shakeType = ShakeType.Position, int frameMod = 1, bool useLocalProperties = false )
	{
		return Go.to( self, duration, new TweenConfig().shake( shakeMagnitude, shakeType, frameMod, useLocalProperties ) );
	}
	
	
	// from tweens
	public static Tween eularAnglesFrom( this Transform self, float duration, Vector3 endValue, bool isRelative = false )
	{
		return Go.from( self, duration, new TweenConfig().eulerAngles( endValue, isRelative ) );
	}
	
	
	public static Tween localEularAnglesFrom( this Transform self, float duration, Vector3 endValue, bool isRelative = false )
	{
		return Go.from( self, duration, new TweenConfig().localEulerAngles( endValue, isRelative ) );
	}
	
	
	public static Tween positionFrom( this Transform self, float duration, Vector3 endValue, bool isRelative = false )
	{
		return Go.from( self, duration, new TweenConfig().position( endValue, isRelative ) );
	}
	
	
	public static Tween localPositionFrom( this Transform self, float duration, Vector3 endValue, bool isRelative = false )
	{
		return Go.from( self, duration, new TweenConfig().localPosition( endValue, isRelative ) );
	}
	
	
	public static Tween scaleFrom( this Transform self, float duration, Vector3 endValue, bool isRelative = false )
	{
		return Go.from( self, duration, new TweenConfig().scale( endValue, isRelative ) );
	}
	
	#endregion
	
	
	#region Material extensions
	
	public static Tween colorTo( this Material self, float duration, Color endValue, MaterialColorType colorType = MaterialColorType.Color )
	{
		return Go.to( self, duration, new TweenConfig().materialColor( endValue, colorType ) );
	}
	
	
	public static Tween colorFrom( this Material self, float duration, Color endValue, MaterialColorType colorType = MaterialColorType.Color )
	{
		return Go.from( self, duration, new TweenConfig().materialColor( endValue, colorType ) );
	}
	
	#endregion

}
