using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class BehaviourBinderEditor
    {
        [MenuItem("CONTEXT/Behaviour/Binder For This Component")]
        static void BindBehaviour(MenuCommand command)
        {
            if (command.context is Behaviour target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(Behaviour target)
        {
            var comp = Undo.AddComponent<BehaviourBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}
