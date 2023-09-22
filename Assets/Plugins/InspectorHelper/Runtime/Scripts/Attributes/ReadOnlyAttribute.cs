using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Links
{
	public sealed class ReadOnlyAttribute : PropertyAttribute
	{
#if UNITY_EDITOR
		[UnityEditor.CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
		class Drawer : UnityEditor.PropertyDrawer
		{
			public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
			{
				//無効にしてPropertyFieldを描画する
				using (new UnityEditor.EditorGUI.DisabledScope(true)) {
					UnityEditor.EditorGUI.PropertyField(position, property);
				}
			}
		}
#endif
	}
}
