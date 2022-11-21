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
19. UI Canvas [basic layout](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/UIBasicLayout.html) <details><summary>_show more_</summary><ul> <li>UI = User Interface</li> <li>Include text, buttons, sliders, menus, etc. (user can interact with, not part of game world itself)</li> <li>UI elements live on the "Canvas"</li> <li>The canvas generally exist in "Screen Space" and is mostly separate from the game world</li> <li>You can have multiple canvases</li> </ul></details>
20. [ScriptableObject](https://docs.unity3d.com/ScriptReference/ScriptableObject.html) helps to create objects that don't need to be attached to any game objects. It is most useful for assets which are only meant to store data.
21. Change sprite of GameObject using [Image.sprite](https://docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Image-sprite.html).
22. Use [SceneManager.LoadScene](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html) to load specific scene (usually apply to perform replay a game).

## ironpython

[IronPython 2](https://github.com/IronLanguages/ironpython2), [IronPython 3](https://github.com/IronLanguages/ironpython3)

- [IronPython and Unity](https://shrigsoc.blogspot.com/2016/07/ironpython-and-unity.html). To find needed dlls, first, install IronPython. Then looking for install location on your machine:
  - macos: `/Library/Frameworks/Mono.framework/Versions/Current/lib/ironpython`
  - window: `C:\Program Files (x86)\IronPython {version}\Lib`
- [Use the _dynamic_ keyword/.NET 4.6 feature in Unity](https://stackoverflow.com/questions/45616562/use-the-dynamic-keyword-net-4-6-feature-in-unity)
- The heart of IronPython and any other dynamic languages that run on .NET platform is the [Dynamic Language Runtime (DLR)](https://github.com/IronLanguages/dlr) - [overview](https://learn.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/dynamic-language-runtime-overview#dlr-documentation) - [architecture](https://learn.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/dynamic-language-runtime-overview#dlr-architecture) - [documentation](https://github.com/IronLanguages/dlr/tree/master/Docs). DLR adds a set of services on top of CLR for better supporting dynamic languages, includes
  - [Expression Trees](https://github.com/IronLanguages/dlr/blob/master/Docs/expr-tree-spec.pdf)
  - Call site caching
  - Dynamic ObjectInteroperability.

### further read

- [Expression Tree (C#)](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/)
- [Difference between **design-time** and **runtime** framework](https://stackoverflow.com/questions/4787406/difference-between-design-time-and-runtime-framework)
