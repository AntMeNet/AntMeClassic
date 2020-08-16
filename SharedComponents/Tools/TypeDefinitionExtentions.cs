using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.SharedComponents.Tools
{
    public static class TypeDefinitionExtentions
    {

        /// <summary>
        /// Checks if the TypeDefinition is a child TypeDefinition of the parent TypeDefiniton
        /// </summary>
        /// <param name="self">Child TypeDefinition</param>
        /// <param name="parentTypeDefinition">parent TypeDefinition</param>
        /// <returns></returns>
        public static bool IsSubclassOf(this TypeDefinition self, TypeDefinition parentTypeDefinition)
        {
            return !AreTypeDefinitionsEqual(self, parentTypeDefinition) && self.EnumerateBaseClasses().Any(baseClassDefinition => AreTypeDefinitionsEqual(baseClassDefinition, parentTypeDefinition));
        }

        /// <summary>
        /// Enumerates throw all the base TypeDefinition
        /// </summary>
        /// <param name="self"></param>
        /// <returns>Enumerator of all base TypeDefinitions</returns>
        public static IEnumerable<TypeDefinition> EnumerateBaseClasses(this TypeDefinition self)
        {
            var typeDefinition = self;
            while (typeDefinition != null)
            {
                yield return typeDefinition;
                typeDefinition = typeDefinition.BaseType?.Resolve();
            }
        }

        private static bool AreTypeDefinitionsEqual(TypeDefinition type1, TypeDefinition Type2)
        {
            return type1.MetadataToken == Type2.MetadataToken && type1.FullName == Type2.FullName;
        }

    }
}
