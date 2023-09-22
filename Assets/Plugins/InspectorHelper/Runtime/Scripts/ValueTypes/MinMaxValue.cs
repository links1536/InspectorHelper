using UnityEngine;

namespace Links
{
	[System.Serializable]
	public struct MinMaxValue
	{
		public float MinValue;
		public float MaxValue;
		[HideInInspector, ReadOnly] public float MinLimit;
		[HideInInspector, ReadOnly] public float MaxLimit;

		public float Range
			=> MaxValue - MinValue;

		public float Evaluate(float t)
			=> Mathf.Clamp(Mathf.LerpUnclamped(MinValue, MaxValue, t), MinLimit, MaxLimit);

		public MinMaxValue(float min, float max, float minLimit, float maxLimit)
		{
			MinValue = min;
			MaxValue = max;
			MinLimit = minLimit;
			MaxLimit = maxLimit;
		}

		public MinMaxValue(MinMaxValue minMaxRange)
			: this(minMaxRange.MinValue, minMaxRange.MaxValue, minMaxRange.MinLimit, minMaxRange.MaxLimit)
		{
		}

#if UNITY_EDITOR
		[UnityEditor.CustomPropertyDrawer(typeof(MinMaxValue))]
		class Drawer : UnityEditor.PropertyDrawer
		{
			const float TextBoxWidth = 70;

			public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
			{
				var propertyMinValue = property.FindPropertyRelative("MinValue");
				var propertyMaxValue = property.FindPropertyRelative("MaxValue");
				var propertyMinLimit = property.FindPropertyRelative("MinLimit");
				var propertyMaxLimit = property.FindPropertyRelative("MaxLimit");

				float minValue = propertyMinValue.floatValue;
				float maxValue = propertyMaxValue.floatValue;
				float minLimit = propertyMinLimit.floatValue;
				float maxLimit = propertyMaxLimit.floatValue;

				//ラベルの幅はインデント分短くなる
				float indentWidth = position.width - UnityEditor.EditorGUI.IndentedRect(position).width;
				UnityEditor.EditorGUIUtility.labelWidth = UnityEditor.EditorGUIUtility.labelWidth - indentWidth;
				Rect contentPosition = UnityEditor.EditorGUI.PrefixLabel(position, label);

				//MinMaxスライダー
				Rect sliderPosition = new Rect(
					contentPosition.x + TextBoxWidth,
					contentPosition.y,
					contentPosition.width - TextBoxWidth - TextBoxWidth,
					contentPosition.height
				);
				UnityEditor.EditorGUI.MinMaxSlider(sliderPosition, ref minValue, ref maxValue, minLimit, maxLimit);

				//最小値
				Rect minPosition = new Rect(contentPosition.x, contentPosition.y, TextBoxWidth, contentPosition.height);
				minValue = UnityEditor.EditorGUI.DelayedFloatField(minPosition, minValue);
				minValue = Mathf.Clamp(minValue, minLimit, maxLimit);

				//最大値
				Rect maxPosition = new Rect(contentPosition.xMax - TextBoxWidth, contentPosition.y, TextBoxWidth, contentPosition.height);
				maxValue = UnityEditor.EditorGUI.DelayedFloatField(maxPosition, maxValue);
				maxValue = Mathf.Clamp(maxValue, minLimit, maxLimit);

				bool changeMin = propertyMinValue.floatValue != minValue;
				bool changeMax = propertyMaxValue.floatValue != maxValue;
				bool change = changeMin || changeMax;
				if (changeMin)
					propertyMinValue.floatValue = minValue;
				if (changeMax)
					propertyMaxValue.floatValue = maxValue;
			}
		}
#endif
	}
}
