using UnityEngine;
using UnityEngine.UI;
namespace Rellac.UI
{
	[RequireComponent(typeof(Image))]
	public class UIManagerExampleColourSwapper : MonoBehaviour
	{
		public Color[] potentialColours;
		// Start is called before the first frame update
		void Start()
		{
			GetComponent<Image>().color = potentialColours[Random.Range(0, potentialColours.Length)];
		}
	}
}