using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BitmaskTypes.Editor
{
    public static class EnumNameTypeAssetCodeGeneration 
    {
        [MenuItem("Gemserk/Regenerate Enum Names Code")]
        public static void RegenerateAllEnumTypeCode()
        {
            GenerateEnumNamesCode(typeof(EnumNameTypeAsset));
        }

        private static void GenerateEnumNamesCode(Type t)
        {
            var paths = AssetDatabase.FindAssets($"t:{t}")
                .Select(AssetDatabase.GUIDToAssetPath);

            foreach (var path in paths)
            {
                var folder = Path.GetDirectoryName(path);
                var enumNameType = AssetDatabase.LoadAssetAtPath<EnumNameTypeAsset>(path);
                if (!enumNameType.generateCode)
                    continue;
                
                if (string.IsNullOrEmpty(enumNameType.className))
                    continue;
                
                if (string.IsNullOrEmpty(enumNameType.namespaceName))
                    continue;
                
                GenerateEnumNamesCode(enumNameType, folder);
            }
        }

        private static void GenerateEnumNamesCode(EnumNameTypeAsset enumNameTypeAsset, string targetFolder)
        {
            var targetUnit = new CodeCompileUnit();
            var targetNamespace = new CodeNamespace(enumNameTypeAsset.namespaceName);

            // targetNamespace.Imports.Add(new CodeNamespaceImport("System"));

            var targetClass = new CodeTypeDeclaration(enumNameTypeAsset.className)
            {
                TypeAttributes = TypeAttributes.Public
            };

            for (var i = 0; i < enumNameTypeAsset.types.Count; i++)
            {
                var type = enumNameTypeAsset.types[i];
            
                var intValue = new CodeMemberField
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Static,
                    Name = type.name,
                    Type = new CodeTypeReference(typeof(int)),
                    InitExpression = new CodeSnippetExpression($"1 << {i}")
                };

                targetClass.Members.Add(intValue);
            }


            targetNamespace.Types.Add(targetClass);
            targetUnit.Namespaces.Add(targetNamespace);

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions
            {
                BlankLinesBetweenMembers = false,
                BracingStyle = "C",
            };

            var generatedClassFile = $"{enumNameTypeAsset.className}.cs";
        
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
        
            using (var sourceWriter = new StreamWriter(Path.Combine(targetFolder, generatedClassFile)))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }
        
            AssetDatabase.Refresh();
        }
    }
}