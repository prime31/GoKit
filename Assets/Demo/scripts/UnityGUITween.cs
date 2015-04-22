using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnityGUITween : BaseDemoGUI
{
	public RectTransform anchorImage;
	public RectTransform sizeOffsetImage;
	
	void Start()
	{
		// this will run indefinitely.
		Go.to( anchorImage, 4, new GoTweenConfig()
		      .anchorMin(Vector2.one * 0.25f)
		      .anchorMax(Vector2.one * 0.75f)
		      .setIterations( -1, GoLoopType.PingPong ) );

		// hook this one up so we can play with the easeTypes in the UI.
		_tween = Go.to( sizeOffsetImage, 2, new GoTweenConfig()
		               .offsetMax(Vector2.right * 100)
		               .sizeDelta(Vector2.one * -50f, true)
		               .setIterations( -1, GoLoopType.PingPong ) );
	}
}
