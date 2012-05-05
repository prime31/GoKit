using UnityEngine;
using System.Collections;


public enum SplineType
{
	StraightLine, // 2 points
	QuadraticBezier, // 3 points
	CubicBezier, // 4 points
	CatmullRom // 5+ points
}
