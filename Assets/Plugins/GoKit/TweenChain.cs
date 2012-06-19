using UnityEngine;
using System.Collections;


public class TweenChain : AbstractTweenCollection
{
	public TweenChain() : base()
	{}
	
	
	#region chained property setters
	
	public TweenChain setId( int id )
	{
		this.id = id;
		return this;
	}

	
	public TweenChain setIterations( int iterations )
	{
		this.iterations = iterations;
		return this;
	}
	
	
	public TweenChain setIterations( int iterations, LoopType loopType )
	{
		this.iterations = iterations;
		this.loopType = loopType;
		return this;
	}

	
	public TweenChain setUpdateType( UpdateType updateType )
	{
		this.updateType = updateType;
		return this;
	}
	
	#endregion

	
	#region internal Chain management
	
	private void append( TweenFlowItem item )
	{
		// early out for invalid items
		if( item.tween != null && !item.tween.isValid() )
			return;
		
		if( float.IsInfinity( item.duration ) )
		{
			Debug.Log( "adding a Tween with infinite iterations to a TweenChain is not permitted" );
			return;
		}
		
		// ensure the tween isnt already live
		if( item.tween != null )
			Go.removeTween( item.tween );
		
		_tweenFlows.Add( item );
		
		// update the duration and total duration
		duration += item.duration;
		
		if( iterations > 0 )
			totalDuration = duration * iterations;
		else
			totalDuration = float.PositiveInfinity;
	}
	
	
	private void prepend( TweenFlowItem item )
	{
		// early out for invalid items
		if( item.tween != null && !item.tween.isValid() )
			return;
		
		if( float.IsInfinity( item.duration ) )
		{
			Debug.Log( "adding a Tween with infinite iterations to a TweenChain is not permitted" );
			return;
		}
		
		// ensure the tween isnt already live
		if( item.tween != null )
			Go.removeTween( item.tween );
		
		// fix all the start times on our previous chains
		foreach( var ci in _tweenFlows )
			ci.startTime += item.duration;
		
		_tweenFlows.Add( item );
		
		// update the duration and total duration
		duration += item.duration;
		totalDuration = duration * iterations;
	}
	
	#endregion
	
	
	#region Chain management
	
	/// <summary>
	/// appends a Tween at the end of the current flow
	/// </summary>
	public TweenChain append( AbstractTween tween )
	{
		var item = new TweenFlowItem( duration, tween );
		append( item );
		
		return this;
	}
	
	
	/// <summary>
	/// appends a delay to the end of the current flow
	/// </summary>
	public TweenChain appendDelay( float delay )
	{
		var item = new TweenFlowItem( duration, delay );
		append( item );
		
		return this;
	}
	
	
	/// <summary>
	/// adds a Tween to the front of the flow
	/// </summary>
	public TweenChain prepend( AbstractTween tween )
	{
		var item = new TweenFlowItem( 0, tween );
		prepend( item );
		
		return this;
	}
	
	
	/// <summary>
	/// adds a delay to the front of the flow
	/// </summary>
	public TweenChain prependDelay( float delay )
	{
		var item = new TweenFlowItem( 0, delay );
		prepend( item );
		
		return this;
	}

	#endregion

}
