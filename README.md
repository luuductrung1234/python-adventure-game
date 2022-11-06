# python-adventure-game

## unity documentation

[[doc](https://docs.unity.com/)] - [[manual](https://docs.unity3d.com/Manual/index.html)] - [[script references](https://docs.unity3d.com/ScriptReference/index.html)]

1. [Scripting](https://docs.unity3d.com/Manual/ScriptingSection.html) in Unity
   - [Setup up scripting environment](https://docs.unity3d.com/Manual/ScriptingSettingUp.html)
   - [Scripting concepts](https://docs.unity3d.com/Manual/ScriptingConcepts.html)<details><summary>script lifecycle flowchart</summary>![monobehaviour flowchart](https://docs.unity3d.com/uploads/Main/monobehaviour_flowchart.svg)</details>
   - [Important classes](https://docs.unity3d.com/Manual/ScriptingImportantClasses.html)
   - [Unity architecture](https://docs.unity3d.com/Manual/unity-architecture.html)
   - [Plug-ins](https://docs.unity3d.com/Manual/Plugins.html)
   - [C# Job System](https://docs.unity3d.com/Manual/JobSystem.html)
2. [MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html)'s callbacks
   - [Start()](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html), [Update()](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html)
3. Transform - [manual](https://docs.unity3d.com/Manual/class-Transform.html) - [script](https://docs.unity3d.com/ScriptReference/Transform.html)
   - [Transform.Rotate()](https://docs.unity3d.com/ScriptReference/Transform.Rotate.html)
   - [Transform.Translate()](https://docs.unity3d.com/ScriptReference/Transform.Translate.html)
   - [Transform.position](https://docs.unity3d.com/ScriptReference/Transform-position.html) _(can use this property to make a follow camera)_
   - [Transform.rotation](https://docs.unity3d.com/ScriptReference/Transform-rotation.html) is a [quaternion](https://docs.unity3d.com/ScriptReference/Quaternion.html) that stores the rotation of the Transform in world space.
4. [Script serialization](https://docs.unity3d.com/Manual/script-Serialization.html) and [SerializeField](https://docs.unity3d.com/ScriptReference/SerializeField.html)
5. [Input](https://docs.unity3d.com/ScriptReference/Input.html), [Input.GetAxis](https://docs.unity3d.com/ScriptReference/Input.GetAxis.html), [Input.GetButton](https://docs.unity3d.com/ScriptReference/Input.GetButton.html)
6. The interval in seconds from the last frame to the current one (depend on CPU computing strength): [Time.deltaTime](https://docs.unity3d.com/ScriptReference/Time-deltaTime.html)
   - To make a value becomes frame independent, `value * Time.deltaTime`
7. [Collider2D](https://docs.unity3d.com/ScriptReference/Collider2D.html) and [Rigidbody2D](https://docs.unity3d.com/ScriptReference/Rigidbody2D.html) (involve in [Collision2D](https://docs.unity3d.com/ScriptReference/Collision2D.html))
   - `Rigidbody2D` put the sprite under the control of physics engine
   - [OnTriggerEnter2D()](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html), [OnCollisionEnter2D()](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html)
8. Sprites are made of pixels
   - Resolution refers to the number of pixels in an image (higher resolution = more pixels)
9. Unity unit
   - 1 Unity unit has no meaning, just whatever we want it to represent. It could be meters, kilometers, miles, inches,...
10. `Pixels Per Unit`
    - New assets default to 100 pixels per Unity unit.
    - Bigger asset = less `Pixels Per Unit`
11. Creating a `Reference`
    - If we want to access / change / call anything other than this game object's transform, we need to create a reference
    - We need to tell Unity what the "thing" is that we are referring to.
12. Group game objects using [Tags](https://docs.unity3d.com/Manual/Tags.html)
13. Creating [Prefabs](https://docs.unity3d.com/Manual/CreatingPrefabs.html), and [instantiating Prefabs at run time](https://docs.unity3d.com/Manual/InstantiatingPrefabs.html)
14. Spawn game objects randomly/repeatedly <details><summary>_show more_</summary> - [how to spawn an object](https://gamedevbeginner.com/how-to-spawn-an-object-in-unity-using-instantiate/) - [how to spawn anything](https://www.youtube.com/watch?v=gsU7mZv3TtI)</details>
15. [How to reference to another game objects (parent, children)?](https://stackoverflow.com/questions/22377372/unity-how-to-reference-an-object-from-a-different-one)
16. Cinemachine [about](https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/index.html) - [doc](https://docs.unity3d.com/Packages/com.unity.cinemachine@2.3/manual/index.html) <details><summary>_how to add cinemachine_</summary><ul><li>Add the Package Manager window</li> <li>Find and install Cinemachine</li> <li>Add a Virtual Camera</li> <li>Point it to follow the ball</li> <li>Change the Screen X value to show more of whats to come</li> </ul></details>
17. [Effectors 2D](https://docs.unity3d.com/Manual/Effectors2D.html)
18. Instead of rotate object using `transform.Rotate()`, we can use [Rigidbody2D](https://docs.unity3d.com/ScriptReference/Rigidbody2D.html)'s [AddTorque](https://docs.unity3d.com/ScriptReference/Rigidbody2D.AddTorque.html) _(see alsoo [angular drag](https://docs.unity3d.com/ScriptReference/Rigidbody2D-angularDrag.html), [angular velocity](https://docs.unity3d.com/ScriptReference/Rigidbody2D-angularVelocity.html) and [linear drag](https://docs.unity3d.com/ScriptReference/Rigidbody2D-drag.html))_
