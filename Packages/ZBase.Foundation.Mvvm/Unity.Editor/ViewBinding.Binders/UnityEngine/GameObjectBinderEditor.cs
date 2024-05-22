using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class GameObjectBinderEditor
    {
        [MenuItem("GameObject/Add Binder For GameObject", priority = 0)]
        static void BindGameObject(MenuCommand command)
        {
            if (command.context is GameObject target)
            {
                Setup(target);
            }
        }
        
        [MenuItem("CONTEXT/Component/Binder For This GameObject")]
        static void BindGameObjectComponent(MenuCommand command)
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
