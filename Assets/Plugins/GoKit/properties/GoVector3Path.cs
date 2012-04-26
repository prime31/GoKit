using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;


public class GoVector3Path
{
	public float routeLength { get; private set; }
	public int currentSegment { get; private set; }
	public bool isClosed { get; private set; }
	
	private Dictionary<int, float> _segmentStartLocations;
	private Dictionary<int, float> _segmentDistances;
	private List<Vector3> _nodes;
	private bool _isReversed; // internal flag that lets us know if our nodes are reversed or not
	
	
	public GoVector3Path( List<Vector3> nodes )
	{
		_nodes = nodes;
	}
	
	
	public GoVector3Path( Vector3[] nodes )
	{
		_nodes = new List<Vector3>( nodes );
	}
	
	
	public GoVector3Path( string pathAssetName )
	{
		if( !pathAssetName.EndsWith( ".asset" ) )
			pathAssetName += ".asset";
		
#if UNITY_EDITOR
		// in the editor we default to looking in the StreamingAssets folder
		var path = Path.Combine( Path.Combine( Application.dataPath, "StreamingAssets" ), pathAssetName );
#else
		// at runtime, we load from the dataPath
		var path = Path.Combine(  Path.Combine( Application.dataPath, "Raw" ), pathAssetName );
#endif
		var bytes = File.ReadAllBytes( path );
		_nodes = bytesToVector3List( bytes );
	}
	
	
	public static List<Vector3> bytesToVector3List( byte[] bytes )
	{
		var vecs = new List<Vector3>();
		for( var i = 0; i < bytes.Length; i += 12 )
		{
			var newVec = new Vector3( System.BitConverter.ToSingle( bytes, i ), System.BitConverter.ToSingle( bytes, i + 4 ), System.BitConverter.ToSingle( bytes, i + 8 ) );
			vecs.Add( newVec );
		}
		
		return vecs;
	}
	
	
	/// <summary>
	/// responsible for calculating total length, segmentStartLocations and segmentDistances
	/// </summary>
	public void buildPath()
	{
		// we dont care about the first node for distances because they are always t:0 and len:0 and we dont need the first or last for locations
		_segmentStartLocations = new Dictionary<int, float>( _nodes.Count - 2 );
		_segmentDistances = new Dictionary<int, float>( _nodes.Count - 1 );

		for( var i = 0; i < _nodes.Count - 1; i++ )
		{
			// calculate the distance to the next node
			var distance = Vector3.Distance( _nodes[i], _nodes[i + 1] );
			_segmentDistances.Add( i, distance );
			routeLength += distance;
		}
		

		// now that we have the total length we can loop back through and calculate the segmentStartLocations
		var accruedRouteLength = 0f;
		for( var i = 0; i < _segmentDistances.Count - 1; i++ )
		{
			accruedRouteLength += _segmentDistances[i];
			_segmentStartLocations.Add( i + 1, accruedRouteLength / routeLength );
		}
	}
	

