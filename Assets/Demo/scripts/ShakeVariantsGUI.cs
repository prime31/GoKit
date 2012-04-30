using UnityEngine;
using System.Collections;


public class ShakeVariantsGUI : BaseDemoGUI
{
	public Transform cube;
	
	
	protected override void OnGUI()
	{
		// make some buttons to showcase different shake types
		if( GUILayout.Button( "Shake Position" ) )
		{
			stopRunningTween();
			_tween = Go.to( cube, 0.5f, new TweenConfig().shake( new Vector3( 1, 1, 1 ), ShakeType.Position ) );
		}
		
		
		if( GUILayout.Button( "Shake Scale" ) )
		{
			stopRunningTween();
			_tween = Go.to( cube, 0.5f, new TweenConfig().shake( new Vector3( 2, 2, 2 ), ShakeType.Scale ) );
		}
		
		
		if( GUILayout.Button( "Shake Eulers" ) )
		{
			stopRunningTween();
			_tween = Go.to( cube, 0.5f, new TweenConfig().shake( new Vector3( 150, 150, 150 ), ShakeType.Eulers ) );
		}
		
		
		if( GUILayout.Button( "Shake Position & Scale" ) )
		{
			stopRunningTween();
			_tween = Go.to( cube, 0.5f, new TweenConfig().shake( new Vector3( 1, 1, 1 ), ShakeType.Position | ShakeType.Scale ) );
		}
		
		
		// we add the eulers separately here so that we can get enough magnitude with the shake
		if( GUILayout.Button( "Shake Position & Eulers" ) )
		{
			stopRunningTween();
			_tween = Go.to( cube, 0.5f, new TweenConfig().shake( new Vector3( 1, 1, 1 ), ShakeType.Position ).shake( new Vector3( 150, 150, 150 ), ShakeType.Eulers ) );
		}
		
		
		if( GUILayout.Button( "Shake Position, Scale & Eulers" ) )
		{
			stopRunningTween();
			_tween = Go.to( cube, 0.5f, new TweenConfig().shake( new Vector3( 1, 1, 1 ), ShakeType.Position | ShakeType.Scale ).shake( new Vector3( 150, 150, 150 ), ShakeType.Eulers ) );
		}
		
		
		// the frameMod parameter basically acts as a way to slow down the shake by skipping frames in the animation
		// to make it a bit less jumpy
		if( GUILayout.Button( "Shake Position with Frame Mod" ) )
		{
			stopRunningTween();
			_tween = Go.to( cube, 0.5f, new TweenConfig().shake( new Vector3( 1, 1, 1 ), ShakeType.Position, 2 ) );
		}
	}
	
	
	private void stopRunningTween()
	{
		// shake tweens should always be completed before stopping to ensure the item ends up in
		// the same location it started
		if( _tween != null )
		{
			_tween.complete();
			_tween.destroy();
			_tween = null;
		}
	}

}
