using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tool : MonoBehaviour {
	[HideInInspector]
	public string toolName;

	[HideInInspector]
	public Camera cam;
	[HideInInspector]
	public Text feedback;
	[HideInInspector]
	public LineRenderer line;

	public virtual void Activate() { }
	public virtual void Deactivate() { }
	public virtual void Active() { }
	public virtual void FixedActive() { }

	public virtual void OnLMouse(bool shift) { }
	public virtual void OnMMouse(bool shift) { }
	public virtual void OnRMouse(bool shift) { }
	public virtual void OnScroll(float scroll) { }

	protected bool RaycastForInteractable(float reach, ref Interactable interactable) {
        RaycastHit hit;
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

		if (Physics.Raycast(ray: ray, hitInfo: out hit, maxDistance: reach, layerMask: 1) && hit.collider.gameObject.GetComponent<Interactable>() != null) {
			interactable = hit.collider.gameObject.GetComponent<Interactable>().attachedRigidBody.gameObject.GetComponent<Interactable>();
			return true;
		} else return false;
	}

	protected bool RaycastForComponent<T> (float reach, ref T component) where T : MonoBehaviour {
        RaycastHit hit;
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

		if (Physics.Raycast(ray, out hit, reach) && hit.collider.gameObject.GetComponent<T>() != null) {
			component = hit.collider.gameObject.GetComponent<T>();
			return true;
		} else return false;
	}

	public void Feedback(string text) {
		feedback.text = text;
	}
}
