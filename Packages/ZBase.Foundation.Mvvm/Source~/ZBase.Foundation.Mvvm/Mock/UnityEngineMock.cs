#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it

using System;
using System.Collections.Generic;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class SerializeField : Attribute { }

    public enum FindObjectsInactive
    {
        Exclude,
        Include
    }

    [Flags]
    public enum HideFlags
    {
        None = 0x0,
        HideInHierarchy = 0x1,
        HideInInspector = 0x2,
        DontSaveInEditor = 0x4,
        NotEditable = 0x8,
        DontSaveInBuild = 0x10,
        DontUnloadUnusedAsset = 0x20,
        DontSave = 0x34,
        HideAndDontSave = 0x3D
    }

    public enum FindObjectsSortMode
    {
        None,
        InstanceID
    }

    public class Object
    {
        public string name { get; set; }

        public extern HideFlags hideFlags { get; set; }

        public int GetInstanceID()
            => throw new NotImplementedException();

        public override int GetHashCode()
            => throw new NotImplementedException();

        public override bool Equals(object other)
            => throw new NotImplementedException();

        public static implicit operator bool(Object exists)
            => throw new NotImplementedException();

        public static void Destroy(Object obj)
            => throw new NotImplementedException();

        public static void DestroyImmediate(Object obj)
            => throw new NotImplementedException();

        public static Object[] FindObjectsOfType(Type type)
            => throw new NotImplementedException();

        public static Object[] FindObjectsByType(Type type, FindObjectsSortMode sortMode)
            => throw new NotImplementedException();

        public static T[] FindObjectsOfType<T>() where T : Object
            => throw new NotImplementedException();

        public static T[] FindObjectsByType<T>(FindObjectsSortMode sortMode) where T : Object
            => throw new NotImplementedException();

        public static T[] FindObjectsOfType<T>(bool includeInactive) where T : Object
            => throw new NotImplementedException();

        public static T[] FindObjectsByType<T>(FindObjectsInactive findObjectsInactive, FindObjectsSortMode sortMode) where T : Object
            => throw new NotImplementedException();

        public static T FindObjectOfType<T>() where T : Object
            => throw new NotImplementedException();

        public static T FindObjectOfType<T>(bool includeInactive) where T : Object
            => throw new NotImplementedException();

        public static T FindFirstObjectByType<T>() where T : Object
            => throw new NotImplementedException();

        public static T FindAnyObjectByType<T>() where T : Object
            => throw new NotImplementedException();

        public static T FindFirstObjectByType<T>(FindObjectsInactive findObjectsInactive) where T : Object
            => throw new NotImplementedException();

        public static T FindAnyObjectByType<T>(FindObjectsInactive findObjectsInactive) where T : Object
            => throw new NotImplementedException();

        public static Object FindObjectOfType(Type type)
            => throw new NotImplementedException();

        public static Object FindFirstObjectByType(Type type)
            => throw new NotImplementedException();

        public static Object FindAnyObjectByType(Type type)
            => throw new NotImplementedException();

        public static Object FindObjectOfType(Type type, bool includeInactive)
            => throw new NotImplementedException();

        public static Object FindFirstObjectByType(Type type, FindObjectsInactive findObjectsInactive)
            => throw new NotImplementedException();

        public static Object FindAnyObjectByType(Type type, FindObjectsInactive findObjectsInactive)
            => throw new NotImplementedException();

        public override string ToString()
            => throw new NotImplementedException();

        public static bool operator ==(Object x, Object y)
            => throw new NotImplementedException();

        public static bool operator !=(Object x, Object y)
            => throw new NotImplementedException();
    }

    public class Component : Object
    {
        public extern Transform transform { get; }

        public extern GameObject gameObject { get; }

        public string tag { get; set; }

        public Component GetComponent(Type type)
            => throw new NotImplementedException();

        public T GetComponent<T>()
            => throw new NotImplementedException();

        public bool TryGetComponent(Type type, out Component component)
            => throw new NotImplementedException();

        public bool TryGetComponent<T>(out T component)
            => throw new NotImplementedException();

        public Component GetComponentInChildren(Type t, bool includeInactive)
            => throw new NotImplementedException();

        public Component GetComponentInChildren(Type t)
            => throw new NotImplementedException();

        public T GetComponentInChildren<T>(bool includeInactive)
            => throw new NotImplementedException();

        public T GetComponentInChildren<T>()
            => throw new NotImplementedException();

        public Component[] GetComponentsInChildren(Type t, bool includeInactive)
            => throw new NotImplementedException();

        public Component[] GetComponentsInChildren(Type t)
            => throw new NotImplementedException();

        public T[] GetComponentsInChildren<T>(bool includeInactive)
            => throw new NotImplementedException();

        public void GetComponentsInChildren<T>(bool includeInactive, List<T> result)
            => throw new NotImplementedException();

        public T[] GetComponentsInChildren<T>()
            => throw new NotImplementedException();

        public void GetComponentsInChildren<T>(List<T> results)
            => throw new NotImplementedException();

        public Component GetComponentInParent(Type t, bool includeInactive)
            => throw new NotImplementedException();

        public Component GetComponentInParent(Type t)
            => throw new NotImplementedException();

        public T GetComponentInParent<T>(bool includeInactive)
            => throw new NotImplementedException();

        public T GetComponentInParent<T>()
            => throw new NotImplementedException();

        public Component[] GetComponentsInParent(Type t, bool includeInactive)
            => throw new NotImplementedException();

        public Component[] GetComponentsInParent(Type t)
            => throw new NotImplementedException();

        public T[] GetComponentsInParent<T>(bool includeInactive)
            => throw new NotImplementedException();

        public void GetComponentsInParent<T>(bool includeInactive, List<T> results)
            => throw new NotImplementedException();

        public T[] GetComponentsInParent<T>()
            => throw new NotImplementedException();

        public Component[] GetComponents(Type type)
            => throw new NotImplementedException();

        public void GetComponents(Type type, List<Component> results)
            => throw new NotImplementedException();

        public void GetComponents<T>(List<T> results)
            => throw new NotImplementedException();

        public T[] GetComponents<T>()
            => throw new NotImplementedException();

        public bool CompareTag(string tag)
            => throw new NotImplementedException();

        public void SendMessageUpwards(string methodName, object value)
            => throw new NotImplementedException();

        public void SendMessageUpwards(string methodName)
            => throw new NotImplementedException();

        public void SendMessageUpwards(string methodName, SendMessageOptions options)
            => throw new NotImplementedException();

        public void SendMessage(string methodName, object value)
            => throw new NotImplementedException();

        public void SendMessage(string methodName)
            => throw new NotImplementedException();

        public void SendMessage(string methodName, SendMessageOptions options)
            => throw new NotImplementedException();

        public void BroadcastMessage(string methodName, object parameter)
            => throw new NotImplementedException();

        public void BroadcastMessage(string methodName)
            => throw new NotImplementedException();

        public void BroadcastMessage(string methodName, SendMessageOptions options)
            => throw new NotImplementedException();
    }

    public enum SendMessageOptions
    {
        RequireReceiver,
        DontRequireReceiver
    }

    public class Transform : Component { }

    public sealed class GameObject : Object { }

    public class Behaviour : Component
    {
        public extern bool enabled { get; set; }

        public extern bool isActiveAndEnabled { get; }
    }

    public class MonoBehaviour : Behaviour { }

    public class CanvasGroup : Behaviour
    {
        public float alpha { get; set; }

        public bool interactable { get; set; }

        public bool blocksRaycasts { get; set; }
    }
}
