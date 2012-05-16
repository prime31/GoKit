GoKit
====

Lightweight tween library for Unity aimed at making tweening objects dead simple and completely flexible. API docs are available
[here](http://prime31.com/unity/docs/#goKitDoc) and the [wiki](https://github.com/prime31/GoKit/wiki) contains usage information and examples.



Meet the GoKit Players
-----

* **Tween**: base class that handles a list of TweenProperties to be animated concurrently, tween duration, loop count/type and the object to perform the animations on. Hang on to a reference if you need to control the Tween after it starts (play, pause, reverse, restart, goto, etc). Tweens are automatically removed from GoKit
when they complete by default. If you set them not to be automatically removed, they will be left in the tween engine and you can restart/reverse/reset them at a later time. Tweens can be set to update in the standard Update, FixedUpdate or LateUpdate methods or via a time scale independent update (handy when animating something while time scale is set to 0).
* **TweenConfig**: passed to the Tween constructor and contains a list of the TweenProperties, ease type, completion handler, etc. These can be created using method chaining for a JQuery-like flow or via standard instantiation and property setting. They're fully reuseable so save these if you intend to create multiple Tweens with the same config.
* **TweenProperty**: houses the value to tween to and the ease function to use while tweening. Optionally, each TweenProperty can be set to relative, meaning
the property will move by the end value specified rather than to it. Fully reuseable.
* **TweenChain**: used to manage a series of Tweens (or TweenChains/TweenFlows). You can append/prepend elements (and delay intervals) to make a single chain of animations. It offers the same playback controls as a standard Tween except they operate on the entire chain instead of a single Tween.
* **TweenFlow**: used to manage a timeline of Tweens (or TweenChains/TweenFlows) each with specific start time. Contrary to a TweenChain where each member
must complete before the next starts TweenFlows allow you to overlap playback of it's members. TweenFlows offer the same playback controls as a standard Tween except they operate on the entire flow instead of a single Tween.


What can I Tween?
-----

Short answer: anything. You can tween any property on any object of type Vector2, Vector3 (including through a series of Vector3s), Vector4, int, float or Color. These make up what we call generic tweens. Generic tweens are slightly slower than specific tweens. We did some heavy-duty benchmarking and used some .NET trickery to make them pretty darn fast though. That being said, the library also offers what we call specific tweens. These are confined to a specific target object type and property and use direct access for the tween making them hyper fast. The available specific tweens are eulerAngles, localEulerAngles, material colors (_Color, _SpecColor, _Emission and _ReflectColor), position, localPosition (along with following a path) and scale. You can also always make your own specific Tweens using the fully extensible TweenProperty system.


Extension Methods For Ease of Use and Syntax Sugar (sorry UnityScript users)
----

GoKit adds extension methods to the Transform and Material classes for easy access to creating often-used single property animations. If there is a particular
animation combo that you use often, use the GoKitTweenExtensions class as a template to make your own Extension methods on any class or object.


License
----
For any developers just wanting to use GoKit in their games go right ahead.  You can use GoKit in any and all games either modified or unmodified.  In order to keep the spirit of this open source project it is expressly forbid to sell or commercially distribute GoKit outside of your games. You can freely use it in as many games as you would like but you cannot commercially distribute the source code either directly or compiled into a library outside of your game.

Feel free to include a "prime31 inside" logo on your about page, web page, splash page or anywhere else your game might show up if you would like.  
[small](http://prime31.com/assets/images/prime31InsideSmall.png) or 
[larger](http://prime31.com/assets/images/prime31Inside.png) or
[huge](http://prime31.com/assets/images/prime31InsideHuge.png)
