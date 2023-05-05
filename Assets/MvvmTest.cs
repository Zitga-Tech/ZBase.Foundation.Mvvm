using System;
using System.Runtime.InteropServices;
using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.Input;
using ZBase.Foundation.Mvvm.Unions;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace MvvmTests
{
    public class MvvmTest : MonoBehaviour, IObservableContext
    {
        private readonly TextModel _textModel = new TextModel();

        [SerializeField]
        private TMP_TextBinder _textBinder;

        public IObservableObject Target => _textModel;

        private void Awake()
        {
        }
    }

    [Serializable]
    public partial class TextModel : IObservableObject
    {
        [ObservableProperty]
        private int _intValue;
    }
}
