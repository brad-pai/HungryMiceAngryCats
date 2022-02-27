using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TweenManager : MonoBehaviour {

	public abstract class Tween : MonoBehaviour {
		public int id { get; protected set; }
		public Transform targetTransform { get; protected set; }
		protected float beginTime;
		protected float endTime;
		protected AnimationCurve animationCurve;
		protected System.Action<Tween> managerCallBackFunction;
		protected System.Action callBackFunction;

		public abstract void Stop();
	}

	public static TweenManager instance { get; private set; }
	private List<Tween> currentTweenList = new List<Tween>();
	private int idCounter = 0;
	public AnimationCurve tweenScaleDefault;

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
	}

	//Discard a tween from the currentTweenList
	public void DiscardTween(Tween tween) {
		currentTweenList.Remove(tween);
	}

	//Returns the next available ID and increments the idCounter
	private int GetNextAvailableId() {
		return idCounter++;
	}

	//Call this function to start a scaling tween with the specified parameters. The function returns the Tween's ID.
	public int TweenScale(Transform targetTransform, AnimationCurve animationCurve, System.Action callBackFunction) {
		StopTween(targetTransform);
		TweenScale tweenScale = gameObject.AddComponent<TweenScale>();
		tweenScale.Initialize(GetNextAvailableId(), targetTransform, animationCurve, DiscardTween, callBackFunction);
		currentTweenList.Add(tweenScale);
		return tweenScale.id;
	}

	public void StopTween(Transform targetTransform) {
		foreach (Tween tween in currentTweenList) {
			if (tween.targetTransform == targetTransform) {
				tween.Stop();
				break;
			}
		}
	}

	//Stops the Tween with the passed ID if it still exists
	public void StopTween(int id) {
		foreach (Tween tween in currentTweenList) {
			if (tween.id == id) {
				tween.Stop();
				break;
			}
		}
	}

	//Tests the tween scale
	public void TestTweenScale() {
		TweenScale(transform, tweenScaleDefault, null);
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(TweenManager))]
public class TweenManagerEditor : Editor {
	public override void OnInspectorGUI() {
		var tweenManager = (TweenManager)target;
		if (tweenManager == null) {
			return;
		}

		if (GUILayout.Button("Test tween scale")) {
			if (Application.isPlaying) {
				TweenManager.instance.TestTweenScale();
			} else {
				Debug.Log("Start the game first.");
			}
		}
		DrawDefaultInspector();
	}
}
#endif

