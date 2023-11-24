using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/GameObject Binder")]
    public partial class GameObjectBinder : MonoBinder<GameObject>
    {
        protected sealed override void OnAwake(ref GameObject[] targets)
        {
            if (targets.Length < 1)
            {
                targets = new GameObject[1] { this.gameObject };
            }
        }

        [BindingProperty]
        [field: Label("Active Self")]
        [field: HideInInspector]
        private void SetActive(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].SetActive(value);
            }
        }
    }
}
