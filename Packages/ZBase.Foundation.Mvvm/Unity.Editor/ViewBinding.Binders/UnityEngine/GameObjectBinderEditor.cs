#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class GameObjectBinderEditor
    {
        [MenuItem("CONTEXT/GameObject/Binder")]
        static void BindGameObject(MenuCommand command)
        {
            var target = command.context as GameObject;
            Setup(target);
        }

        public static MonoBehaviour Setup(GameObject target)
        {
            var comp = Undo.AddComponent<GameObjectBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
