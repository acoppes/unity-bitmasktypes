using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Gemserk.BitmaskTypes;
using Gemserk.Examples;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public static class TestCodeGeneration 
{
    [MenuItem("Gemserk/Generate Code")]
    public static void GenerateCode()
    {
        // GenerateEnumMaskCode("GeneratedCode", "GeneratedEnum", "Assets/GeneratedCode", new List<string>()
        // {
        //     "IceDamage", "FireDamage", "PoisonDamage"
        // });

        // GenerateEnumMaskCode(typeof(DamageTypeAsset));

        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in allAssemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(BaseTypeAsset)))
                {
                    GenerateEnumMaskCode(type);
                }
            }
        }
    }

    public static void GenerateEnumMaskCode(Type t)
    {
        var typeFolder = AssetDatabase.FindAssets($"t:TextAsset {t.Name}")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(p => p.EndsWith($"{t.Name}.cs"))
            .Select(Path.GetDirectoryName)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(typeFolder))
        {
            Debug.Log($"Couldn't find folder for type {t.Name}");
            return;
        }
        
        var types = AssetDatabase.FindAssets($"t:{t.Name}")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<BaseTypeAsset>)
            .ToList();
        
        types.Sort((a, b) => a.bitmaskValue.CompareTo(b.bitmaskValue));
        
        GenerateEnumMaskCode(t.Namespace, $"{t.Name}Enum", typeFolder, 
            types.Select(t => t.name).ToList());
    }

    public static void GenerateEnumMaskCode(string namespaceName, string enumName, string targetFolder,
        IReadOnlyList<string> typesInOrder)
    {
        var targetUnit = new CodeCompileUnit();
        var targetNamespace = new CodeNamespace(namespaceName);

        // targetNamespace.Imports.Add(new CodeNamespaceImport("System"));

        var targetClass = new CodeTypeDeclaration(enumName)
        {
            IsEnum = true,
            TypeAttributes = TypeAttributes.Public
        };

        for (var i = 0; i < typesInOrder.Count; i++)
        {
            var type = typesInOrder[i];
            
            var enumValue = new CodeMemberField
            {
                Attributes = MemberAttributes.Public,
                Name = type,
                InitExpression = new CodeSnippetExpression($"1 << {i}")
            };

            // enumValue.Comments.Add(new CodeCommentStatement($"Corresponding bitmask for {type}"));

            targetClass.Members.Add(enumValue);
        }


        targetNamespace.Types.Add(targetClass);
        targetUnit.Namespaces.Add(targetNamespace);

        var provider = CodeDomProvider.CreateProvider("CSharp");
        var options = new CodeGeneratorOptions
        {
            BracingStyle = "C"
        };

        var generatedClassFile = $"{enumName}.cs";
        
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
