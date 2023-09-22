using UnityEngine;

namespace Links
{
	public sealed class SliderAttribute : PropertyAttribute
	{
		public readonly float LeftValue;
		public readonly float RightValue;

		public SliderAttribute(float leftValue, float rightValue)
		{
			this.LeftValue = leftValue;
			this.RightValue = rightValue;
		}

#if UNITY_EDITOR
		[UnityEditor.CustomPropertyDrawer(typeof(SliderAttribute))]
		class Drawer : UnityEditor.PropertyDrawer
		{
			public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
			{
				SliderAttribute sliderAttribute = (SliderAttribute)attribute;
				if (sliderAttribute == null)
					return;
				//float型用スライダー
				UnityEditor.EditorGUI.Slider(position, property, sliderAttribute.LeftValue, sliderAttribute.RightValue, label);
			}
		}
#endif
	}
}