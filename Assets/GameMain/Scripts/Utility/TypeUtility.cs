using System;

namespace Flower
{
    public static class TypeUtility
    {
        private static readonly string NAMESPACE_NAME = "Flower";

        public static string GetEntitFullTypeName(string entityTypeName)
        {
            if (string.IsNullOrEmpty(entityTypeName))
            {
                return string.Empty;
            }

            if (entityTypeName.StartsWith(string.Format("{0}.", NAMESPACE_NAME)))
            {
                return entityTypeName;
            }

            return string.Format("{0}.{1}", NAMESPACE_NAME, entityTypeName);
        }

        public static Type GetEntityType(string entityTypeName)
        {
            string typeFullName = GetEntitFullTypeName(entityTypeName);

            if (string.IsNullOrEmpty(typeFullName))
            {
                return null;
            }

            return Type.GetType(typeFullName);
        }
    }
}