using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using ZBase.Foundation.Mvvm.ViewBinding;
using ZBase.Foundation.Mvvm.ViewBinding.Adapters;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Adapters;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    [UnityEditor.CustomEditor(typeof(MonoBinder), editorForChildClasses: true)]
    public partial class MonoBinderEditor : UnityEditor.Editor
    {
        /// <summary>
        /// To Type => From Type => Adapter Type HashSet
        /// </summary>
        /// <remarks>
        /// Does not include <see cref="ScriptableAdapter"/>
        /// and <see cref="CompositeAdapter"/>
        /// </remarks>
        private readonly static Dictionary<Type, Dictionary<Type, HashSet<Type>>> s_adapterMap = new();

        static MonoBinderEditor()
        {
            Init();
        }

        private static void Init()
        {
            var adapterTypes = TypeCache.GetTypesDerivedFrom<IAdapter>()
                .Where(static x => x.IsAbstract == false && x.IsSubclassOf(typeof(UnityEngine.Object)) == false)
                .Select(static x => (x, x.GetCustomAttribute<AdapterAttribute>()))
                .Where(static x => x.Item2 != null);

            foreach (var (adapterType, attrib) in adapterTypes)
            {
                if (s_adapterMap.TryGetValue(attrib.ToType, out var map) == false)
                {
                    s_adapterMap[attrib.ToType] = map = new Dictionary<Type, HashSet<Type>>();
                }

                if (map.TryGetValue(attrib.FromType, out var types) == false)
                {
                    map[attrib.FromType] = types = new HashSet<Type>();
                }

                types.Add(adapterType);
            }
        }

        private readonly Dictionary<string, ReorderableList> _rolMap = new();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (this.target is not MonoBinder binder)
            {
                return;
            }

            EditorGUILayout.Space();

            if (binder._contextSetting == MonoBinder.BindingContextSetting.FindWhenPlay)
            {
                DrawFindWhenPlay(binder);
            }
            else
            {
                DrawPresetOnEditor(binder);
            }
        }
    }
}