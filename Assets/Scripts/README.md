# HowToAccessStuffInUnity

```C#
//Here is how to access various items in your unity project. Of course, for the
//following methods to work, your script has to (1) incorporate 'using UnityEngine;'
//and (2) inherit 'MonoBehaviour'. You do not however have to worry too much
//about those two requirements since any unity-auto-generated-script implements
//them out of the box.

using UnityEngine;

public class HowToAccessShit : MonoBehaviour {

	//-------------------------------------------------
	//ACCESSING AND HIDING VARIABLES IN THE INSPECTOR
	//-------------------------------------------------
	//A private variable is not accessible in the inspector
	private int privateInteger;
	//A public variable is accessible in the inspector (provided it is of a type that Unity can serialize)
	public int publicInteger;
	//The value of an initialized public variable is overriden by the value appearing in the inspector
	public int overridenInteger = 1;
	//A public variable is made inaccessible in the inspector when tagged as hidden
	[HideInInspector] public int hiddenInteger;
	//A private variable is made accessible in the inspector when tagged as serialized (provided it is of a type that Unity can serialize)
	[SerializeField] private int revealedInteger;

	//A GameObject is the base object. It always come with at least one component, a
	//'Transform' or 'RectTransform'.
	GameObject go;
	//A Component is added as a property of a GameObject. It defines the behaviour of
	//the GameObject. There can be multiple components on a single GameObject.
	Component c;
	//A Transform (or 'RectTransform') is a special component. It defines the position,
	//size and rotation of a 'GameObject'. Insterrestingly enough, when a 'GameObject'
	//has children (other GameObjects), one must pass through the 'Transform' to access
	//those children. Another unique fact about Transforms is that GameObjects have a
	//built-in method to directly access their Transform, a unique feature that is not
	//available with other Components.
	Transform t;

	void Start() {
		//-------------------------------------------------
		//ACCESSING GAMEOBJECTS
		//-------------------------------------------------
		//Access the current GameObject:
		go = this.gameObject;
		//Find a GameObject by its name:
		go = GameObject.Find("InsertGameObjectNameHere");
		//Find a GameObject by its tag:
		go = GameObject.FindGameObjectWithTag("InsertTagHere");
		//Find all GameObjects that share a common tag:
		GameObject[] gos = GameObject.FindGameObjectsWithTag("InsertTagHere");
		//Or simply make go a public variable and assign a GameObject to it in the editor.

		//-------------------------------------------------
		//ACCESSING PARENT AND CHILDREN
		//-------------------------------------------------
		//Find a GameObject that's a child of your GameObject
		go = go.transform.Find("InsertChildsNameHere").gameObject;
		//Find a GameObject that's a parent of your GameObject:
		go = go.transform.parent.gameObject;

		//-------------------------------------------------
		//ACCESSING COMPONENTS
		//-------------------------------------------------
		//Access a GameObject's component by its type:
		c = go.GetComponent<Transform>();
		//Access a GameObject's component by its type:
		Component[] cs = go.GetComponents<Transform>();

		//-------------------------------------------------
		//ACCESSING COMPONENTS IN PARENT AND CHILDREN
		//-------------------------------------------------
		//Access a Component in a GameObject's child:
		c = go.transform.Find("InsertChildNameHere").gameObject.GetComponent<Transform>();
		//Access a Component in a GameObject's parent:
		c = go.transform.parent.gameObject.GetComponent<Transform>();

		//-------------------------------------------------
		//ACCESSING TRANSFORMS
		//-------------------------------------------------
		//Access the current GameObject's Transform:
		t = this.transform; //This is unique to Transforms. Other components must be accessed with the GetComponent method.
							//Access another GameObject's Transform:
		t = GameObject.Find("InsertGameObjectNameHere").transform; //This is unique to Transforms. Other components must be accessed with the GetComponent method.

		//-------------------------------------------------
		//ACCESSING TRANSFORMS IN PARENT AND CHILDREN
		//-------------------------------------------------
		//Access a chidren's Transform:
		t = go.transform.Find("InsertChildNameHere"); //This is unique to Transforms. Other components must be accessed with the GetComponent method.
													  //Access a parent's Transform:
		t = go.transform.parent; //This is unique to Transforms. Other components must be accessed with the GetComponent method.
	}
}
```
