using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;

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
            
            for (var i = 0; i < enumNameTypeAsset.groupTypes.Count; i++)
            {
                var groupType = enumNameTypeAsset.groupTypes[i];
                var value = enumNameTypeAsset.MaskToString(groupType.value);
                
                // var groupTypes = asset.GetMaskTypes(groupType.value);
                // var strings = groupTypes.Select(t => t.name).ToArray();
                // var value = string.Join(" | ", strings);

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
        
        public static void GenerateTypeSetClass(TypeSetAsset asset, string targetFolder)
        {
            var targetUnit = new CodeCompileUnit();
            var targetNamespace = new CodeNamespace(asset.namespaceName);

            // targetNamespace.Imports.Add(new CodeNamespaceImport("System"));

            var targetClass = new CodeTypeDeclaration(asset.className)
            {
                TypeAttributes = TypeAttributes.Public,
                IsPartial = asset.partialClass
            };
            
            for (var i = 0; i < asset.types.Count; i++)
            {
                var type = asset.types[i];

                CodeExpression initExpression = null;

                if (type is IntTypeAsset intAsset)
                {
                    if (asset.isBitmask)
                    {
                        var shiftValue = (int) Math.Round(Math.Log(intAsset.value, 2));
                        initExpression = new CodeSnippetExpression($"1 << {shiftValue}");
                    }
                    else
                    {
                        initExpression = new CodeSnippetExpression($"{intAsset.value}");
                    }
                } else if (type is BitmaskTypeAsset bitmaskAsset)
                {
                    var shiftValue = (int) Math.Round(Math.Log(bitmaskAsset.type, 2));
                    initExpression = new CodeSnippetExpression($"1 << {shiftValue}");
                }
            
                var intValue = new CodeMemberField
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Static,
                    Name = type.name,
                    Type = new CodeTypeReference(typeof(int)),
                    InitExpression = initExpression
                    // InitExpression = new CodeSnippetExpression($"{type.type}")
                };

                targetClass.Members.Add(intValue);
            }

            {
                targetClass.Members.Add(new CodeMemberField
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Const,
                    Name = "TOTAL_TYPES",
                    Type = new CodeTypeReference(typeof(int)),
                    InitExpression = new CodeSnippetExpression($"{asset.types.Count}")
                });
            }
            
            var nameOfValueStaticMethod = new CodeMemberMethod()
            {
                Name = "ValueToName",
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Parameters =
                {
                    new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(int)), "value")
                },
                ReturnType = new CodeTypeReference(typeof(string))
            };

            asset.types.ForEach(t =>
            {
                nameOfValueStaticMethod.Statements.Add(new CodeSnippetExpression($"if (value == {t.name}) return nameof({t.name})"));
            });
            nameOfValueStaticMethod.Statements.Add(new CodeSnippetExpression("return null"));
            
            targetClass.Members.Add(nameOfValueStaticMethod);
            
            var getNamesStaticMethod = new CodeMemberMethod()
            {
                Name = "GetNames",
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Parameters =
                {
                    new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(int)), "mask"),
                    new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(ICollection<string>)), "collection")
                },
                // ReturnType = new CodeTypeReference(typeof(void))
            };

            asset.types.ForEach(t =>
            {
               //  new CodeConditionStatement(new CodeExpression(), new CodeSnippetStatement());
               if (t is IntTypeAsset)
               {
                   getNamesStaticMethod.Statements.Add(new CodeSnippetExpression($"if ((mask & (1 << {t.name})) == (1 << {t.name})) collection.Add(nameof({t.name}))"));
               } else if (t is BitmaskTypeAsset b)
               {
                   getNamesStaticMethod.Statements.Add(new CodeSnippetExpression($"if ((mask & {t.name}) == {t.name}) collection.Add(nameof({t.name}))"));
               }
                
            });
            
            targetClass.Members.Add(getNamesStaticMethod);

            targetNamespace.Types.Add(targetClass);
            targetUnit.Namespaces.Add(targetNamespace);

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions
            {
                BlankLinesBetweenMembers = true,
                BracingStyle = "C"
            };

            var generatedClassFile = $"{asset.className}.cs";

            if (asset.appendGeneratedToFileName)
            {
                generatedClassFile = $"{asset.className}.Generated.cs";
            }
        
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