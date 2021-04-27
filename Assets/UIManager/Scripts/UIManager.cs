using System.Collections;
using UnityEngine;
namespace Rellac.UI
{
	[CreateAssetMenu(fileName = "New UI Manager", menuName = "UI/Manager", order = 1)]
	public class UIManager : ScriptableObject
	{
		/// <summary>
		/// Manager loops between a selection of UIPanels
		/// </summary>
		[Tooltip("")]
		[SerializeField] private bool containsLoopGroup = false;
		/// <summary>
		/// Panel to start when manager is initialised - no transition will be made
		/// </summary>
		[Tooltip("Panel to start when manager is initialised - no transition will be made")]
		[SerializeField] private UIPanel initialPanel = null;
		/// <summary>
		/// Speed to play transitions across a loop group
		/// </summary>
		[Tooltip("Speed to play transitions across a loop group")]
		[SerializeField] private float loopTransitionSpeed = 1;
		/// <summary>
		/// Animation to play when transitioning to the previous panel in a loop group
		/// </summary>
		[Tooltip("Animation to play when transitioning to the previous panel in a loop group")]
		[SerializeField] private UITransition prevTransition = null;
		/// <summary>
		/// Animation to play when transitioning to the next panel in a loop group
		/// </summary>
		[Tooltip("Animation to play when transitioning to the next panel in a loop group")]
		[SerializeField] private UITransition nextTransition = null;
		/// <summary>
		/// Group of UIPanels to transition between is manager contains a loop group
		/// </summary>
		[Tooltip("Group of UIPanels to transition between is manager contains a loop group")]
		[SerializeField] private UIPanel[] loopGroup = null;

		MonoBehaviour root_;
		/// <summary>
		/// Root MonoBehaviour of scene reference
		/// </summary>
		public MonoBehaviour root
		{
			get
			{
				if (root_ == null) Debug.LogError("Initialise UI Manager!");
				return root_;
			}
		}

		private Animator animator_;
		/// <summary>
		/// Animator component handling transitions
		/// </summary>
		public Animator animator
		{
			get
			{
				if (animator_ == null) Debug.LogError("Initialise UI Manager!");
				return animator_;
			}
		}

		private Transform inParent;
		private Transform outParent;
		private GameObject clickBlocker;
		private GameObject passOverBlocker;

		private bool transitioning = false;
		private UIPanel currentPanel;

		private int loopIdx_ = 0;
		private int loopIdx
		{
			get
			{
				return loopIdx_;
			}
			set
			{
				loopIdx_ = value;
				if (loopIdx_ >= loopGroup.Length)
				{
					loopIdx_ = 0;
				}
				if (loopIdx < 0)
				{
					loopIdx = loopGroup.Length - 1;
				}
			}
		}

		/// <summary>
		/// Initialise manager in scene
		/// </summary>
		/// <param name="parent">RectTransform to assign as parent</param>
		public void Initialise(RectTransform parent)
		{
			root_ = ((GameObject)Instantiate(Resources.Load("UIManager/UI Root"), parent)).GetComponent<MonoBehaviour>();
			animator_ = root.GetComponentInChildren<Animator>();
			inParent = animator.transform.Find("Parent_In");
			outParent = animator.transform.Find("Parent_Out");
			clickBlocker = animator.transform.Find("ClickBlocker").gameObject;
			passOverBlocker = animator.transform.Find("PassOverBlocker").gameObject;
			root.GetComponent<UIFitter>().Fit();

			UIPanel panel = initialPanel;
			RectTransform panelRoot = (RectTransform)Instantiate(panel.panelPrefab, inParent).transform;
			animator.Play("Instant");
			panel.Initialise(this, panelRoot);
			panel.OnPanelTransitionedIn(this, panelRoot);
			currentPanel = panel;
			transitioning = false;
		}

		/// <summary>
		/// Clears out system, requiring another Initialise to use again
		/// </summary>
		public void Clear()
		{
			if (root_ != null)
			{
				Destroy(root.gameObject);
			}
		}

		/// <summary>
		/// Trigger a new panel transition
		/// </summary>
		/// <param name="input">UIPanel to transition to</param>
		public void SetPanel(UIPanel input)
		{
			if (TryPreparePanel(input, input.transition, input.animationSpeed))
			{
				input.PlayTransition(this, input.transition, input.animationSpeed);
				transitioning = true;
			}
		}

		public void NextPanel()
		{
			if (CheckIsValidLoopGroup("NextPanel()"))
			{
				loopIdx++;
				if (TryPreparePanel(loopGroup[loopIdx], nextTransition, loopTransitionSpeed))
				{
					loopGroup[loopIdx].PlayTransition(this, nextTransition, containsLoopGroup ? loopTransitionSpeed : loopGroup[loopIdx].animationSpeed);
					transitioning = true;
				}
			}
		}

		public void PreviousPanel()
		{
			if (CheckIsValidLoopGroup("PreviousPanel()"))
			{
				loopIdx--;
				if (TryPreparePanel(loopGroup[loopIdx], prevTransition, loopTransitionSpeed))
				{
					loopGroup[loopIdx].PlayTransition(this, prevTransition, containsLoopGroup ? loopTransitionSpeed : loopGroup[loopIdx].animationSpeed);
					transitioning = true;
				}
			}
		}

		private bool CheckIsValidLoopGroup(string functionAttempt)
		{
			if (!containsLoopGroup)
			{
				Debug.LogError("Trying to Call " + functionAttempt + " on a UIManager that doesn't contain a loop group");
				return false;
			}
			if (loopGroup.Length == 0)
			{
				Debug.LogError("No panels listed in loop group of " + name);
				return false;
			}
			return true;
		}

		private bool TryPreparePanel(UIPanel input, UITransition transition, float speed)
		{
			if (transitioning)
			{
				Debug.LogError("Wait for transition to end before setting new panel");
				return false;
			}
			if (currentPanel != null)
			{
				currentPanel.onPanelTransitionOutStarted.Invoke((RectTransform)root_.transform);
			}

			clickBlocker.SetActive(true);
			passOverBlocker.SetActive(input.blockPassOver);

			inParent.SetSiblingIndex(transition.panelOnTop == UITransition.ParentSelection.inComingPanel ? 2 : 1);

			foreach (Transform child in inParent)
			{
				child.SetParent(outParent);
				RectTransform rt = (RectTransform)child;
				rt.anchorMin = Vector2.zero;
				rt.anchorMax = Vector2.one;
				rt.offsetMin = rt.offsetMax = Vector2.zero;
			}
			root.StartCoroutine(WaitForAnimationEnd(input, transition, speed));
			input.Initialise(this, (RectTransform)Instantiate(input.panelPrefab, inParent).transform);
			return true;
		}

		private IEnumerator WaitForAnimationEnd(UIPanel panel, UITransition transition, float speed)
		{
			yield return new WaitForSeconds(transition.inAnimation.length * speed);
			ClearOutParent();
			transitioning = false;
			animator.Play("Idle");
			currentPanel.onPanelTransitionedOut.Invoke(currentPanel.GetPanel());
			currentPanel = panel;
			clickBlocker.SetActive(false);
		}

		private void ClearOutParent()
		{
			foreach (Transform child in outParent)
			{
				Destroy(child.gameObject);
			}
		}
	}
}