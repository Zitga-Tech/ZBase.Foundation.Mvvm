using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEditor;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Tests
{
    public static class TypeCacheTests
    {
        private static CustomBinder s_binder = new CustomBinder();

        [Test, Performance]
        public static void TypeCache_Test()
        {
            Measure.Method(() => Test(s_binder))
            .WarmupCount(5)
            .IterationsPerMeasurement(1000)
            .MeasurementCount(20)
            .Run();

            static void Test(CustomBinder binder)
            {
                var type = binder.GetType();

                var fields = TypeCache.GetFieldsWithAttribute<GeneratedBindingFieldAttribute>()
                    .Where(x => x.DeclaringType == type);

                var count = fields.Count();
            }
        }

        [Test, Performance]
        public static void Reflection_Test()
        {
            Measure.Method(() => Test(s_binder))
            .WarmupCount(5)
            .IterationsPerMeasurement(1000)
            .MeasurementCount(20)
            .Run();

            static void Test(CustomBinder binder)
            {
                var type = binder.GetType();

                var fields = type.GetFields()
                    .Where(x => x.GetCustomAttribute<GeneratedBindingFieldAttribute>() != null);

                var count = fields.Count();
            }
        }
    }

    public partial class CustomBinder : IBinder
    {
        public IBindingContext Context { get; set; }

        [Binding]
        private void OnUpdate(int _) { }
    }
}