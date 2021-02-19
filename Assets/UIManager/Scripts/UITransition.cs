using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rellac.UI
{
	[CreateAssetMenu(fileName = "New UI Transition", menuName = "UI/Transition", order = 1)]
	public class UITransition : ScriptableObject
	{
		/// <summary>
		/// Panel rendered on top during this transition
		/// </summary>
		[Tooltip("Panel rendered on top during this transition")]
		public ParentSelection panelOnTop;
		/// <summary>
		/// Animation to play when this panel is called
		/// Must be listed in master Animation Controller
		/// </summary>
		[Tooltip("Animation to play when this panel is called\nMust be listed in master Animation Controller")]
		public AnimationClip inAnimation;

		public enum ParentSelection
		{
			inComingPanel,
			outGoingPanel
		}
	}
}