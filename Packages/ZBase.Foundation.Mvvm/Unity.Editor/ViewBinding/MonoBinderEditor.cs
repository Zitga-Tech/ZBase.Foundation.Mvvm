using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    [UnityEditor.CustomEditor(typeof(MonoBinder), editorForChildClasses: true)]
    public class MonoBinderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (this.target is not MonoBinder binder)
            {
                base.OnInspectorGUI();
                return;
            }

            var contextProp = this.serializedObject.FindProperty(nameof(MonoBinder._context));

            DrawContextField(this.serializedObject, contextProp, binder);
            DrawFindNearestButton(this.serializedObject, contextProp, binder);

            if (contextProp.objectReferenceValue is not IBindingContext)
            {
                return;
            }

            var serializedContext = new SerializedObject(contextProp.objectReferenceValue);
            var targetKindProp = serializedContext.FindProperty(nameof(MonoContext._targetKind));
            var systemObjectProp = serializedContext.FindProperty(nameof(MonoContext._targetSystemObject));
            var unityObjectProp = serializedContext.FindProperty(nameof(MonoContext._targetUnityObject));

            var target = GetContextTarget(targetKindProp, systemObjectProp, unityObjectProp);
            DrawContextTarget(target);
            DrawBindingFields(this.serializedObject, contextProp, binder, target);
            DrawConverters(this.serializedObject, contextProp, binder);
        }

        private void DrawContextField(
              SerializedObject serializedObject
            , SerializedProperty serializedProperty
            , MonoBinder binder
        )
        {
            EditorGUI.BeginChangeCheck();

            var component = EditorGUILayout.ObjectField(
                new GUIContent("Context"), binder._context, typeof(Component), true
            ) as Component;

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(binder, "Set binder._context");
                serializedProperty.objectReferenceValue = TryEnsureComponent(component);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        }

        private void DrawFindNearestButton(
              SerializedObject serializedObject
            , SerializedProperty serializedProperty
            , MonoBinder binder
        )
        {
            if (serializedProperty.objectReferenceValue is IBindingContext)
            {
                return;
            }

            if (serializedProperty.objectReferenceValue)
            {
                EditorGUILayout.HelpBox(
                    $"The component {serializedProperty.objectReferenceValue.GetType()} does not implement {typeof(IBindingContext)}."
                    , MessageType.Error
                );
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Find Nearest Context") == false)
            {
                return;
            }

            var component = FindNearestContext(binder.gameObject);

            Undo.RecordObject(binder, "Find binder._context");
            serializedProperty.objectReferenceValue = component;
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private static IObservableObject GetContextTarget(
              SerializedProperty targetKindProp
            , SerializedProperty systemObjectProp
            , SerializedProperty unityObjectProp
        )
        {
            var targetKind = (MonoContext.ContextTargetKind)targetKindProp.enumValueIndex;

            return targetKind switch {
                MonoContext.ContextTargetKind.SystemObject => systemObjectProp.managedReferenceValue as IObservableObject,
                MonoContext.ContextTargetKind.UnityObject => unityObjectProp.objectReferenceValue as IObservableObject,
                _ => null,
            };
        }

        private static void DrawContextTarget(IObservableObject target)
        {
            if (target is null)
            {
                EditorGUILayout.HelpBox(
                    "The context target is currently undefined. Binding mechanism will not work."
                    , MessageType.Error
                );

                return;
            }

            EditorGUILayout.HelpBox(
                $"This binder can now listen and react to Property Changed events published by {target.GetType()}."
                , MessageType.Info
            );
        }

        private static void DrawBindingFields(
              SerializedObject serializedBinder
            , SerializedProperty contextProp
            , MonoBinder binder
            , IObservableObject target
        )
        {
            if (contextProp.objectReferenceValue is not IBindingContext)
            {
                return;
            }

            var type = binder.GetType();
            var fieldInfos = TypeCache.GetFieldsWithAttribute<GeneratedBindingFieldAttribute>()
                .Where(x => x.DeclaringType == type);

            EditorGUILayout.LabelField("Binding Fields", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.HelpBox(
                $"Select property on {target.GetType().Name} to bind to each binding field."
                , MessageType.None
            );

            EditorGUILayout.Space(3f);

            foreach (var fieldInfo in fieldInfos)
            {
                DrawBindingField(serializedBinder, binder, target, fieldInfo);
            }

            EditorGUILayout.EndVertical();
        }

        private static void DrawBindingField(
              SerializedObject serializedBinder
            , MonoBinder binder
            , IObservableObject target
            , FieldInfo fieldInfo
        )
        {
            var binderProp = serializedBinder.FindProperty(fieldInfo.Name);
            var propertyNameProp = binderProp.FindPropertyRelative("<PropertyName>k__BackingField");
            var propertyName = string.IsNullOrWhiteSpace(propertyNameProp.stringValue)
                ? "< undefined >"
                : propertyNameProp.stringValue;

            var attrib = fieldInfo.GetCustomAttribute<FieldLabelAttribute>();
            var label = attrib != null ? attrib.Value : ObjectNames.NicifyVariableName(fieldInfo.Name);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel(label);

            if (GUILayout.Button(propertyName))
            {
                DrawBindingFieldMenu(serializedBinder, binder, propertyNameProp, target, propertyName);
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void DrawBindingFieldMenu(
              SerializedObject serializedBinder
            , MonoBinder binder
            , SerializedProperty propertyNameProp
            , IObservableObject target
            , string propertyName
        )
        {
            var type = target.GetType();
            var isEmpty = true;
            var attributes = type.GetCustomAttributes<NotifyPropertyChangedInfoAttribute>();
            var menu = new GenericMenu();

            foreach (var attribute in attributes)
            {
                menu.AddItem(
                      new GUIContent($"{attribute.PropertyName} : {attribute.PropertyType.Name}")
                    , propertyName == attribute.PropertyName
                    , x => SetBindingFieldPropertyName(serializedBinder, binder, propertyNameProp, (string)x)
                    , attribute.PropertyName
                );

                isEmpty = false;
            }

            if (isEmpty)
            {
                menu.AddDisabledItem(new GUIContent($"{type.FullName} contains no [ObservableProperty]"));
            }

            menu.ShowAsContext();
        }

        private static void SetBindingFieldPropertyName(
              SerializedObject serializedBinder
            , MonoBinder binder
            , SerializedProperty propertyNameProp
            , string propertyName
        )
        {
            if (propertyNameProp.stringValue != propertyName)
            {
                Undo.RecordObject(binder, $"Set {propertyNameProp.propertyPath}");
                propertyNameProp.stringValue = propertyName;
                serializedBinder.ApplyModifiedProperties();
                serializedBinder.Update();
            }
        }

        private static void DrawConverters(
              SerializedObject serializedBinder
            , SerializedProperty contextProp
            , MonoBinder binder
        )
        {
            if (contextProp.objectReferenceValue is not IBindingContext)
            {
                return;
            }

            var type = binder.GetType();
            var fieldInfos = TypeCache.GetFieldsWithAttribute<GeneratedConverterAttribute>()
                .Where(x => x.DeclaringType == type);

            EditorGUILayout.LabelField("Converters", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.HelpBox(
                $"Select an IAdapter to convert the data transferred to and from each binding field."
                , MessageType.None
            );

            foreach (var fieldInfo in fieldInfos)
            {
                DrawConverter(serializedBinder, binder, fieldInfo);
            }

            EditorGUILayout.EndVertical();
        }

        private static void DrawConverter(
              SerializedObject serializedBinder
            , MonoBinder binder
            , FieldInfo fieldInfo
        )
        {
            var converterProp = serializedBinder.FindProperty(fieldInfo.Name);
            var adapterProp = converterProp.FindPropertyRelative("<Adapter>k__BackingField");

            string adapterName;
            Type adapterType;

            if (adapterProp.managedReferenceValue is IAdapter adapter)
            {
                adapterType = adapter.GetType();
                adapterName = adapterType.FullName;
            }
            else
            {
                adapterType = null;
                adapterName = "< undefined >";
            }

            var attrib = fieldInfo.GetCustomAttribute<FieldLabelAttribute>();
            var label = attrib != null ? attrib.Value : ObjectNames.NicifyVariableName(fieldInfo.Name);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel(label);

            if (GUILayout.Button(adapterName))
            {
                DrawAdapterMenu(serializedBinder, binder, adapterProp, adapterType, adapterName);
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void DrawAdapterMenu(
              SerializedObject serializedBinder
            , MonoBinder binder
            , SerializedProperty adapterProp
            , Type adapterType
            , string adapterName
        )
        {
            var types = TypeCache.GetTypesDerivedFrom<IAdapter>()
                .Where(x => x.IsSubclassOf(typeof(UnityEngine.Object)) == false);

            var menu = new GenericMenu();
            var isEmpty = true;

            foreach (var type in types)
            {
                menu.AddItem(
                      new GUIContent($"{type.Namespace}/{type.Name}")
                    , type.FullName == adapterName
                    , x => SetAdapter(serializedBinder, binder, adapterProp, adapterType, (Type)x)
                    , type
                );

                isEmpty = false;
            }

            if (isEmpty)
            {
                menu.AddDisabledItem(new GUIContent($"No type implements {typeof(IAdapter)}"));
            }

            menu.ShowAsContext();
        }

        private static void SetAdapter(
              SerializedObject serializedBinder
            , MonoBinder binder
            , SerializedProperty adapterProp
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
                Undo.RecordObject(binder, $"Set {adapterProp.propertyPath}");
                adapterProp.managedReferenceValue = instance;
                serializedBinder.ApplyModifiedProperties();
                serializedBinder.Update();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, binder);
            }
        }

        private Component TryEnsureComponent(Component input)
        {
            if (input == false)
            {
                return input;
            }

            var components = input.gameObject.GetComponents<IBindingContext>();

            if (components == null || components.Length < 1)
                return input;

            return components[0] as Component;
        }

        private Component FindNearestContext(GameObject go)
        {
            var parent = go.transform;
            var components = new List<IBindingContext>();

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
    }
}