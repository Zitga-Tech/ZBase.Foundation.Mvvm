using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEditor;
using ZBase.Foundation.Mvvm.ViewBinding;
using ZBase.Foundation.Mvvm.ViewBinding.SourceGen;

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

                var fields = TypeCache.GetFieldsWithAttribute<GeneratedBindingPropertyAttribute>()
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
                    .Where(x => x.GetCustomAttribute<GeneratedBindingPropertyAttribute>() != null);

                var count = fields.Count();
            }
        }
    }

    public partial class CustomBinder : IBinder
    {
        public IBindingContext Context { get; set; }

        [BindingProperty]
        private void OnUpdate(int _) { }
    }
}