using UnityEngine;

namespace Links
{
	public sealed class LayerAttribute : UnityEngine.PropertyAttribute
	{
#if UNITY_EDITOR
		[UnityEditor.CustomPropertyDrawer(typeof(LayerAttribute))]
		class Drawer : UnityEditor.PropertyDrawer
		{
			public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
			{
				LayerAttribute layerAttribute = (LayerAttribute)attribute;
				if (layerAttribute == null)
					return;
				if(property.propertyType != UnityEditor.SerializedPropertyType.Integer) {
					Rect contentPosition = UnityEditor.EditorGUI.PrefixLabel(position, new GUIContent(" "));
					UnityEditor.EditorGUI.LabelField(position, label);
					UnityEditor.EditorGUI.HelpBox(contentPosition, "整数型のみサポートしています", UnityEditor.MessageType.Error);
					return;
				}
				property.intValue = UnityEditor.EditorGUI.LayerField(position, label, property.intValue);
			}
		}
#endif
	}
}
