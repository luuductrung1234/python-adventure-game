# python-adventure-game

## unity documentation

[[doc](https://docs.unity.com/)] - [[manual](https://docs.unity3d.com/Manual/index.html)] - [[script references](https://docs.unity3d.com/ScriptReference/index.html)]

1. [Scripting](https://docs.unity3d.com/Manual/ScriptingSection.html) in Unity
   - [Setup up scripting environment](https://docs.unity3d.com/Manual/ScriptingSettingUp.html)
   - [Scripting concepts](https://docs.unity3d.com/Manual/ScriptingConcepts.html)
   - [Important classes](https://docs.unity3d.com/Manual/ScriptingImportantClasses.html)
   - [Unity architecture](https://docs.unity3d.com/Manual/unity-architecture.html)
   - [Plug-ins](https://docs.unity3d.com/Manual/Plugins.html)
   - [C# Job System](https://docs.unity3d.com/Manual/JobSystem.html)
2. [Transform](https://docs.unity3d.com/ScriptReference/Transform.html)
   - [Transform.Rotate()](https://docs.unity3d.com/ScriptReference/Transform.Rotate.html)
   - [Transform.Translate()](https://docs.unity3d.com/ScriptReference/Transform.Translate.html)
   - [Transform.position](https://docs.unity3d.com/ScriptReference/Transform-position.html) _(can use this property to make a follow camera)_
3. [MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html)'s callbacks
   - [Start()](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html), [Update()](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html)
   - [OnTriggerEnter2D()](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html), [OnCollisionEnter2D()](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html)
4. [Script serialization](https://docs.unity3d.com/Manual/script-Serialization.html) and [SerializeField](https://docs.unity3d.com/ScriptReference/SerializeField.html)
5. [Input](https://docs.unity3d.com/ScriptReference/Input.html), [Input.GetAxis](https://docs.unity3d.com/ScriptReference/Input.GetAxis.html), [Input.GetButton](https://docs.unity3d.com/ScriptReference/Input.GetButton.html)
6. The interval in seconds from the last frame to the current one (depend on CPU computing strength): [Time.deltaTime](https://docs.unity3d.com/ScriptReference/Time-deltaTime.html)
   - To make a value becomes frame independent, `value * Time.deltaTime`
7. [Collider2D](https://docs.unity3d.com/ScriptReference/Collider2D.html) and [Rigidbody2D](https://docs.unity3d.com/ScriptReference/Rigidbody2D.html) (involve in [Collision2D](https://docs.unity3d.com/ScriptReference/Collision2D.html))
   - `Rigidbody2D` put the sprite under the control of physics engine
8. Sprites are made of pixels
   - Resolution refers to the number of pixels in an image (higher resolution = more pixels)
9. Unity unit
   - 1 Unity unit has no meaning, just whatever we want it to represent. It could be meters, kilometers, miles, inches,...
10. `Pixels Per Unit`
    - New assets default to 100 pixels per Unity unit.
    - Bigger asset = less `Pixels Per Unit`
