using System.Collections;
using UnityEngine;
namespace Rellac.UI
{
	[CreateAssetMenu(fileName = "New UI Manager", menuName = "UI/Manager", order = 1)]
	public class UIManager : ScriptableObject
	{
		/// <summary>
		/// Panel to start when manager is initialised - no transition will be made
		/// </summary>
		[Tooltip("Panel to start when manager is initialised - no transition will be made")]
		[SerializeField] private UIPanel initialPanel = null;

		RectTransform root_;
		/// <summary>
		/// Root RectTransform of scene reference
		/// </summary>
		public RectTransform root
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

		/// <summary>
		/// Initialise manager in scene
		/// </summary>
		/// <param name="parent">RectTransform to assign as parent</param>
		public void Initialise(RectTransform parent)
		{
			root_ = (RectTransform)((GameObject)Instantiate(Resources.Load("UIManager/UI Root"), parent)).transform;
			animator_ = root.GetComponentInChildren<Animator>();
			inParent = animator.transform.Find("Parent_In");
			outParent = animator.transform.Find("Parent_Out");
			clickBlocker = animator.transform.Find("ClickBlocker").gameObject;
			passOverBlocker = animator.transform.Find("PassOverBlocker").gameObject;
			root.GetComponent<UIFitter>().Fit();
			foreach (UIFitter fitter in root.GetComponentsInChildren<UIFitter>())
			{
				fitter.Fit();
			}

			RectTransform panelRoot = (RectTransform)Instantiate(initialPanel.panelPrefab, inParent).transform;
			animator.Play("Instant");
			initialPanel.Initialise(panelRoot);
			root.GetComponent<MonoBehaviour>().StartCoroutine(initialPanel.WaitForTransitionIn(panelRoot));
			currentPanel = initialPanel;
			transitioning = false;
		}

		/// <summary>
		/// Trigger a new panel transition
		/// </summary>
		/// <param name="input">UIPanel to transition to</param>
		public void SetPanel(UIPanel input)
		{
			if (transitioning)
			{
				Debug.LogError("Wait for transition to end before setting new panel");
				return;
			}
			if (currentPanel != null)
			{
				currentPanel.onPanelTransitionOutStarted.Invoke(root_);
			}

			clickBlocker.SetActive(true);
			passOverBlocker.SetActive(input.blockPassOver);

			inParent.SetSiblingIndex(input.transition.panelOnTop == UITransition.ParentSelection.inComingPanel ? 2 : 1);

			foreach (Transform child in inParent)
			{
				child.SetParent(outParent);
				RectTransform rt = (RectTransform)child;
				rt.anchorMin = Vector2.zero;
				rt.anchorMax = Vector2.one;
				rt.offsetMin = rt.offsetMax = Vector2.zero;
			}
			root.GetComponent<MonoBehaviour>().StartCoroutine(WaitForAnimationEnd(input));
			input.Initialise((RectTransform)Instantiate(input.panelPrefab, inParent).transform);
			input.PlayTransition(this);
			transitioning = true;
		}

		private IEnumerator WaitForAnimationEnd(UIPanel panel)
		{
			yield return new WaitForSeconds(panel.transition.inAnimation.length * panel.animationSpeed);
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