using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    [UnityEditor.CustomEditor(typeof(MonoBindingContext))]
    public sealed class MonoBindingContextEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (this.target is not MonoBindingContext context)
            {
                base.OnInspectorGUI();
                return;
            }

            var targetKindProp = this.serializedObject.FindProperty(nameof(MonoBindingContext._targetKind));
            var systemObjectProp = this.serializedObject.FindProperty(nameof(MonoBindingContext._targetSystemObject));
            var unityObjectProp = this.serializedObject.FindProperty(nameof(MonoBindingContext._targetUnityObject));

            EditorGUI.BeginChangeCheck();

            var targetKind = (MonoBindingContext.ContextTargetKind)EditorGUILayout.EnumPopup(
                "Target Kind", context._targetKind
            );

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(context, "Set context._targetKind");
                targetKindProp.enumValueIndex = (int)targetKind;

                switch (targetKind)
                {
                    case MonoBindingContext.ContextTargetKind.SystemObject:
                    {
                        unityObjectProp.objectReferenceValue = null;
                        break;
                    }

                    case MonoBindingContext.ContextTargetKind.UnityObject:
                    {
                        systemObjectProp.managedReferenceValue = null;
                        break;
                    }
                }

                this.serializedObject.ApplyModifiedProperties();
                this.serializedObject.Update();
            }

            switch (context._targetKind)
            {
                case MonoBindingContext.ContextTargetKind.SystemObject:
                {
                    DrawTargetSystemObject(context, this.serializedObject, systemObjectProp);
                    break;
                }

                case MonoBindingContext.ContextTargetKind.UnityObject:
                {
                    DrawTargetUnityObject(this.serializedObject, unityObjectProp, context);
                    break;
                }
            }
        }

        private static void DrawTargetSystemObject(
              MonoBindingContext context
            , SerializedObject obj
            , SerializedProperty prop
        )
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Target");

            string targetTypeName;
            Type targetType;

            if (context._targetSystemObject == null)
            {
                targetType = null;
                targetTypeName = "< undefined >";
            }
            else
            {
                targetType = context._targetSystemObject.GetType();
                targetTypeName = targetType.FullName;
            }

            if (GUILayout.Button(targetTypeName))
            {
                var types = TypeCache.GetTypesDerivedFrom<IObservableObject>()
                    .Where(x => x.IsAbstract == false && x.IsSubclassOf(typeof(UnityEngine.Object)) == false);

                var menu = new GenericMenu();
                var isEmpty = true;

                foreach (var type in types)
                {
                    menu.AddItem(
                          new GUIContent($"{type.Namespace}/{type.Name}")
                        , type.FullName == targetTypeName
                        , x => SetTargetSystemObject(obj, prop, context, targetType, (Type)x)
                        , type
                    );

                    isEmpty = false;
                }

                if (isEmpty)
                {
                    menu.AddDisabledItem(new GUIContent($"No type implements {typeof(IObservableObject)}"));
                }

                menu.ShowAsContext();
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void SetTargetSystemObject(
              SerializedObject serializedObject
            , SerializedProperty serializedProperty
            , MonoBindingContext context
            , Type currentType
            , Type newType
        )
        {
            if (currentType == newType || newType == null)
            {
                return;
            }

            try
            {
                var instance = Activator.CreateInstance(newType) as IObservableObject;
                Undo.RecordObject(context, "Set context._targetSystemObject");
                serializedProperty.managedReferenceValue = instance;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, context);
            }
        }

        private static void DrawTargetUnityObject(
              SerializedObject serializedObject
            , SerializedProperty serializedProperty
            , MonoBindingContext context
        )
        {
            var target = EditorGUILayout.ObjectField(
                "Target", context._targetUnityObject, typeof(UnityEngine.Object), true
            );

            if (target is not IObservableObject)
            {
                return;
            }

            if (context._targetUnityObject != target)
            {
                Undo.RecordObject(context, "Set context._targetUnityObject");
                serializedProperty.objectReferenceValue = target;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        }
    }
}
