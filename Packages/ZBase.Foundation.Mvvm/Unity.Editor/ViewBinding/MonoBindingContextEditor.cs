#pragma warning disable CA1031 // Do not catch general exception types

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.ComponentModel.SourceGen;

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
            var targetPropertyPath = this.serializedObject.FindProperty(nameof(MonoBindingContext._targetPropertyPath));
            var createOnAwake = this.serializedObject.FindProperty(nameof(MonoBindingContext._createOnAwake));

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
                    DrawTargetPropertyPath(
                          this.serializedObject
                        , targetPropertyPath
                        , context._targetSystemObject
                    );
                    DrawCreateOnAwake(createOnAwake, context);
                    break;
                }

                case MonoBindingContext.ContextTargetKind.UnityObject:
                {
                    DrawTargetUnityObject(this.serializedObject, unityObjectProp, context);
                    DrawTargetButtons(this.serializedObject, systemObjectProp, unityObjectProp, context);
                    DrawTargetPropertyPath(
                          this.serializedObject
                        , targetPropertyPath
                        , context._targetUnityObject as IObservableObject
                    );
                    DrawCreateOnAwake(createOnAwake, context);
                    break;
                }
            }
        }

        private static void DrawTargetButtons(
              SerializedObject serializedObject
            , SerializedProperty systemObjectProp
            , SerializedProperty unityObjectProp
            , MonoBindingContext context
        )
        {
            EditorGUILayout.BeginHorizontal();

            if (context._targetKind == MonoBindingContext.ContextTargetKind.UnityObject
                && GUILayout.Button("Find Nearest Target")
            )
            {
                var component = FindNearestContext(context.gameObject);

                Undo.RecordObject(context, "Find context._targetUnityObject");
                unityObjectProp.objectReferenceValue = component;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            if (GUILayout.Button("Clear Target"))
            {
                Undo.RecordObject(context, "Clear context._targets");
                systemObjectProp.managedReferenceValue = null;
                unityObjectProp.objectReferenceValue = null;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            EditorGUILayout.EndHorizontal();
        }

        private static Component FindNearestContext(GameObject go)
        {
            var parent = go.transform;
            var components = new List<IObservableObject>();

            while (parent)
            {
                components.Clear();
                parent.GetComponents(components);

                if (components.Count > 0)
                {
                    return components[0] as Component;
                }

                parent = parent.parent;
            }

            return null;
        }

        private static void DrawCreateOnAwake(
              SerializedProperty createOnAwakeSP
            , MonoBindingContext context
        )
        {
            EditorGUI.BeginChangeCheck();

            var value = EditorGUILayout.Toggle("Create On Awake", context._createOnAwake);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(context, "Set context._createOnAwake");
                createOnAwakeSP.boolValue = value;
                createOnAwakeSP.serializedObject.ApplyModifiedProperties();
                createOnAwakeSP.serializedObject.Update();
            }

            if (value == false)
            {
                EditorGUILayout.HelpBox(
                      "The target of this binding context will NOT be automatically created on `MonoBehaviour.Awake()`.\n" +
                      "At runtime the `MonoBindingContext.InitializeManually(target)` method must be used " +
                      "to provide the appropriate target."
                    , MessageType.Warning
                );
            }
        }

        private static void DrawTargetPropertyPath(
              SerializedObject serializedObject
            , SerializedProperty targetPropertyPathSP
            , IObservableObject rootTarget
        )
        {
            if (rootTarget == null)
            {
                return;
            }

            var rootType = rootTarget.GetType();
            var fields = TypeCache.GetFieldsWithAttribute<IsObservableObjectAttribute>()
                .Where(x => x.IsPublic && x.IsStatic
                    && x.IsLiteral && x.IsInitOnly == false
                    && x.DeclaringType == rootType
                );

            if (fields.Any() == false)
            {
                return;
            }

            var rol = new ReorderableList(serializedObject, targetPropertyPathSP, true, true, true, true) {
                elementHeight = 25f,
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Target Property Path"),
                onAddDropdownCallback = (rect, list) => OnAddDropdown(rect, list, rootTarget),
                onRemoveCallback = OnRemove,
            };

            rol.drawElementCallback = (rect, index, isActive, isFocused)
                => DrawElement(rect, index, isActive, isFocused, rol);

            EditorGUILayout.Space();

            rol.DoLayoutList();

            static void OnAddDropdown(Rect rect, ReorderableList rol, IObservableObject rootTarget)
            {
                var type = rootTarget.GetType();
                var serializedProperty = rol.serializedProperty;
                var length = serializedProperty.arraySize;

                for (var i = 0; i < length; i++)
                {
                    var propertyName = serializedProperty.GetArrayElementAtIndex(i).stringValue;

                    if (string.IsNullOrWhiteSpace(propertyName))
                    {
                        break;
                    }

                    var attribs = TypeCache.GetFieldsWithAttribute<IsObservableObjectAttribute>()
                        .Where(x => x.IsPublic && x.IsStatic
                            && x.IsLiteral && x.IsInitOnly == false
                            && x.DeclaringType == type
                            && string.Equals(x.GetValue(null), propertyName)
                        )
                        .Select(x => x.GetCustomAttribute<IsObservableObjectAttribute>())
                        .Where(x => x is { });

                    type = attribs.FirstOrDefault()?.Type;

                    if (type == null)
                    {
                        break;
                    }
                }

                if (type == null)
                {
                    EditorUtility.DisplayDialog(
                          "Cannot Find More IObservableObject"
                        , "There is no property that is IObservableObject inside the last target in the property path."
                        , "I understand"
                    );

                    return;
                }

                var fields = TypeCache.GetFieldsWithAttribute<IsObservableObjectAttribute>()
                    .Where(x => x.IsPublic && x.IsStatic
                        && x.IsLiteral && x.IsInitOnly == false
                        && x.DeclaringType == type
                    );

                var menu = new GenericMenu();
                
                foreach (var field in fields)
                {
                    var value = field.GetValue(null);

                    if (value is not string propertyName)
                    {
                        continue;
                    }

                    var label = new GUIContent(propertyName);

                    menu.AddItem(label, false, AddPropertyPath, (rol, propertyName));
                }

                menu.ShowAsContext();
            }

            static void AddPropertyPath(object param)
            {
                if (param is not (ReorderableList rol, string propertyPath))
                {
                    return;
                }

                var serializedObject = rol.serializedProperty.serializedObject;
                var target = serializedObject.targetObject;

                try
                {
                    var index = rol.serializedProperty.arraySize;

                    Undo.RecordObject(target, $"Set {rol.serializedProperty.propertyPath}.Array.data[{index}]");

                    rol.serializedProperty.arraySize++;
                    rol.index = index;

                    var elementSP = rol.serializedProperty.GetArrayElementAtIndex(index);
                    elementSP.stringValue = propertyPath;
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex, target);
                }
            }

            static void OnRemove(ReorderableList rol)
            {
                var serializedObject = rol.serializedProperty.serializedObject;
                var target = serializedObject.targetObject;

                Undo.RecordObject(target, $"Remove {rol.selectedIndices.Count} items at {rol.serializedProperty.propertyPath}");

                var index = rol.serializedProperty.arraySize - 1;
                rol.serializedProperty.DeleteArrayElementAtIndex(index);

                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            static void DrawElement(
                  Rect rect
                , int index
                , bool isActive
                , bool isFocused
                , ReorderableList rol
            )
            {
                var elementSP = rol.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.LabelField(rect, elementSP.stringValue);
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
                    .Where(static x => x.IsAbstract == false && x.IsSubclassOf(typeof(UnityEngine.Object)) == false);

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
                  "Target"
                , context._targetUnityObject
                , typeof(UnityEngine.Object)
                , true
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
