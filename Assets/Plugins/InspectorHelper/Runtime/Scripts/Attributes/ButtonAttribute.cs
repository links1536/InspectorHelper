using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Links
{
	public sealed class ButtonAttribute : PropertyAttribute
	{
		public readonly string MethodName;
		public readonly string ButtonName;
		public readonly object[] Parameters;

		public ButtonAttribute(string methodName, string buttonName = null, params object[] parameters)
		{
			MethodName = methodName;
			ButtonName = buttonName ?? methodName;
			Parameters = parameters;
		}

#if UNITY_EDITOR
		[UnityEditor.CustomPropertyDrawer(typeof(ButtonAttribute))]
		class Drawer : UnityEditor.PropertyDrawer
		{
			public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
			{
				ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;
				if (buttonAttribute == null)
					return;

				if (string.IsNullOrEmpty(buttonAttribute.MethodName)) {
					UnityEditor.EditorGUI.HelpBox(position, "ターゲットメソッドが指定されていません", UnityEditor.MessageType.Error);
					return;
				}

				//プロパティ名のラベルを描画し、ラベル以外のrectを返す
				position = UnityEditor.EditorGUI.PrefixLabel(position, label);

				//ボタンを描画
				if (!GUI.Button(position, buttonAttribute.ButtonName ?? buttonAttribute.MethodName))
					return;
				try {
					BindingFlags bindFlag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

					//Serializableを設定した非SerializedObjectクラス相手でもちゃんと辿る
					object targetObject = property.serializedObject.targetObject;
					var type = targetObject.GetType();
					string propertyPath = property.propertyPath;
					string[] pathList = propertyPath.Split('.');

					//最後から1個上の部分まで参照を辿る
					for (int i = 0; i < pathList.Length - 1; i++) {
						var member = type.GetMember(pathList[i], bindFlag)?[0];
						if (member == null) {
							string currentPath = string.Join(".", pathList.Take(i));
							Debug.LogError(currentPath + "は見つかりませんでした", property.serializedObject.targetObject);
							return;
						}
						//フィールドをたどる
						if (member.MemberType == MemberTypes.Field) {
							var fieldInfo = type.GetField(pathList[i], bindFlag);
							if (fieldInfo == null) {
								return;
							}
							type = fieldInfo.FieldType;
							targetObject = fieldInfo.GetValue(targetObject);
						}
#if false
						//プロパティをたどる
						if (member.MemberType == MemberTypes.Property) {
							var propertyInfo = type.GetProperty(pathList[i], bindFlag);
							if (propertyInfo == null) {
								return;
							}
							type = propertyInfo.PropertyType;
							targetObject = propertyInfo.GetValue(targetObject);
						}
#endif
					}

					//関数呼び出し
					MethodInfo method = type.GetMethod(buttonAttribute.MethodName, bindFlag);
					method.Invoke(targetObject, buttonAttribute.Parameters);
				}
				catch (System.Exception e) {
					Debug.Log(buttonAttribute.MethodName + " を実行できません");
					Debug.LogException(e);
				}
			}
		}
#endif
	}
}
