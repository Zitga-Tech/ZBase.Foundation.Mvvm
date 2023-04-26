using System;
using UnityEngine;

namespace MvvmTests
{
    public class MvvmTest : MonoBehaviour
    {
        [SerializeReference, SerializeReferenceDropdown]
        private IBoolAdapter _boolAdapter;
    }

    public interface IAdapter
    {
    }

    public interface IAdapter<T> : IAdapter
    {
        T Convert(object value);
    }

    public interface IBoolAdapter : IAdapter<bool> { }

    [Serializable]
    public class BoolAdapter : IBoolAdapter
    {
        public bool Convert(object value)
        {
            return (bool)value;
        }
    }

    [Serializable]
    public class BoolAdapter2 : IBoolAdapter
    {
        public bool Convert(object value)
        {
            return (bool)value;
        }
    }
}
