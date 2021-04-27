using UnityEditor;
namespace Rellac.UI.Editor
{
	[CustomEditor(typeof(UIManager))]
	[CanEditMultipleObjects]
	public class UIManagerEditor : UnityEditor.Editor
	{
		SerializedProperty containsLoopGroup;
		SerializedProperty initialPanel;
		SerializedProperty loopTransitionSpeed;
		SerializedProperty prevTransition;
		SerializedProperty nextTransition;
		SerializedProperty loopGroup;

		void OnEnable()
		{
			containsLoopGroup = serializedObject.FindProperty("containsLoopGroup");
			initialPanel = serializedObject.FindProperty("initialPanel");
			loopTransitionSpeed = serializedObject.FindProperty("loopTransitionSpeed");
			prevTransition = serializedObject.FindProperty("prevTransition");
			nextTransition = serializedObject.FindProperty("nextTransition");
			loopGroup = serializedObject.FindProperty("loopGroup");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(containsLoopGroup);
			EditorGUILayout.PropertyField(initialPanel);
			if (containsLoopGroup.boolValue)
			{
				EditorGUILayout.PropertyField(loopTransitionSpeed);
				EditorGUILayout.PropertyField(prevTransition);
				EditorGUILayout.PropertyField(nextTransition);
				EditorGUILayout.PropertyField(loopGroup);
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}