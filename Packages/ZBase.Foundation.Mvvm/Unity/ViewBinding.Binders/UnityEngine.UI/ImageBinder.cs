using System;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Image Binder")]
    public partial class ImageBinder : MonoBinder
    {
        [SerializeField]
        private Image[] _targets = new Image[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Image>(out var target))
                {
                    _targets = new Image[] { target };
                }
            }

            if (_targets.Length < 1)
            {
                Debug.LogWarning($"The toggle list is empty.", this);
            }
        }

        [Binding]
        [field: Label("color")]
        [field: HideInInspector]
        private void SetColor(Color color)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.color = color;
                }
            }
        }

        [Binding]
        [field: Label("sprite")]
        [field: HideInInspector]
        private void SetSprite(Sprite sprite)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.sprite = sprite;
                }
            }
        }
    }
}
