using UnityEngine;
using UnityEngine.Events;
namespace Rellac.UI
{
	public class UIManagerExampleStart : MonoBehaviour
	{
		public UnityEvent onStart;
		// Start is called before the first frame update
		void Start()
		{
			onStart.Invoke();
		}
	}
}