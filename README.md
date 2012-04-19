UnnamedTweenLib
===============

Lightweight tween library for Unity



Some Important Definitions
-----

* **Tween**: base class that handles a list of TweenProperties to be animated concurrently, tween duration and the object to perform the animations on. Hang on to a reference if
you need to control the Tween after it starts (play, pause, reverse, etc). These auto-destruct by default. If you set them not to auto-destruct they will
be left in the tween engine and you can restart/reverse them at a later time. Tweens can be set to update in the standard Update, FixedUpdate or LateUpdate methods
or via a time scale independent update (handy when animating something while time scale is set to 0).
* **TweenConfig**: passed to the Tween constructor and contains a list of the TweenProperties, ease type, completion handler, etc. These can be created
using method chaining for a JQuery like flow or via standard instantiation and property setting. Fully reuseable so save these if you intend
to create multiple Tweens with the same config.
* **TweenProperty**: houses the value to tween to and the ease function to use while tweening. Optionally, each TweenProperty can be set to relative, meaning
the property will move by the end value specified rather than to it. Fully reuseable.


What can I Tween?
-----

You can tween any property on any object of type Vector2, Vector3, int, float or Color. These make up what we call generic tweens. Generic tweens are slightly slower
than specific tweens (more on that in a moment). We did some heavy-duty benchmarking and used some .NET trickery to make them pretty darn fast. That being
said the library offers what we call specific tweens. These are confined to a specific target object type and property and use direct access for the tween
making them hyper fast. The available specific tweens are eulerAngles, localEulerAngles, material colors (_Color, _SpecColor, _Emission and _ReflectColor),
position, localPosition and scale.