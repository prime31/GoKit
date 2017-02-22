using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractGoSplineSolver
{
	protected List<Vector3> _nodes;
	public List<Vector3> nodes { get { return _nodes; } }
	protected float _pathLength;
	
	public float pathLength
	{
		get
		{
			return _pathLength;
		}
	}
	
	
	// how many subdivisions should we divide each segment into? higher values take longer to build and lookup but
	// result in closer to actual constant velocity
	protected int totalSubdivisionsPerNodeForLookupTable = 5;

	// time:distance lookup table
	protected struct Segment
	{
		public float time;
		public float distance;

		public Segment ( float time, float distance )
		{
			this.time = time;
			this.distance = distance;
		}
	}

	protected List<Segment> segments;


	// the default implementation breaks the spline down into segments and approximates distance by adding up
	// the length of each segment
	public virtual void buildPath()
	{
		// build or clear segments cache
		var totalSubdivisions = _nodes.Count * totalSubdivisionsPerNodeForLookupTable;
		if( segments == null )
		{
			segments = new List<Segment>(totalSubdivisions);
		}
		else
		{
			segments.Clear();
			segments.Capacity = totalSubdivisions;
		}

		_pathLength = 0;
		float timePerSlice = 1f / totalSubdivisions;
		var lastPoint = getPoint( 0 );
		
		// skip the first node and wrap 1 extra node
		// we dont care about the first node for distances because they are always t:0 and len:0
		for( var i = 1; i < totalSubdivisions + 1; i++ )
		{
			// what is the current time along the path?
			float currentTime = timePerSlice * i;

			var currentPoint = getPoint( currentTime );
			_pathLength += Vector3.Distance( currentPoint, lastPoint );
			lastPoint = currentPoint;

			// cache segment
			segments.Add(new Segment(currentTime, _pathLength));
		}
	}
	
	
	public abstract void closePath();
	
	
	// gets the raw point not taking into account constant speed. used for drawing gizmos
	public abstract Vector3 getPoint( float t );
	
	
	// gets the point taking in to account constant speed. the default implementation approximates the length of the spline
	// by walking it and calculating the distance between each node
	public virtual Vector3 getPointOnPath( float t )
	{
		// we know exactly how far along the path we want to be from the passed in t
		float targetDistance = _pathLength * t;

		// loop through all the values in our lookup table and find the two nodes our targetDistance falls between
		// translate the values from the lookup table estimating the arc length between our known nodes from the lookup table
		int nextSegmentIndex;
		for( nextSegmentIndex = 0; nextSegmentIndex < segments.Count; nextSegmentIndex++ )
		{
			if( segments[nextSegmentIndex].distance >= targetDistance )
				break;
		}

		Segment nextSegment = segments[nextSegmentIndex];

		if( nextSegmentIndex == 0 ) {
			// t within first segment
			t = ( targetDistance / nextSegment.distance ) * nextSegment.time;
		}
		else
		{
			// t within prev..next segment
			Segment previousSegment = segments[nextSegmentIndex-1];

			float segmentTime = nextSegment.time - previousSegment.time;
			float segmentLength = nextSegment.distance - previousSegment.distance;

			t = previousSegment.time + ( ( targetDistance - previousSegment.distance ) / segmentLength ) * segmentTime;
		}

		return getPoint( t );
	}
	
	
	public void reverseNodes()
	{
		_nodes.Reverse();
	}
	
	
	public virtual void drawGizmos()
	{}

}
