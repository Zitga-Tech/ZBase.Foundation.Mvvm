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
            var targetKindProp = serializedContext.FindProperty(nameof(MonoBindingContext._targetKind));
            var systemObjectProp = serializedContext.FindProperty(nameof(MonoBindingContext._targetSystemObject));
            var unityObjectProp = serializedContext.FindProperty(nameof(MonoBindingContext._targetUnityObject));

            var target = GetContextTarget(targetKindProp, systemObjectProp, unityObjectProp);
            DrawContextTarget(target);
            DrawBindingProperties(this.serializedObject, contextProp, binder, target);
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
            var targetKind = (MonoBindingContext.ContextTargetKind)targetKindProp.enumValueIndex;

            return targetKind switch {
                MonoBindingContext.ContextTargetKind.SystemObject => systemObjectProp.managedReferenceValue as IObservableObject,
                MonoBindingContext.ContextTargetKind.UnityObject => unityObjectProp.objectReferenceValue as IObservableObject,
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

        private static void DrawBindingProperties(
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

            var binderType = binder.GetType();
            var fieldInfos = TypeCache.GetFieldsWithAttribute<GeneratedBindingPropertyAttribute>()
                .Where(x => x.DeclaringType == binderType);

            var targetType = target.GetType();
            var targetAttributes = targetType.GetCustomAttributes<NotifyPropertyChangedInfoAttribute>();

            EditorGUILayout.LabelField("Binding Properties", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.HelpBox(
                $"Select a target property on {target.GetType().Name} to bind."
                , MessageType.None
            );

            EditorGUILayout.Space(3f);

            foreach (var fieldInfo in fieldInfos)
            {
                DrawBindingProperty(serializedBinder, binder, targetType, targetAttributes, fieldInfo);
            }

            EditorGUILayout.EndVertical();
        }

        private static void DrawBindingProperty(
              SerializedObject serializedBinder
            , MonoBinder binder
            , Type targetType
            , IEnumerable<NotifyPropertyChangedInfoAttribute> targetAttributes
            , FieldInfo fieldInfo
        )
        {
            var binderProp = serializedBinder.FindProperty(fieldInfo.Name);
            var propertyNameProp = binderProp.FindPropertyRelative("<TargetPropertyName>k__BackingField");

            string propertyName;
            GUIContent propertyLabel;

            if (string.IsNullOrWhiteSpace(propertyNameProp.stringValue))
            {
                propertyLabel = new GUIContent("< undefined >");
                propertyName = "";
            }
            else
            {
                var candidate = propertyNameProp.stringValue;
                var propAttrib = targetAttributes.FirstOrDefault(x => x.PropertyName == candidate);

                if (propAttrib == null)
                {
                    propertyLabel = new GUIContent(
                          $"< invalid > {candidate}"
                        , $"{targetType.FullName} does not contain a property named {candidate}"
                    );

                    propertyName = "";
                }
                else
                {
                    propertyLabel = new GUIContent(
                          $"{propAttrib.PropertyName} : {propAttrib.PropertyType.GetName()}"
                        , $"class {targetType.Name}\nin {targetType.Namespace}"
                    );
                    propertyName = propAttrib.PropertyName;
                }
            }

            var attrib = fieldInfo.GetCustomAttribute<LabelAttribute>();
            var label = attrib != null ? attrib.Value : ObjectNames.NicifyVariableName(fieldInfo.Name);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel(label);

            if (GUILayout.Button(propertyLabel))
            {
                DrawBindingPropertyMenu(serializedBinder, binder, propertyNameProp, targetType, targetAttributes, propertyName);
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void DrawBindingPropertyMenu(
              SerializedObject serializedBinder
            , MonoBinder binder
            , SerializedProperty propertyNameProp
            , Type targetType
            , IEnumerable<NotifyPropertyChangedInfoAttribute> targetAttributes
            , string propertyName
        )
        {
            var menu = new GenericMenu();
            var isEmpty = true;

            foreach (var attribute in targetAttributes)
            {
                menu.AddItem(
                      new GUIContent($"{attribute.PropertyName} : {attribute.PropertyType.GetName()}")
                    , propertyName == attribute.PropertyName
                    , x => SetBindingPropertyName(serializedBinder, binder, propertyNameProp, (string)x)
                    , attribute.PropertyName
                );

                isEmpty = false;
            }

            if (isEmpty)
            {
                menu.AddDisabledItem(new GUIContent($"{targetType.FullName} contains no [ObservableProperty]"));
            }

            menu.ShowAsContext();
        }

        private static void SetBindingPropertyName(
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

            var binderType = binder.GetType();
            var fieldInfos = TypeCache.GetFieldsWithAttribute<GeneratedConverterAttribute>()
                .Where(x => x.DeclaringType == binderType);

            var adapterTypes = TypeCache.GetTypesDerivedFrom<IAdapter>()
                .Where(x => x.IsAbstract == false && x.IsSubclassOf(typeof(UnityEngine.Object)) == false);

            var adapterTypesIsEmpty = adapterTypes.Count() == 0;

            EditorGUILayout.LabelField("Converters", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.HelpBox(
                $"Select an IAdapter to convert the data transferred from the source property."
                , MessageType.None
            );

            foreach (var fieldInfo in fieldInfos)
            {
                DrawConverter(serializedBinder, binder, fieldInfo, adapterTypes, adapterTypesIsEmpty);
            }

            EditorGUILayout.EndVertical();
        }

        private static void DrawConverter(
              SerializedObject serializedBinder
            , MonoBinder binder
            , FieldInfo fieldInfo
            , IEnumerable<Type> adapterTypes
            , bool adapterTypesIsEmpty
        )
        {
            var converterProp = serializedBinder.FindProperty(fieldInfo.Name);
            var adapterProp = converterProp.FindPropertyRelative("<Adapter>k__BackingField");

            GUIContent adapterLabel;
            string adapterFullName;
            Type adapterType;

            if (adapterProp.managedReferenceValue is IAdapter adapter)
            {
                adapterType = adapter.GetType();
                var keyword = adapterType.IsValueType ? "struct" : "class";
                var labelAttrib = adapterType.GetCustomAttribute<LabelAttribute>();
                
                adapterLabel = new GUIContent(
                      labelAttrib?.Value ?? adapterType.Name
                    , $"{keyword} {adapterType.Name}\nin {adapterType.Namespace}"
                );

                adapterFullName = adapterType.FullName;
            }
            else
            {
                adapterType = null;
                adapterLabel = new GUIContent("< undefined >");
                adapterFullName = "";
            }

            var attrib = fieldInfo.GetCustomAttribute<LabelAttribute>();
            var label = attrib != null ? attrib.Value : ObjectNames.NicifyVariableName(fieldInfo.Name);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel(label);

            if (GUILayout.Button(adapterLabel))
            {
                DrawAdapterMenu(serializedBinder, binder, adapterProp, adapterTypes, adapterTypesIsEmpty, adapterType, adapterFullName);
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void DrawAdapterMenu(
              SerializedObject serializedBinder
            , MonoBinder binder
            , SerializedProperty adapterProp
            , IEnumerable<Type> adapterTypes
            , bool adapterTypesIsEmpty
            , Type adapterType
            , string adapterFullName
        )
        {
            var menu = new GenericMenu();

            if (adapterTypesIsEmpty == false)
            {
                menu.AddItem(
                      new GUIContent("None")
                    , string.IsNullOrWhiteSpace(adapterFullName)
                    , () => RemoveAdapter(serializedBinder, binder, adapterProp)
                );
            }

            foreach (var type in adapterTypes)
            {
                var labelAttrib = type.GetCustomAttribute<LabelAttribute>();
                var labelText = labelAttrib?.Value ?? type.Name;
                var keyword = type.IsValueType ? "struct" : "class";
                var label = new GUIContent(
                      $"{type.Namespace}/{labelText}"
                    , $"{keyword} {type.Name}\nin {type.Namespace}"
                );

                menu.AddItem(
                      label
                    , type.FullName == adapterFullName
                    , x => SetAdapter(serializedBinder, binder, adapterProp, adapterType, (Type)x)
                    , type
                );
            }

            if (adapterTypesIsEmpty)
            {
                menu.AddDisabledItem(new GUIContent($"No type implements {typeof(IAdapter)}"));
            }

            menu.ShowAsContext();
        }

        private static void RemoveAdapter(
              SerializedObject serializedBinder
            , MonoBinder binder
            , SerializedProperty adapterProp
        )
        {
            Undo.RecordObject(binder, $"Remove {adapterProp.propertyPath}");
            adapterProp.managedReferenceValue = null;
            serializedBinder.ApplyModifiedProperties();
            serializedBinder.Update();
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
                var instance = Activator.CreateInstance(newType) as IAdapter;
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