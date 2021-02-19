﻿using System.Collections;
using UnityEngine;
namespace Rellac.UI
{
	[CreateAssetMenu(fileName = "New UI Panel", menuName = "UI/Panel", order = 1)]
	public class UIPanel : ScriptableObject
	{
		/// <summary>
		/// Prefab containing content for this panel
		/// </summary>
		[Tooltip("Prefab containing content for this panel")]
		public GameObject panelPrefab;
		/// <summary>
		/// Transition used for this animation
		/// </summary>
		[Tooltip("Transition used for this animation")]
		public UITransition transition;
		/// <summary>
		/// Speed in seconds to run transition animation
		/// </summary>
		[Tooltip("Speed in seconds to run transition animation")]
		public float animationSpeed = 1;
		/// <summary>
		/// Enable click blocker behind this panel
		/// </summary>
		[Tooltip("Enable click blocker behind this panel")]
		public bool blockPassOver = false;
		/// <summary>
		/// Passes RectTransform of root when this prefab is instantiated
		/// </summary>
		public RectTransformEvent onPanelInstantiated;
		/// <summary>
		/// Passes RectTransform of root when transition has ended
		/// </summary>
		public RectTransformEvent onPanelTransitionedIn;
		/// <summary>
		/// Passes RectTransform of root when transition out begins
		/// </summary>
		public RectTransformEvent onPanelTransitionOutStarted;
		/// <summary>
		/// Fired when transition has ended and gameobject has been destroyed
		/// </summary>
		public RectTransformEvent onPanelTransitionedOut;
		
		private RectTransform instantiation;
		/// <summary>
		/// Get root of panel
		/// </summary>
		/// <returns>root GameObject</returns>
		public RectTransform GetPanel()
		{
			return instantiation;
		}

		/// <summary>
		/// Initialise with instantiated panel
		/// </summary>
		/// <param name="input">new root panel in scene</param>
		public void Initialise(RectTransform input)
		{
			instantiation = input;
			onPanelInstantiated.Invoke(instantiation);
		}

		/// <summary>
		/// Play set animation transition for this panel
		/// </summary>
		/// <param name="manager">UIManager handling Animation</param>
		public void PlayTransition(UIManager manager, UITransition transition_, float speed)
		{
			manager.animator.speed = 1f / speed;
			manager.animator.Play(transition_.inAnimation.name);
			manager.root.GetComponent<MonoBehaviour>().StartCoroutine(WaitForTransitionIn(instantiation, speed));
		}

		public IEnumerator WaitForTransitionIn(RectTransform parent, float speed)
		{
			yield return new WaitForSeconds(transition.inAnimation.length * speed);
			onPanelTransitionedIn.Invoke(parent);
		}

	}
}