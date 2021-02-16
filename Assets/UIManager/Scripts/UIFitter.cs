using System.Collections;
using UnityEngine;
namespace Rellac.UI {
	public class UIFitter : MonoBehaviour
	{
		/// <summary>
		/// Rect to copy size settings from
		/// </summary>
		[Tooltip("Rect to copy size settings from")]
		[SerializeField] private RectTransform targetRect = null;
		/// <summary>
		/// Rects to resize to target size
		/// </summary>
		[Tooltip("Rects to resize to target size")]
		[SerializeField] private RectTransform[] rectsToResize = null;

		/// <summary>
		/// Fit rects to target size
		/// </summary>
		public void Fit()
		{
			StartCoroutine(FitWait());
		}

		/// <summary>
		/// Sometimes we need to wait a frame for an update before fitting
		/// </summary>
		private IEnumerator FitWait()
		{
			Vector2 size = new Vector2(targetRect.rect.width, targetRect.rect.height);
			for (int i = 0; i < rectsToResize.Length; i++)
			{
				rectsToResize[i].sizeDelta = size;
			}
			yield return new WaitForEndOfFrame();
			for (int i = 0; i < rectsToResize.Length; i++)
			{
				rectsToResize[i].sizeDelta = size;
			}
		}
	}
}