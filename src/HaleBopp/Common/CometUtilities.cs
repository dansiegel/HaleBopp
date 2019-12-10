using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Comet;
using DryIoc;

namespace HaleBopp.Common
{
    internal static class CometUtilities
    {
        private static readonly Dictionary<Type, IEnumerable<FieldInfo>> Mappings = new Dictionary<Type, IEnumerable<FieldInfo>>();
        private static readonly Assembly CometAssembly = typeof(BindingObject).Assembly;

        internal static void SetMappings(View view)
        {
            var viewType = view.GetType();
            if (Mappings.ContainsKey(viewType)) return;

            var fields = CheckForStateAttributes(viewType);
            var mappings = fields.Where(x => x.GetValue(view) is null);
            Mappings[viewType] = mappings;
        }

        public static IEnumerable<object> GetViewStateObjects(View view)
        {
            var fields = CheckForStateAttributes(view.GetType()).ToList();
            foreach(var field in fields)
            {
                var state = field.GetValue(view);

                if (IsGenericStateObject(field))
                {
                    yield return field.FieldType.GetProperty("Value").GetValue(state);
                }

                yield return state;
            }
        }

        public static IEnumerable<FieldInfo> CheckForStateAttributes(Type type)
        {
            if (Mappings.ContainsKey(type))
                return Mappings[type];

            return type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(x => IsGenericStateObject(x) || DeclaresStateAttribute(x));
        }

        private static bool IsGenericStateObject(FieldInfo info) =>
            info.FieldType.Assembly == CometAssembly && info.FieldType.Name == "State`1";

        private static bool DeclaresStateAttribute(FieldInfo info) =>
            Attribute.IsDefined(info, typeof(StateAttribute)) && info.FieldType.ImplementsServiceType(typeof(INotifyPropertyRead));

        public static IEnumerable<string> GetStatePropertiesAndFields(IContainer container, Type viewType)
        {
            var fields = CheckForStateAttributes(viewType);

            foreach (var field in fields)
            {
                if (field.FieldType.Assembly == CometAssembly)
                {
                    var rootType = field.FieldType.GenericTypeArguments.First();
                    if (rootType.IsPrimitive)
                        continue;

                    container.RegisterDelegate(field.FieldType, r =>
                    {
                        var instance = r.Resolve(rootType);
                        return Activator.CreateInstance(rootType, instance);
                    }, Reuse.Transient);
                }

                if (field.FieldType.IsPrimitive)
                    continue;

                yield return field.Name;
            }
        }
    }
}
