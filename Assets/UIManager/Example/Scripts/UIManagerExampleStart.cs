using UnityEngine;
using UnityEngine.Events;
namespace DrawAndCode.Examples.UI
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