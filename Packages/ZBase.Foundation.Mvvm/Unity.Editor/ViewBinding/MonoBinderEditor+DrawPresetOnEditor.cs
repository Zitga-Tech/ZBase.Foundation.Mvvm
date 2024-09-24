#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable CA2000 // Dispose objects before losing scope

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;
using ZBase.Foundation.Mvvm.ViewBinding.Adapters;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Adapters;
using ZBase.Foundation.Mvvm.ViewBinding.SourceGen;
using ZBase.Foundation.Mvvm.ComponentModel.SourceGen;
using ZBase.Foundation.Mvvm.Input;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    partial class MonoBinderEditor
    {
        private void DrawPresetOnEditor(MonoBinder binder)
        {
            var contextProp = this.serializedObject.FindProperty(nameof(MonoBinder._context));

            DrawContextField(this.serializedObject, contextProp, binder);
            DrawFindNearestContext(this.serializedObject, contextProp, binder);

            if (contextProp.objectReferenceValue is not IBindingContext)
            {
                return;
            }

            var serializedContext = new SerializedObject(contextProp.objectReferenceValue);
            var contextTargetType = GetContextTargetType(serializedContext);

            if (DrawContextTarget(contextProp.objectReferenceValue, contextTargetType) == false)
            {
                return;
            }

            var binderType = binder.GetType();
            var bindingPropertyAttributes = binderType.GetCustomAttributes<BindingPropertyMethodInfoAttribute>();
            var bindingPropertyMap = new Dictionary<string, Type>();

            foreach (var attribute in bindingPropertyAttributes)
            {
                if (bindingPropertyMap.ContainsKey(attribute.MethodName) == false)
                {
                    bindingPropertyMap[attribute.MethodName] = attribute.ParameterType;
                }
            }

            var bindingCommandAttributes = binderType.GetCustomAttributes<BindingCommandMethodInfoAttribute>();
            var bindingCommandMap = new Dictionary<string, Type>();

            foreach (var attribute in bindingCommandAttributes)
            {
                if (bindingCommandMap.ContainsKey(attribute.MethodName) == false)
                {
                    bindingCommandMap[attribute.MethodName] = attribute.ParameterType;
                }
            }

            var targetNPCIAttributes = contextTargetType.GetCustomAttributes<NotifyPropertyChangedInfoAttribute>();
            var targetPropertyMap = new Dictionary<string, Type>();

            foreach (var attribute in targetNPCIAttributes)
            {
                if (targetPropertyMap.ContainsKey(attribute.PropertyName) == false)
                {
                    targetPropertyMap[attribute.PropertyName] = attribute.PropertyType;
                }
            }

            var targetRCIAttributes = contextTargetType.GetCustomAttributes<RelayCommandInfoAttribute>();
            var targetCommandMap = new Dictionary<string, Type>();

            foreach (var attribute in targetRCIAttributes)
            {
                if (targetCommandMap.ContainsKey(attribute.CommandName) == false)
                {
                    targetCommandMap[attribute.CommandName] = attribute.ParameterType;
                }
            }

            var bindingPropertyLabelMap = new Dictionary<string, string>();
            var bindingPropertyFieldInfos = TypeCache.GetFieldsWithAttribute<GeneratedBindingPropertyAttribute>()
                .Where(x => x.DeclaringType == binderType);

            foreach (var fieldInfo in bindingPropertyFieldInfos)
            {
                var propAttrib = fieldInfo.GetCustomAttribute<GeneratedBindingPropertyAttribute>();
                var labelAttrib = fieldInfo.GetCustomAttribute<LabelAttribute>();

                if (bindingPropertyLabelMap.ContainsKey(propAttrib.MethodName) == false)
                {
                    var name = ObjectNames.NicifyVariableName(propAttrib.MethodName);
                    bindingPropertyLabelMap[propAttrib.MethodName] = labelAttrib?.Label ?? name;
                }
            }

            var bindingCommandLabelMap = new Dictionary<string, string>();
            var bindingCommandFieldInfos = TypeCache.GetFieldsWithAttribute<GeneratedBindingCommandAttribute>()
                .Where(x => x.DeclaringType == binderType);

            foreach (var fieldInfo in bindingCommandFieldInfos)
            {
                var propAttrib = fieldInfo.GetCustomAttribute<GeneratedBindingCommandAttribute>();
                var labelAttrib = fieldInfo.GetCustomAttribute<LabelAttribute>();

                if (bindingCommandLabelMap.ContainsKey(propAttrib.MethodName) == false)
                {
                    var name = ObjectNames.NicifyVariableName(propAttrib.MethodName);
                    bindingCommandLabelMap[propAttrib.MethodName] = labelAttrib?.Label ?? name;
                }
            }

            var serializedBinder = this.serializedObject;

            foreach (var (bindingName, bindingType) in bindingPropertyMap)
            {
                DrawBindingProperty(
                      binder
                    , serializedBinder
                    , binderType
                    , bindingName
                    , bindingType
                    , contextTargetType
                    , targetPropertyMap
                    , bindingPropertyLabelMap
                    , _rolMap
                );
            }

            foreach (var (bindingName, bindingType) in bindingCommandMap)
            {
                DrawBindingCommand(
                      binder
                    , serializedBinder
                    , binderType
                    , bindingName
                    , bindingType
                    , contextTargetType
                    , targetCommandMap
                    , bindingCommandLabelMap
                );
            }
        }

        private static void DrawContextField(
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

        private static void DrawFindNearestContext(
              SerializedObject serializedObject
            , SerializedProperty serializedProperty
            , MonoBinder binder
        )
        {
            var objectRef = serializedProperty.objectReferenceValue;

            if (objectRef && objectRef is not IBindingContext)
            {
                EditorGUILayout.HelpBox(
                    $"The component {objectRef.GetType()} does not implement {typeof(IBindingContext)}."
                    , MessageType.Error
                );
            }
            else if (objectRef == false)
            {
                EditorGUILayout.HelpBox(
                    "Preset On Editor requires the Context to be not null."
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

        private static Type GetContextTargetType(SerializedObject serializedContext)
        {
            var targetKindProp = serializedContext.FindProperty(nameof(MonoBindingContext._targetKind));
            var systemObjectProp = serializedContext.FindProperty(nameof(MonoBindingContext._targetSystemObject));
            var unityObjectProp = serializedContext.FindProperty(nameof(MonoBindingContext._targetUnityObject));
            var propertyPath = serializedContext.FindProperty(nameof(MonoBindingContext._targetPropertyPath));
            var targetKind = (MonoBindingContext.ContextTargetKind)targetKindProp.enumValueIndex;

            var type = targetKind switch {
                MonoBindingContext.ContextTargetKind.SystemObject => systemObjectProp.managedReferenceValue?.GetType(),
                MonoBindingContext.ContextTargetKind.UnityObject => GetType(unityObjectProp.objectReferenceValue),
                _ => null,
            };

            if (type != null && propertyPath.arraySize > 0)
            {
                for (var i = 0; i < propertyPath.arraySize; i++)
                {
                    var propertyName = propertyPath.GetArrayElementAtIndex(i).stringValue;
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
                        return null;
                    }
                }
            }

            return type;

            static Type GetType(UnityEngine.Object objectRef)
            {
                return objectRef ? objectRef.GetType() : null;
            }
        }

        private static bool DrawContextTarget(UnityEngine.Object obj, Type targetType)
        {
            if (targetType is null)
            {
                EditorGUILayout.HelpBox(
                    "The context target is currently undefined. Binding mechanism will not work."
                    , MessageType.Error
                );

                if (obj)
                {
                    var color = GUI.color;
                    GUI.color = Color.yellow;

                    if (GUILayout.Button("Find context to fix"))
                    {
                        Selection.activeObject = obj;
                    }

                    GUI.color = color;
                }

                return false;
            }

            EditorGUILayout.HelpBox(
                $"This binder can now listen and react to Property Changed events published by {targetType}."
                , MessageType.Info
            );

            return true;
        }

        private static void DrawBindingProperty(
              MonoBinder binder
            , SerializedObject serializedBinder
            , Type binderType
            , string bindingName
            , Type bindingType
            , Type targetType
            , Dictionary<string, Type> targetPropertyMap
            , Dictionary<string, string> labelMap
            , Dictionary<string, ReorderableList> rolMap
        )
        {
            var targetPropertyNameSPPath = $"_bindingFieldFor{bindingName}.<TargetPropertyName>k__BackingField";
            var adapterSPPath = $"_converterFor{bindingName}.<Adapter>k__BackingField";

            var targetPropertyNameSP = serializedBinder.FindProperty(targetPropertyNameSPPath);
            var adapterPropertySP = serializedBinder.FindProperty(adapterSPPath);

            GetTargetPropertyData(
                  targetType
                , targetPropertyMap
                , targetPropertyNameSP
                , out var targetPropertyName
                , out var targetPropertyType
                , out var targetPropertyLabel
            );

            GetAdapterPropertyData(
                  adapterPropertySP
                , out var adapterType
                , out var adapterPropertyLabel
                , out var scriptableAdapter
                , out var compositeAdapter
            );

            if (labelMap.TryGetValue(bindingName, out var bindingLabelText) == false)
            {
                bindingLabelText = ObjectNames.NicifyVariableName(bindingName);
            }

            var bindingTypeName = bindingType.GetFriendlyName();
            var bindingTypeLabel = new GUIContent(bindingTypeName, bindingType.GetFullName());
            var bindingLabel = new GUIContent(
                  bindingLabelText
                , $"{binderType.Name}.{bindingName} ( {bindingTypeName} )"
            );

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                var color = GUI.color;
                GUI.color = Color.green;
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.LabelField(bindingLabel, bindingTypeLabel);
                EditorGUILayout.EndHorizontal();
                GUI.color = color;

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel("Bind To Property");

                    if (GUILayout.Button(targetPropertyLabel))
                    {
                        DrawBindingPropertyMenu(
                              binder
                            , serializedBinder
                            , targetPropertyNameSP
                            , adapterPropertySP
                            , bindingType
                            , targetType
                            , targetPropertyName
                            , targetPropertyMap
                        );
                    }

                }
                EditorGUILayout.EndHorizontal();

                if (adapterPropertySP != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.PrefixLabel("Convert By");

                        if (GUILayout.Button(adapterPropertyLabel))
                        {
                            DrawAdapterPropertyMenu(
                                  binder
                                , serializedBinder
                                , bindingType
                                , targetPropertyType
                                , adapterPropertySP
                                , adapterType
                            );
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    DrawScriptableAdapter(
                          serializedBinder
                        , adapterPropertySP
                        , scriptableAdapter
                    );

                    DrawCompositeAdapter(
                          serializedBinder
                        , adapterPropertySP
                        , compositeAdapter
                        , rolMap
                    );
                }

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        private static void GetTargetPropertyData(
              Type targetType
            , Dictionary<string, Type> targetPropertyMap
            , SerializedProperty targetPropertyNameSP
            , out string targetPropertyName
            , out Type targetPropertyType
            , out GUIContent targetPropertyLabel
        )
        {
            if (string.IsNullOrWhiteSpace(targetPropertyNameSP.stringValue))
            {
                targetPropertyName = "";
                targetPropertyType = null;
                targetPropertyLabel = new GUIContent("< undefined >");
            }
            else
            {
                var candidate = targetPropertyNameSP.stringValue;

                if (targetPropertyMap.TryGetValue(candidate, out var propertyType) == false)
                {
                    targetPropertyName = "";
                    targetPropertyType = null;
                    targetPropertyLabel = new GUIContent(
                          $"< invalid > {candidate}"
                        , $"{targetType.FullName} does not contain a property named {candidate}"
                    );
                }
                else
                {
                    var returnTypeName = propertyType.GetFriendlyName();
                    targetPropertyName = candidate;
                    targetPropertyType = propertyType;
                    targetPropertyLabel = new GUIContent(
                          $"{candidate} : {returnTypeName}"
                        , $"property {candidate} : {returnTypeName}\nclass {targetType.Name}\nnamespace {targetType.Namespace}"
                    );
                }
            }
        }

        private static void GetAdapterPropertyData(
              SerializedProperty adapterPropertySP
            , out Type adapterType
            , out GUIContent adapterPropertyLabel
            , out ScriptableAdapter scriptableAdapter
            , out CompositeAdapter compositeAdapter
        )
        {
            if (adapterPropertySP?.managedReferenceValue is IAdapter adapter)
            {
                adapterType = adapter.GetType();

                var keyword = adapterType.IsValueType ? "struct" : "class";
                var adapterLabelAttrib = adapterType.GetCustomAttribute<LabelAttribute>();

                adapterPropertyLabel = new GUIContent(
                      adapterLabelAttrib?.Label ?? adapterType.Name
                    , $"{keyword} {adapterType.Name}\nnamespace {adapterType.Namespace}"
                );

                scriptableAdapter = adapter as ScriptableAdapter;
                compositeAdapter = adapter as CompositeAdapter;
            }
            else
            {
                adapterType = null;
                adapterPropertyLabel = new GUIContent("< undefined >");
                scriptableAdapter = null;
                compositeAdapter = null;
            }
        }

        private static void DrawBindingPropertyMenu(
              MonoBinder binder
            , SerializedObject serializedBinder
            , SerializedProperty targetPropertyNameSP
            , SerializedProperty adapterPropertySP
            , Type bindingType
            , Type targetType
            , string targetPropertyName
            , Dictionary<string, Type> targetPropertyMap
        )
        {
            var menu = new GenericMenu();

            menu.AddItem(
                  new GUIContent("<None>")
                , false
                , RemoveBindingProperty
                , (binder, serializedBinder, targetPropertyNameSP, adapterPropertySP)
            );

            foreach (var (propName, propType) in targetPropertyMap)
            {
                menu.AddItem(
                      new GUIContent($"{propName} : {propType.GetFriendlyName()}")
                    , propName == targetPropertyName
                    , SetBindingProperty
                    , (binder, serializedBinder, targetPropertyNameSP, adapterPropertySP, bindingType, propName, propType)
                );
            }

            if (targetPropertyMap.Count < 1)
            {
                menu.AddDisabledItem(new GUIContent($"{targetType.FullName} contains no [ObservableProperty]"));
            }

            menu.ShowAsContext();
        }

        private static void RemoveBindingProperty(object param)
        {
            if (param is not (
                  MonoBinder binder
                , SerializedObject serializedBinder
                , SerializedProperty targetPropertyNameSP
                , SerializedProperty adapterPropertySP
            ))
            {
                return;
            }

            Undo.RecordObject(binder, $"Remove {targetPropertyNameSP.propertyPath}");
            targetPropertyNameSP.stringValue = string.Empty;
            adapterPropertySP.managedReferenceValue = null;
            serializedBinder.ApplyModifiedProperties();
            serializedBinder.Update();
        }

        private static void SetBindingProperty(object param)
        {
            {
                if (param is (
                      MonoBinder binder
                    , SerializedObject serializedBinder
                    , SerializedProperty targetPropertyNameSP
                    , SerializedProperty adapterPropertySP
                    , Type bindingType
                    , string selectedPropName
                    , Type selectedPropType
                ))
                {
                    Undo.RecordObject(binder, $"Set {targetPropertyNameSP.propertyPath}");

                    targetPropertyNameSP.stringValue = selectedPropName;

                    if (TryCreateDefaultAdapter(selectedPropType, bindingType, out var adapter))
                    {
                        adapterPropertySP.managedReferenceValue = adapter;
                    }

                    serializedBinder.ApplyModifiedProperties();
                    serializedBinder.Update();
                    return;
                }
            }

            {
                if (param is (
                      MonoBinder binder
                    , SerializedObject serializedBinder
                    , SerializedProperty targetPropertyNameSP
                    , _
                    , _
                    , string selectedPropName
                    , _
                ))
                {
                    Undo.RecordObject(binder, $"Set {targetPropertyNameSP.propertyPath}");

                    targetPropertyNameSP.stringValue = selectedPropName;
                    serializedBinder.ApplyModifiedProperties();
                    serializedBinder.Update();
                    return;
                }
            }
        }

        private static bool TryCreateDefaultAdapter(Type fromType, Type toType, out IAdapter adapter)
        {
            adapter = null;

            if (s_adapterMap.TryGetValue(toType, out var map)
                && map.TryGetValue(fromType, out var adapterTypes)
            )
            {
                try
                {
                    var sortedTypes = adapterTypes.OrderBy(x => {
                        var attrib = x.GetCustomAttribute<AdapterAttribute>();
                        return attrib?.Order ?? AdapterAttribute.DEFAULT_ORDER;
                    });

                    var adapterType = sortedTypes.FirstOrDefault();

                    if (adapterType != null)
                    {
                        adapter = Activator.CreateInstance(adapterType) as IAdapter;
                    }
                }
                catch
                {
                    adapter = null;
                }
            }

            return adapter != null;
        }

        private static void DrawAdapterPropertyMenu(
              MonoBinder binder
            , SerializedObject serializedBinder
            , Type bindingType
            , Type targetPropertyType
            , SerializedProperty adapterPropertySP
            , Type adapterTypeSaved
        )
        {
            var adapterTypes = new List<Type> {
                typeof(CompositeAdapter),
                typeof(ScriptableAdapter)
            };

            try
            {
                if (s_adapterMap.TryGetValue(bindingType, out var map)
                    && map.TryGetValue(targetPropertyType, out var types)
                )
                {
                    var orderedTypes = types.OrderBy(x => {
                        var attrib = x.GetCustomAttribute<AdapterAttribute>();
                        return attrib?.Order ?? AdapterAttribute.DEFAULT_ORDER;
                    });

                    adapterTypes.AddRange(orderedTypes);
                }
            }
            catch { }

            var menu = new GenericMenu();

            menu.AddItem(
                  new GUIContent("<None>")
                , false
                , RemoveAdapterProperty
                , (binder, serializedBinder, adapterPropertySP)
            );

            AddAdapterTypesToMenu(
                  menu
                , binder
                , serializedBinder
                , adapterPropertySP
                , adapterTypeSaved
                , adapterTypes
                , false
                , string.Empty
            );

            AddAdapterTypesToMenu(
                  menu
                , binder
                , serializedBinder
                , adapterPropertySP
                , adapterTypeSaved
                , GetOtherAdapterTypesExcludeFromType(targetPropertyType)
                , true
                , "Other/"
            );

            menu.ShowAsContext();
        }

        private static List<Type> GetOtherAdapterTypesExcludeFromType(Type fromTypeToExclude)
        {
            var result = new List<Type>();

            foreach (var (toType, map) in s_adapterMap)
            {
                foreach (var (fromType, types) in map)
                {
                    if (fromType == fromTypeToExclude)
                    {
                        continue;
                    }

                    var orderedTypes = types.OrderBy(x => {
                        var attrib = x.GetCustomAttribute<AdapterAttribute>();
                        return attrib?.Order ?? AdapterAttribute.DEFAULT_ORDER;
                    });

                    result.AddRange(orderedTypes);
                }
            }

            return result;
        }

        private static void AddAdapterTypesToMenu(
              GenericMenu menu
            , MonoBinder binder
            , SerializedObject serializedBinder
            , SerializedProperty adapterPropertySP
            , Type adapterTypeSaved
            , List<Type> adapterTypes
            , bool includeDirectory
            , string directoryPrefix
        )
        {
            foreach (var adapterType in adapterTypes)
            {
                var adapterAttrib = adapterType.GetCustomAttribute<AdapterAttribute>();
                var labelAttrib = adapterType.GetCustomAttribute<LabelAttribute>();
                var labelText = labelAttrib?.Label ?? adapterType.Name;
                var keyword = adapterType.IsValueType ? "struct" : "class";
                var directory = labelAttrib?.Directory ?? adapterType.Namespace;
                directory = $"{directoryPrefix}{directory}";

                if (adapterAttrib?.ToType != null)
                {
                    directory = $"{directory}/{adapterAttrib.ToType.GetFriendlyName()}";
                }

                var label = new GUIContent(
                      includeDirectory ? $"{directory}/{labelText}" : labelText
                    , $"{keyword} {adapterType.Name}\nnamespace {adapterType.Namespace}"
                );

                menu.AddItem(
                      label
                    , adapterType == adapterTypeSaved
                    , SetAdapterProperty
                    , (binder, serializedBinder, adapterPropertySP, adapterType)
                );
            }
        }

        private static void RemoveAdapterProperty(object param)
        {
            if (param is not (
                  MonoBinder binder
                , SerializedObject serializedBinder
                , SerializedProperty adapterPropertySP
            ))
            {
                return;
            }

            Undo.RecordObject(binder, $"Remove {adapterPropertySP.propertyPath}");
            adapterPropertySP.managedReferenceValue = null;
            serializedBinder.ApplyModifiedProperties();
            serializedBinder.Update();
        }

        private static void SetAdapterProperty(object param)
        {
            if (param is not (
                  MonoBinder binder
                , SerializedObject serializedBinder
                , SerializedProperty adapterPropertySP
                , Type selectedAdapterType
            ))
            {
                return;
            }

            try
            {
                var adapter = Activator.CreateInstance(selectedAdapterType);

                Undo.RecordObject(binder, $"Set {adapterPropertySP.propertyPath}");
                adapterPropertySP.managedReferenceValue = adapter;
                serializedBinder.ApplyModifiedProperties();
                serializedBinder.Update();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, binder);
            }
        }

        private static void DrawScriptableAdapter(
              SerializedObject serializedBinder
            , SerializedProperty adapterPropertySP
            , ScriptableAdapter scriptableAdapter
        )
        {
            if (scriptableAdapter == null)
            {
                return;
            }

            var assetSP = adapterPropertySP.FindPropertyRelative("_asset");

            if (assetSP == null)
            {
                return;
            }

            var label = new GUIContent(
                  "Asset"
                , $"An adapter derived from {typeof(ScriptableAdapterAsset).Name}."
            );

            EditorGUI.BeginChangeCheck();

            GUILayout.BeginHorizontal();
            GUILayout.Space(15f);
            GUILayout.BeginVertical();

            EditorGUILayout.PropertyField(assetSP, label, false);

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                serializedBinder.ApplyModifiedProperties();
            }
        }

        private static void DrawCompositeAdapter(
              SerializedObject serializedBinder
            , SerializedProperty adapterPropertySP
            , CompositeAdapter compositeAdapter
            , Dictionary<string, ReorderableList> rolMap
        )
        {
            if (compositeAdapter == null)
            {
                return;
            }

            var adapterListSP = adapterPropertySP.FindPropertyRelative("_adapters");

            if (adapterListSP == null)
            {
                return;
            }

            var mapKey = adapterListSP.propertyPath;
            var instanceId = serializedBinder.targetObject.GetInstanceID();
            var foldoutKey = $"__foldout__{instanceId}.{mapKey}";
            var foldoutStr = EditorUserSettings.GetConfigValue(foldoutKey);

            if (bool.TryParse(foldoutStr, out var foldout) == false)
            {
                foldout = false;
            }

            if (rolMap.TryGetValue(mapKey, out var rol) == false || rol == null)
            {
                rolMap[mapKey] = rol = new ReorderableList(serializedBinder, adapterListSP, true, false, true, true) {
                    elementHeight = 25f,
                    onAddDropdownCallback = OnAddDropdown,
                    onRemoveCallback = OnRemove,
                    drawElementCallback = (rect, index, isActive, isFocused)
                        => DrawElement(rect, index, isActive, isFocused, rol),
                };
            }

            var label = new GUIContent(
                  "Adapters"
                , "These adapters will process the input data in an order."
            );

            GUILayout.BeginHorizontal();
            GUILayout.Space(15f);
            GUILayout.BeginVertical();

            foldout = EditorGUILayout.Foldout(foldout, label);
            EditorUserSettings.SetConfigValue(foldoutKey, foldout.ToString());

            if (foldout)
            {
                rol.DoLayoutList();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            static void OnAddDropdown(Rect rect, ReorderableList rol)
            {
                var menu = new GenericMenu();
                var adapterTypes = GetOtherAdapterTypesExcludeFromType(null);

                foreach (var adapterType in adapterTypes)
                {
                    var adapterAttrib = adapterType.GetCustomAttribute<AdapterAttribute>();
                    var labelAttrib = adapterType.GetCustomAttribute<LabelAttribute>();
                    var labelText = labelAttrib?.Label ?? adapterType.Name;
                    var keyword = adapterType.IsValueType ? "struct" : "class";
                    var directory = labelAttrib?.Directory ?? adapterType.Namespace;

                    if (adapterAttrib?.ToType != null)
                    {
                        directory = $"{directory}/{adapterAttrib.ToType.GetFriendlyName()}";
                    }

                    var label = new GUIContent(
                          $"{directory}/{labelText}"
                        , $"{keyword} {adapterType.Name}\nnamespace {adapterType.Namespace}"
                    );

                    menu.AddItem(label, false, AddAdapter, (rol, adapterType));
                }

                menu.ShowAsContext();
            }

            static void OnRemove(ReorderableList rol)
            {
                if (rol.selectedIndices.Count < 1)
                {
                    EditorUtility.DisplayDialog(
                          "Cannot Remove Adapter From List"
                        , "Must select at least 1 entry to remove."
                        , "I understand"
                    );
                    return;
                }

                var serializedObject = rol.serializedProperty.serializedObject;
                var target = serializedObject.targetObject;

                Undo.RecordObject(target, $"Remove {rol.selectedIndices.Count} items at {rol.serializedProperty.propertyPath}");

                foreach (var index in rol.selectedIndices)
                {
                    rol.serializedProperty.DeleteArrayElementAtIndex(index);
                }

                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            static void AddAdapter(object param)
            {
                if (param is not (ReorderableList rol, Type adapterType))
                {
                    return;
                }

                var serializedObject = rol.serializedProperty.serializedObject;
                var target = serializedObject.targetObject;

                try
                {
                    var adapter = Activator.CreateInstance(adapterType);
                    var index = rol.serializedProperty.arraySize;

                    Undo.RecordObject(target, $"Set {rol.serializedProperty.propertyPath}.Array.data[{index}]");

                    rol.serializedProperty.arraySize++;
                    rol.index = index;

                    var elementSP = rol.serializedProperty.GetArrayElementAtIndex(index);
                    elementSP.managedReferenceValue = adapter;
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex, target);
                }
            }

            static void DrawElement(Rect rect, int index, bool isActive, bool isFocused, ReorderableList rol)
            {
                var elementSP = rol.serializedProperty.GetArrayElementAtIndex(index);
                var margin = 50f;
                rect = new Rect(rect.x + (margin / 2f), rect.y + 3f, rect.width - margin, 21f);

                GetAdapterPropertyData(
                      elementSP
                    , out var adapterTypeSaved
                    , out var adapterPropertyLabel
                    , out _
                    , out _
                );

                if (GUI.Button(rect, adapterPropertyLabel))
                {
                    var menu = new GenericMenu();
                    var adapterTypes = GetOtherAdapterTypesExcludeFromType(adapterTypeSaved);

                    foreach (var adapterType in adapterTypes)
                    {
                        var adapterAttrib = adapterType.GetCustomAttribute<AdapterAttribute>();
                        var labelAttrib = adapterType.GetCustomAttribute<LabelAttribute>();
                        var labelText = labelAttrib?.Label ?? adapterType.Name;
                        var keyword = adapterType.IsValueType ? "struct" : "class";
                        var directory = labelAttrib?.Directory ?? adapterType.Namespace;

                        if (adapterAttrib?.ToType != null)
                        {
                            directory = $"{directory}/{adapterAttrib.ToType.GetFriendlyName()}";
                        }

                        var label = new GUIContent(
                              $"{directory}/{labelText}"
                            , $"{keyword} {adapterType.Name}\nnamespace {adapterType.Namespace}"
                        );

                        menu.AddItem(
                              label
                            , adapterType == adapterTypeSaved
                            , SetAdapterAtElement
                            , (rol, elementSP, adapterType)
                        );
                    }

                    menu.ShowAsContext();
                }
            }

            static void SetAdapterAtElement(object param)
            {
                if (param is not (
                      ReorderableList rol
                    , SerializedProperty elementSP
                    , Type selectedAdapterType
                ))
                {
                    return;
                }

                var serializedObject = rol.serializedProperty.serializedObject;
                var target = serializedObject.targetObject;

                try
                {
                    var adapter = Activator.CreateInstance(selectedAdapterType);

                    Undo.RecordObject(target, $"Set {elementSP.propertyPath}");
                    elementSP.managedReferenceValue = adapter;
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex, target);
                }
            }
        }

        private static void DrawBindingCommand(
              MonoBinder binder
            , SerializedObject serializedBinder
            , Type binderType
            , string bindingName
            , Type bindingType
            , Type targetType
            , Dictionary<string, Type> targetCommandMap
            , Dictionary<string, string> labelMap
        )
        {
            var targetCommandNameSPPath = $"_bindingCommandFor{bindingName}.<TargetCommandName>k__BackingField";
            var targetCommandNameSP = serializedBinder.FindProperty(targetCommandNameSPPath);

            GetTargetCommandData(
                  targetType
                , targetCommandMap
                , targetCommandNameSP
                , out var targetCommandName
                , out var targetCommandLabel
            );

            if (labelMap.TryGetValue(bindingName, out var bindingLabelText) == false)
            {
                bindingLabelText = ObjectNames.NicifyVariableName(bindingName);
            }

            var bindingTypeName = bindingType?.GetFriendlyName() ?? string.Empty;
            var bindingTypeLabel = new GUIContent(bindingTypeName, bindingType?.GetFullName() ?? string.Empty);
            var bindingLabel = new GUIContent(
                  bindingLabelText
                , $"{binderType.Name}.{bindingName} ( {bindingTypeName} )"
            );

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                var color = GUI.color;
                GUI.color = Color.green;
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.LabelField(bindingLabel, bindingTypeLabel);
                EditorGUILayout.EndHorizontal();
                GUI.color = color;

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel("Bind To Command");

                    if (GUILayout.Button(targetCommandLabel))
                    {
                        DrawBindingCommandMenu(
                              binder
                            , serializedBinder
                            , targetCommandNameSP
                            , targetType
                            , targetCommandName
                            , targetCommandMap
                        );
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        private static void GetTargetCommandData(
              Type targetType
            , Dictionary<string, Type> targetCommandMap
            , SerializedProperty targetCommandNameSP
            , out string targetCommandName
            , out GUIContent targetCommandLabel
        )
        {
            if (string.IsNullOrWhiteSpace(targetCommandNameSP.stringValue))
            {
                targetCommandName = "";
                targetCommandLabel = new GUIContent("< undefined >");
            }
            else
            {
                var candidate = targetCommandNameSP.stringValue;

                if (targetCommandMap.TryGetValue(candidate, out var commandType) == false)
                {
                    targetCommandName = "";
                    targetCommandLabel = new GUIContent(
                          $"< invalid > {candidate}"
                        , $"{targetType.FullName} does not contain a command named {candidate}"
                    );
                }
                else if (commandType == null)
                {
                    targetCommandName = candidate;
                    targetCommandLabel = new GUIContent(
                          $"{candidate}"
                        , $"command {candidate}\nclass {targetType.Name}\nnamespace {targetType.Namespace}"
                    );
                }
                else
                {
                    var returnTypeName = commandType.GetFriendlyName();
                    targetCommandName = candidate;
                    targetCommandLabel = new GUIContent(
                          $"{candidate} ( {returnTypeName} )"
                        , $"command {candidate} : {returnTypeName}\nclass {targetType.Name}\nnamespace {targetType.Namespace}"
                    );
                }
            }
        }

        private static void DrawBindingCommandMenu(
              MonoBinder binder
            , SerializedObject serializedBinder
            , SerializedProperty targetCommandNameSP
            , Type targetType
            , string targetCommandName
            , Dictionary<string, Type> targetCommandMap
        )
        {
            var menu = new GenericMenu();

            menu.AddItem(
                  new GUIContent("<None>")
                , false
                , RemoveBindingCommand
                , (binder, serializedBinder, targetCommandNameSP)
            );

            foreach (var (commandName, commandType) in targetCommandMap)
            {
                var label = commandType == null
                    ? new GUIContent(commandName)
                    : new GUIContent($"{commandName} ( {commandType.GetFriendlyName()} )");

                menu.AddItem(
                      label
                    , commandName == targetCommandName
                    , SetBindingCommand
                    , (binder, serializedBinder, targetCommandNameSP, commandName)
                );
            }

            if (targetCommandMap.Count < 1)
            {
                menu.AddDisabledItem(new GUIContent($"{targetType.FullName} contains no [ObservableCommand]"));
            }

            menu.ShowAsContext();
        }

        private static void RemoveBindingCommand(object param)
        {
            if (param is not (
                  MonoBinder binder
                , SerializedObject serializedBinder
                , SerializedProperty targetCommandNameSP
            ))
            {
                return;
            }

            Undo.RecordObject(binder, $"Remove {targetCommandNameSP.propertyPath}");
            targetCommandNameSP.stringValue = string.Empty;
            serializedBinder.ApplyModifiedProperties();
            serializedBinder.Update();
        }

        private static void SetBindingCommand(object param)
        {
            if (param is not (
                  MonoBinder binder
                , SerializedObject serializedBinder
                , SerializedProperty targetCommandNameSP
                , string selectedCommandName
            ))
            {
                return;
            }

            Undo.RecordObject(binder, $"Set {targetCommandNameSP.propertyPath}");

            targetCommandNameSP.stringValue = selectedCommandName;
            serializedBinder.ApplyModifiedProperties();
            serializedBinder.Update();
        }

    }
}