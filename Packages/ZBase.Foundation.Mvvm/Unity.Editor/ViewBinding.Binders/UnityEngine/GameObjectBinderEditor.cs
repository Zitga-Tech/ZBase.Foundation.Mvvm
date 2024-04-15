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
            if (command.context is GameObject target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(GameObject target)
        {
            var comp = Undo.AddComponent<GameObjectBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}
