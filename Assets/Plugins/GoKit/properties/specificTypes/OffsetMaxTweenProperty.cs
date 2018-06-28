using UnityEngine;

public sealed class OffsetMaxTweenProperty : AbstractOffsetTweenProperty
{
    public OffsetMaxTweenProperty( Vector2 endValue, bool isRelative = false ) : base( endValue, isRelative, useMax: true )
    {
    }
}
