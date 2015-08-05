/*

	Spline Build and Query Tests - GoKit AbstractGoSplineSolver master vs speedysplines

	Greg Harding greg@flightless.co.nz www.flightless.nz
	Retina MacBook Pro 2012, OS X 10.9.5, Unity 5.1.2f1

	Empty scene with single gameobject running this script.

	Building splines;
	master: Built 10000 splines with 0 lookups - avg 86.31397ms over 100 tests
	speedysplines: Built 10000 splines with 0 lookups - avg 72.74738ms over 100 tests
	~16% faster

	Querying spline;
	master: Built 1 splines with 10000 lookups - avg 46.2421ms over 100 tests
	speedysplines: Built 1 splines with 10000 lookups - avg 5.12037ms over 100 tests
	~89% faster

	Building and querying splines;
	master: Built 1000 splines with 1000 lookups - avg 4454.5128ms over 100 tests
	speedysplines: Built 1000 splines with 1000 lookups - avg 514.76155ms over 100 tests
	~88% faster
	
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class GoKitSplineTest : MonoBehaviour {

	// test settings
	public float testDelay = 5f;
	public float retestDelay = 1f;
	protected List<double> testTimes;

	public int splineCount = 100;
	public int splineLookupCount = 100;

	// spline
	protected Vector3 startPosition = Vector3.zero;
	protected Vector3 controlPosition1 = Vector3.one;
	protected Vector3 controlPosition2 = Vector3.right;
	protected Vector3 endPosition = Vector3.zero;

	protected Vector3[] splineNodes;


	protected void Start() {
		testTimes = new List<double>(100);

		splineNodes = new Vector3[] { startPosition, controlPosition1, controlPosition2, endPosition };

		Invoke("BuildSplines", testDelay);
	}

	protected void BuildSplines() {
		Stopwatch stopWatch = new Stopwatch();
		stopWatch.Start();

		BuildSplines(splineCount);

		stopWatch.Stop();

		TimeSpan ts = stopWatch.Elapsed;
		testTimes.Add(ts.TotalMilliseconds);

		UnityEngine.Debug.Log(string.Format("Built {0} splines with {1} lookups in {2}ms (avg {3}ms over {4} tests)", splineCount, splineLookupCount, ts.TotalMilliseconds, GetAverage(testTimes), testTimes.Count));

		Invoke("BuildSplines", retestDelay);
	}

	protected void BuildSplines(int num) {
		for (int s=0; s<num; s++) {
			GoSpline spline = new GoSpline(splineNodes);
			spline.buildPath();

			if (splineLookupCount > 0) {
				LookupSpline(spline);
			}
		}
	}

	protected void LookupSpline(GoSpline spline) {
		float tStep = 1f / splineLookupCount;

		float t = tStep;
		for (int i=0; i<splineLookupCount; i++) {
			spline.getPointOnPath(t);
			t += tStep;
		}
	}

	protected double GetAverage(List<double> values) {
		// no data?
		if (values.Count == 0) return 0;

		// sum
		double sum = 0;
		for (int t=0; t<values.Count; t++) {
			sum += values[t];
		}

		// avg
		return sum / values.Count;
	}
}
