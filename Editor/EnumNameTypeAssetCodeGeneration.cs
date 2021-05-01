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

        private static List<EnumNameTypeAsset.TypeName> GetMaskTypes(List<EnumNameTypeAsset.TypeName> types, int value)
        {
            var list = new List<EnumNameTypeAsset.TypeName>();
            
            for (var i = 0; i < types.Count; i++)
            {
                var type = types[i];
                
                if (((1 << i) & value) != 0)
                {
                    list.Add(type);
                }
            }

            return list;
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
            
            for (var i = 0; i < enumNameTypeAsset.groupTypes.Count; i++)
            {
                var groupType = enumNameTypeAsset.groupTypes[i];
                var groupTypes = enumNameTypeAsset.GetMaskTypes(groupType.value);

                var strings = groupTypes.Select(t => t.name).ToArray();
                var value = string.Join(" | ", strings);

                var intValue = new CodeMemberField
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Static,
                    Name = groupType.name,
                    Type = new CodeTypeReference(typeof(int)),
                    InitExpression = new CodeSnippetExpression(value)
                };

                targetClass.Members.Add(intValue);
            }

            targetClass.Members.Add(new CodeMemberMethod()
            {
                Name = "MatchAny",
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Parameters =
                {
                    new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(int)), "a"),
                    new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(int)), "b")
                },
                ReturnType = new CodeTypeReference(typeof(bool)),
                Statements =
                {
                    new CodeSnippetExpression("return (a & b) != 0")
                }
            });

            targetNamespace.Types.Add(targetClass);
            targetUnit.Namespaces.Add(targetNamespace);

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions
            {
                BlankLinesBetweenMembers = true,
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