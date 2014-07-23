using UnityEngine;
using System;


public static class GoEaseAnimationCurve
{
	public static Func<float, float, float, float, float> EaseCurve( GoTween tween )
	{
		if (tween == null)
		{
			Debug.LogError("no tween to extract easeCurve from");
		}

		if (tween.easeCurve == null)
		{
			Debug.LogError("no curve found for tween");
		}

		return delegate (float t, float b, float c, float d)
		{
			return tween.easeCurve.Evaluate(t / d) * c + b;
		};
	}
}

