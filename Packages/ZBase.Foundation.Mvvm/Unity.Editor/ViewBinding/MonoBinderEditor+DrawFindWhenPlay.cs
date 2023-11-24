using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using ZBase.Foundation.Mvvm.ViewBinding;
using ZBase.Foundation.Mvvm.ViewBinding.Adapters;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Adapters;
using ZBase.Foundation.Mvvm.ViewBinding.SourceGen;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    partial class MonoBinderEditor
    {
        private void DrawFindWhenPlay(MonoBinder binder)
        {
            EditorGUILayout.HelpBox(
                  "Find When Play does NOT require the Context to be set up beforehand.\n" +
                  "The actual Context will be retrieved on entering play mode or at runtime."
                , MessageType.Info
            );

            EditorGUILayout.Space();

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
                    , bindingCommandLabelMap
                );
            }
        }

        private static void DrawBindingProperty(
              MonoBinder binder
            , SerializedObject serializedBinder
            , Type binderType
            , string bindingName
            , Type bindingType
            , Dictionary<string, string> labelMap
            , Dictionary<string, ReorderableList> rolMap
        )
        {
            var targetPropertyNameSPPath = $"_bindingFieldFor{bindingName}.<TargetPropertyName>k__BackingField";
            var adapterSPPath = $"_converterFor{bindingName}.<Adapter>k__BackingField";

            var targetPropertyNameSP = serializedBinder.FindProperty(targetPropertyNameSPPath);
            var adapterPropertySP = serializedBinder.FindProperty(adapterSPPath);

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
                    var selectedPropName = EditorGUILayout.TextField("Bind To Property", targetPropertyNameSP.stringValue);

                    if (string.Equals(targetPropertyNameSP.stringValue, selectedPropName) == false)
                    {
                        Undo.RecordObject(binder, $"Set {targetPropertyNameSP.propertyPath}");

                        targetPropertyNameSP.stringValue = selectedPropName;

                        serializedBinder.ApplyModifiedProperties();
                        serializedBinder.Update();
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel("Convert By");

                    if (GUILayout.Button(adapterPropertyLabel))
                    {
                        DrawAdapterPropertyMenu(
                              binder
                            , serializedBinder
                            , bindingType
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

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        private static void DrawAdapterPropertyMenu(
              MonoBinder binder
            , SerializedObject serializedBinder
            , Type bindingType
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
                if (s_adapterMap.TryGetValue(bindingType, out var map))
                {
                    foreach (var types in map.Values)
                    {
                        var orderedTypes = types.OrderBy(x => {
                            var attrib = x.GetCustomAttribute<AdapterAttribute>();
                            return attrib?.Order ?? AdapterAttribute.DEFAULT_ORDER;
                        });

                        adapterTypes.AddRange(orderedTypes);
                    }
                }
            }
            catch { }

            var menu = new GenericMenu();

            menu.AddItem(
                  new GUIContent("None")
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
                , GetOtherAdapterTypesExcludeToType(bindingType)
                , true
                , "Other/"
            );

            menu.ShowAsContext();
        }

        private static IEnumerable<Type> GetOtherAdapterTypesExcludeToType(Type toTypeToExclude)
        {
            var result = new List<Type>();

            foreach (var (toType, map) in s_adapterMap)
            {
                if (toType == toTypeToExclude)
                {
                    continue;
                }

                foreach (var types in map.Values)
                {
                    var orderedTypes = types.OrderBy(x => {
                        var attrib = x.GetCustomAttribute<AdapterAttribute>();
                        return attrib?.Order ?? AdapterAttribute.DEFAULT_ORDER;
                    });

                    result.AddRange(orderedTypes);
                }
            }

            return result;
        }

        private static void DrawBindingCommand(
              MonoBinder binder
            , SerializedObject serializedBinder
            , Type binderType
            , string bindingName
            , Type bindingType
            , Dictionary<string, string> labelMap
        )
        {
            var targetCommandNameSPPath = $"_bindingCommandFor{bindingName}.<TargetCommandName>k__BackingField";
            var targetCommandNameSP = serializedBinder.FindProperty(targetCommandNameSPPath);

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
                    var selectedCommandName = EditorGUILayout.TextField("Bind To Command", targetCommandNameSP.stringValue);

                    if (string.Equals(selectedCommandName, targetCommandNameSP.stringValue) == false)
                    {
                        Undo.RecordObject(binder, $"Set {targetCommandNameSP.propertyPath}");

                        targetCommandNameSP.stringValue = selectedCommandName;

                        serializedBinder.ApplyModifiedProperties();
                        serializedBinder.Update();
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }
}