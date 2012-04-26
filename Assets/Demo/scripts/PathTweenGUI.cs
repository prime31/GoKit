using UnityEngine;
using System.Collections;


public class PathTweenGUI : BaseDemoGUI
{
	public Transform cube;
	
	
	void Start()
	{
		// example showing how to create a GoVector3Path from a saved file that was made with the visual editor
		var path = new GoVector3Path( "demoRoute" );
		
		// alternatively, paths can be created from an array of Vector3s
		//var vectors = new Vector3[] { new Vector3( 0, 1, 1 ), new Vector3( 4, 5, 6 ) };
		//var p = new GoVector3Path( vectors );
		
		// create the Tween and set the path to be relative (this will make the cube move BY the values stored in the
		// path instead of TO them). autoRemoveOnComplete is also set to false so that the Tween stays in the GoKit
		// engine and we can start/stop/reset it at will using the playback controls
		_tween = Go.to( cube, 4f, new TweenConfig().positionPath( path, true ) );
		_tween.autoRemoveOnComplete = false;
	}

}
