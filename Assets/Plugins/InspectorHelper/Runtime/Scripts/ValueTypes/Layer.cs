using System;
using UnityEngine;

namespace Links
{
	[System.Serializable]
	public struct Layer : IEquatable<Layer>
	{
		public int value
		{
			get => m_Value;
			set => m_Value = value;
		}

		[HideInInspector, SerializeField]
		int m_Value;

		public Layer(int layer)
		{
			m_Value = layer;
		}

		public Layer(Layer layer)
			: this(layer.m_Value)
		{
		}

		public static string LayerToName(int layer)
			=> LayerMask.LayerToName(layer);
		public static int NameToLayer(string layerName)
			=> LayerMask.NameToLayer(layerName);

		public override bool Equals(object obj)
			=> obj is Layer layer
			&& Equals(layer);

		public bool Equals(Layer other)
			=> value == other.value
			&& m_Value == other.m_Value;

		public override int GetHashCode()
			=> HashCode.Combine(value, m_Value);

		public static bool operator ==(Layer left, Layer right)
			=> left.Equals(right);

		public static bool operator !=(Layer left, Layer right)
			=> !(left == right);

		//int型とLayer型を変換できるようにする
		public static implicit operator int(Layer layer)
			=> layer.m_Value;
		public static implicit operator Layer(int intVal)
			=> new Layer(intVal);

#if UNITY_EDITOR
		[UnityEditor.CustomPropertyDrawer(typeof(Layer))]
		class Drawer : UnityEditor.PropertyDrawer
		{
			public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
			{
				//int m_ValueにラベルのIndexが入っている
				var valueProperty = property.FindPropertyRelative("m_Value");
				valueProperty.intValue = UnityEditor.EditorGUI.LayerField(position, label, valueProperty.intValue);
			}
		}
#endif
	}
}
