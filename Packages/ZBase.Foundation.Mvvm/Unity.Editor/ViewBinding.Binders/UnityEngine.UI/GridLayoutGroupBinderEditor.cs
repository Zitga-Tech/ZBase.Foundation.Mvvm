#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class GridLayoutGroupBinderEditor
    {
        [MenuItem("CONTEXT/GridLayoutGroup/Binder")]
        static void BindGridLayoutGroup(MenuCommand command)
        {
            if (command.context is GridLayoutGroup target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(GridLayoutGroup target)
        {
            var comp = Undo.AddComponent<GridLayoutGroupBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
