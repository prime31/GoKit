using UnityEngine;
using System.Collections;


/// <summary>
/// TweenFlows are used for creating a chain of Tweens via the append/prepend methods. You can also get timeline
/// like control by inserting Tweens and setting them to start at a specific time. Note that TweenFlows do not
/// honor the delays set within regular Tweens. Use the append/prependDelay method to add any required delays
/// </summary>
public class TweenFlow : AbstractTweenCollection
{
	public TweenFlow() : base()
	{}
	
	
	#region chained property setters
	
	public TweenFlow setId( int id )
	{
		this.id = id;
		return this;
	}

	
	public TweenFlow setIterations( int iterations )
	{
		this.iterations = iterations;
		return this;
	}
	
	
	public TweenFlow setIterations( int iterations, LoopType loopType )
	{
		this.iterations = iterations;
		this.loopType = loopType;
		return this;
	}

	
	public TweenFlow setUpdateType( UpdateType updateType )
	{
		this.updateType = updateType;
		return this;
	}
	
	#endregion
	
	
	#region internal Flow management
	
	/// <summary>
	/// the item being added already has a start time so no extra parameter is needed
	/// </summary>
	private void insert( TweenFlowItem item )
	{
		// early out for invalid items
		if( item.tween != null && !item.tween.isValid() )
			return;
		
		if( float.IsInfinity( item.duration ) )
		{
			Debug.Log( "adding a Tween with infinite iterations to a TweenFlow is not permitted" );
			return;
		}
		
		// ensure the tween isnt already live
		if( item.tween != null )
			Go.removeTween( item.tween );
		
		// add the item then sort based on startTimes
		_tweenFlows.Add( item );
		_tweenFlows.Sort( ( x, y ) =>
		{
			return x.startTime.CompareTo( y.startTime );
		} );
		
		duration = Mathf.Max( item.startTime + item.duration, duration );
		totalDuration = duration * iterations;
	}
	
	#endregion
	
	
	#region Flow management
	
	/// <summary>
	/// inserts a Tween and sets it to start at the given startTime
	/// </summary>
	public TweenFlow insert( float startTime, AbstractTween tween )
	{
		var item = new TweenFlowItem( startTime, tween );
		insert( item );
		
		return this;
	}
	
	#endregion
	

}
