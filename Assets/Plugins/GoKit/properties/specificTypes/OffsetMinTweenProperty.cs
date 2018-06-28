using UnityEngine;

public sealed class OffsetMinTweenProperty : AbstractOffsetTweenProperty
{
    public OffsetMinTweenProperty( Vector2 endValue, bool isRelative = false ) : base( endValue, isRelative, useMax: false )
    {
    }
}
