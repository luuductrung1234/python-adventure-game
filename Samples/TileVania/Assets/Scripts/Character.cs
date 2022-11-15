using UnityEngine;
using IronPython.Hosting;

public class Character : MonoBehaviour
{
	void Start()
	{
		var eng = Python.CreateEngine();
		var scope = eng.CreateScope();
		eng.Execute(@"
def greetings(name):
		return 'Hello ' + name.title() + '!'
", scope);
		dynamic greetings = scope.GetVariable("greetings");
		Debug.Log(greetings("World"));
	}
}

