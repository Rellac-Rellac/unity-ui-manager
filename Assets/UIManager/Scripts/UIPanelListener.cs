using UnityEngine;
using UnityEngine.Events;
namespace Rellac.UI
{
	public class UIPanelListener : MonoBehaviour
	{
		/// <summary>
		/// Panel to register events 
		/// </summary>
		[Tooltip("Panel to register events")]
		[SerializeField] private UIPanel panel = null;

		/// <summary>
		/// Passes RectTransform of root when this prefab is instantiated
		/// </summary>
		public RectTransformEvent onPanelInstantiated;
		/// <summary>
		/// Passes RectTransform of root when transition has ended
		/// </summary>
		public RectTransformEvent onPanelTransitionedIn;
		/// <summary>
		/// Fired when transition has ended and gameobject has been destroyed
		/// </summary>
		public RectTransformEvent onPanelTransitionedOut;

		private UnityAction<RectTransform> panelInstantiated;
		private UnityAction<RectTransform> panelTransitionedIn;
		private UnityAction<RectTransform> panelTransitionedOut;

		// Start is called before the first frame update
		void Start()
		{
			panelInstantiated = OnPanelInstantiated;
			panelTransitionedIn = OnPanelTransitionedIn;
			panelTransitionedOut = OnPanelTransitionedOut;
			panel.onPanelInstantiated.AddListener(panelInstantiated);
			panel.onPanelTransitionedIn.AddListener(panelTransitionedIn);
			panel.onPanelTransitionedOut.AddListener(panelTransitionedOut);
		}

		private void OnDestroy()
		{
			onPanelInstantiated.RemoveListener(panelInstantiated);
			onPanelTransitionedIn.RemoveListener(panelTransitionedIn);
			onPanelTransitionedOut.RemoveListener(panelTransitionedOut);
		}

		/// <summary>
		/// Panel has been instantiated in the scene
		/// </summary>
		/// <param name="rect">parent rect of ui prefab</param>
		public void OnPanelInstantiated(RectTransform rect)
		{
			onPanelInstantiated.Invoke(rect);
		}

		/// <summary>
		/// Panel has finished its transition in
		/// </summary>
		/// <param name="rect">parent rect of ui prefab</param>
		public void OnPanelTransitionedIn(RectTransform rect)
		{
			onPanelTransitionedIn.Invoke(rect);
		}

		/// <summary>
		/// Panel has finished its transition out
		/// </summary>
		/// <param name="rect">parent rect of ui prefab</param>
		public void OnPanelTransitionedOut(RectTransform rect)
		{
			onPanelTransitionedOut.Invoke(rect);
		}
	}
}