using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using Newtonsoft.Json;

namespace DataAccessLayer.LogHelper
{
    public static class LogHelper
    {
        public static Log CreateLog<T>(T entity, string actionType, int userId, object? oldData = null)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string? changedData = null;
            string? originalData = null;

            if (actionType == "Add")
            {
                var nonDefaultFields = GetNonDefaultProperties(entity);
                changedData = JsonConvert.SerializeObject(nonDefaultFields, settings);
            }
            else if (actionType == "Update" && oldData != null)
            {
                var changes = GetChangedProperties((T)oldData, entity);

                var changedOnly = new Dictionary<string, object?>();
                var originalOnly = new Dictionary<string, object?>();

                foreach (var (key, (original, current)) in changes)
                {
                    originalOnly[key] = original;
                    changedOnly[key] = current;
                }


                changedData = JsonConvert.SerializeObject(changedOnly, settings);
                originalData = JsonConvert.SerializeObject(originalOnly, settings);
            }
            else if (actionType == "Delete" && oldData != null)
            {
                originalData = JsonConvert.SerializeObject(oldData, settings);
            }

            return new Log
            {
                TableName = typeof(T).Name,
                ActionId = actionType switch
                {
                    "Add" => 1,
                    "Update" => 2,
                    "Delete" => 3,
                    _ => 0
                },
                UserId = userId,
                CreateDate = DateTime.Now,
                Status = true,
                ChangedData = changedData,
                OriginalData = originalData,
                Description = $"{actionType} işlemi gerçekleştirildi."
            };
        }

        private static Dictionary<string, (object? Original, object? Current)> GetChangedProperties<T>(T original, T current)
        {
            var changes = new Dictionary<string, (object? Original, object? Current)>();
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                // Skip navigation properties (complex types)
                if (!IsSimpleType(prop.PropertyType))
                    continue;

                var originalValue = prop.GetValue(original);
                var currentValue = prop.GetValue(current);

                if (!Equals(originalValue, currentValue))
                {
                    changes[prop.Name] = (originalValue, currentValue);
                }
            }

            return changes;
        }

        private static Dictionary<string, object?> GetNonDefaultProperties<T>(T entity)
        {
            var result = new Dictionary<string, object?>();
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                if (!IsSimpleType(prop.PropertyType))
                    continue;

                var value = prop.GetValue(entity);
                var defaultValue = prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null;

                if (value != null && !Equals(value, defaultValue))
                {
                    result[prop.Name] = value;
                }
            }

            return result;
        }

        // Helper: checks if a type is simple (primitive, string, enum, or nullable of these)
        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(string)
                   || (Nullable.GetUnderlyingType(type) != null && IsSimpleType(Nullable.GetUnderlyingType(type)));
        }

    }

}


