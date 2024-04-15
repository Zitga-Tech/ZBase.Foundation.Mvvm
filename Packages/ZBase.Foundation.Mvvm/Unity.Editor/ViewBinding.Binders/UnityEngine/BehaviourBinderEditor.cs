using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class BehaviourBinderEditor
    {
        [MenuItem("CONTEXT/Behaviour/Binder")]
        static void BindBehaviour(MenuCommand command)
        {
            var target = command.context as Behaviour;
            Setup(target);
        }

        public static MonoBehaviour Setup(Behaviour target)
        {
            var comp = Undo.AddComponent<BehaviourBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}
