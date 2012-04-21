GoKit
====

Lightweight tween library for Unity aimed at making tweening objects dead simple and completely flexible



Some Important Definitions
-----

* **Tween**: base class that handles a list of TweenProperties to be animated concurrently, tween duration, loop count/type and the object to perform the animations on. Hang on to a reference if you need to control the Tween after it starts (play, pause, reverse, restart, goto, etc). Tween are automatically removed from GoKit
when they complete by default.
If you set them not to be automatically removed, they will be left in the tween engine and you can restart/reverse/reset them at a later time. Tweens can be
set to update in the standard Update, FixedUpdate or LateUpdate methods or via a time scale independent update (handy when animating
something while time scale is set to 0).
* **TweenConfig**: passed to the Tween constructor and contains a list of the TweenProperties, ease type, completion handler, etc. These can be created
using method chaining for a JQuery like flow or via standard instantiation and property setting. Fully reuseable so save these if you intend
to create multiple Tweens with the same config.
* **TweenProperty**: houses the value to tween to and the ease function to use while tweening. Optionally, each TweenProperty can be set to relative, meaning
the property will move by the end value specified rather than to it. Fully reuseable.
* **TweenFlow**: can be used to manage a series of Tweens. You can append/prepend Tweens (and delay intervals) to make a single chain of animations or
you can specify an exact start time for any Tween to get timeline-like control over your Tweens. It offers the same playback controls as a standard Tween except they operate on the entire chain instead of a single Tween.


What can I Tween?
-----

You can tween any property on any object of type Vector2, Vector3 (including along a series of Vector3s), Vector4, int, float or Color.
These make up what we call generic tweens.
Generic tweens are slightly slower than specific tweens. We did some heavy-duty benchmarking and used some .NET trickery to make them pretty darn fast though.
That being said the library offers what we call specific tweens as well. These are confined to a specific target object type and property and use
direct access for the tween making them hyper fast. The available specific tweens are eulerAngles, localEulerAngles, material colors (_Color, _SpecColor, _Emission and _ReflectColor), position, localPosition (along with following a path) and scale. You can also always make your own specific tween using the extensible
TweenProperty systeml


Extension Methods For Easy Use and Syntax Sugar (sorry UnityScript users)
----

GoKit adds extension methods to the Object, Transform, GameObject and Material classes for easy access to creating single property animations. If there is a particular
animation combo that you use often, use the GoKitTweenExtensions class as a template to make your own Extension methods on any class or object.