	/// <summary>
	/// helper function to get the nearest point on a line to another point
	/// </summary>
	private Vector3 nearestPointOnLineToPoint( Vector3 lineStart, Vector3 lineEnd, Vector3 point, bool clampToSegment )
	{
		// first grab the distance along the line that the intersection of a perpindicular line occurs
		var lineAsVector = lineEnd - lineStart;
		var t = Vector3.Dot( lineAsVector, point - lineStart ) / Vector3.Dot( lineAsVector, lineAsVector );

		// if we are clamping to the segment we need to return the endpoint if t > 1 or t < 0
		if( clampToSegment )
		{
			if( t > 1 )
				return lineEnd;
			else if( t < 0 )
				return lineStart;
		}
		
		// figure out the actual point on the line
		return lineStart + t * lineAsVector;
	}

	
	/// <summary>
	/// returns the point that corresponds to the given t where t >= 0 and t <= 1
	/// </summary>
	public Vector3 getPointOnRoute( float t )
	{
		// if the path is closed, we will allow t to wrap. if is not we need to clamp t
		if( t < 0 || t > 1 )
		{
			if( isClosed )
			{
				if( t < 0 )
					t += 1;
				else
					t -= 1;
			}
			else
			{
				t = Mathf.Clamp( t, 0, 1 );
			}
		}

		// TODO: optimize this to start the search from the currentSegment stored from the last outing
		
		// which segment are we on?
		currentSegment = 0;
		foreach( var info in _segmentStartLocations )
		{
			if( info.Value < t )
			{
				currentSegment = info.Key;
				continue;
			}
			
			break;
		}
		
		// now we need to know the total distance travelled in all previous segments so we can subtract it from the total
		// travelled to get exactly how far along the current segment we are
		var totalDistanceTravelled = t * routeLength;
		var i = currentSegment - 1; // we want all the previous segment lengths
		while( i >= 0 )
		{
			totalDistanceTravelled -= _segmentDistances[i];
			--i;
		}
		
		return Vector3.Lerp( _nodes[currentSegment], _nodes[currentSegment + 1], totalDistanceTravelled / _segmentDistances[currentSegment] );
	}
	
	
	/// <summary>
	/// returns t where t is equal to the value between 0-1 on the path for the given point
	/// </summary>
	public float getRoutePositionForPoint( Vector3 point )
	{
		// TODO: optimize this to start the search from the currentSegment
		
		// first off, get the point on the proper y value
		point.y = _nodes[0].y;
		
		// now we need to find the nearest segment
		var distance = float.MaxValue;
		var segment = -1;
		Vector3 nearestPoint = Vector3.zero;
		for( var i = 0; i < _nodes.Count - 1; i++ )
		{
			var currentPointOnSegment = nearestPointOnLineToPoint( _nodes[i], _nodes[i + 1], point, true );
			var calulatedDistance = Vector3.Distance( point, currentPointOnSegment );
			
			// if we found a new closest go ahead and keep it
			if( calulatedDistance < distance )
			{
				nearestPoint = currentPointOnSegment;
				distance = calulatedDistance;
				segment = i;
			}
		}
		
		// get the total distance for all the previous segments
		var accruedDistance = 0f;
		for( var i = 0; i < segment; i++ )
			accruedDistance += _segmentDistances[i];
		

		// now we add the distance from the point we found to the first point in the segment we found
		// this will give us the total distance around the path we travelled
		accruedDistance += Vector3.Distance( nearestPoint, _nodes[segment] );
		
		// divide by the routeLength and we have our t value
		return accruedDistance / routeLength;
	}
	

	/// <summary>
	/// gets the next node in the path. useful for orienting via LookAt
	/// </summary>
	public Vector3 getNextNode()
	{
		// the current segment will always have a next node due to t always being before the last node
		return _nodes[currentSegment + 1];
	}
	
	
	/// <summary>
	/// gets the previous node in the path. useful for orienting via LookAt
	/// </summary>
	public Vector3 getPreviousNode()
	{
		// we just look at the current node which is the last we passed
		return _nodes[currentSegment];
	}
	
	
	/// <summary>
	/// gets the last node in the path
	/// </summary>
	public Vector3 getLastNode()
	{
		return _nodes[_nodes.Count - 1];
	}
	
	
	public float getNormalizedSpeed( float desiredSpeed )
	{
		return 1 / ( routeLength / desiredSpeed );
	}
	
	
	/// <summary>
	/// closes the path adding a new node at the end that is equal to the start node if it isn't already equal
	/// </summary>
	public void closePath()
	{
		// dont let this get closed twice!
		if( isClosed )
			return;
		
		isClosed = true;
		
		// add a node to close the route if necessary
		if( _nodes[0] != _nodes[_nodes.Count - 1] )
			_nodes.Add( _nodes[0] );
	}
	
	
	/// <summary>
	/// reverses the order of the nodes
	/// </summary>
	public void reverseNodes()
	{
		if( !_isReversed )
		{
			_nodes.Reverse();
			_isReversed = true;
		}
	}
	
	
	/// <summary>
	/// unreverses the order of the nodes if they were reversed
	/// </summary>
	public void unreverseNodes()
	{
		if( _isReversed )
		{
			_nodes.Reverse();
			_isReversed = false;
		}
	}
	
}
