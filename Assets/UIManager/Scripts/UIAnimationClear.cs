using UnityEngine;
namespace Rellac.UI
{
	/// <summary>
	/// This class exists as a way for an animation to clear the out parent earlier than at the end of the transition if needed
	/// </summary>
	public class UIAnimationClear : MonoBehaviour
	{
		/// <summary>
		/// Parent to clear on call
		/// </summary>
		[Tooltip("Parent to clear on call")]
		[SerializeField] private Transform outParent = null;

		/// <summary>
		/// Clear all children under specified parent
		/// </summary>
		public void ClearOutParent()
		{
			foreach (Transform child in outParent)
			{
				Destroy(child.gameObject);
			}
		}
	}
}