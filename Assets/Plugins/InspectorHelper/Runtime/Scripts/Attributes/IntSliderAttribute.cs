using UnityEngine;

namespace Links
{
	public sealed class IntSliderAttribute : PropertyAttribute
	{
		public readonly int Min;
		public readonly int Max;

		public IntSliderAttribute(int min, int max)
		{
			this.Min = min;
			this.Max = max;
		}

#if UNITY_EDITOR
		[UnityEditor.CustomPropertyDrawer(typeof(IntSliderAttribute))]
		class Drawer : UnityEditor.PropertyDrawer
		{
			public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
			{
				IntSliderAttribute sliderAttribute = (IntSliderAttribute)attribute;
				if (sliderAttribute == null)
					return;
				//int型用スライダー
				UnityEditor.EditorGUI.IntSlider(position, property, sliderAttribute.Min, sliderAttribute.Max);
			}
		}
#endif
	}
}