using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;

public static class TestCodeGeneration 
{
    // private static CodeFieldReferenceExpression createEnumVar() {
    //     var e = new CodeFieldReferenceExpression(
    //             new CodeTypeReferenceExpression(
    //                 typeof(int)
    //             ),
    //             "Magenta");
    //     return e;
    // }

    [MenuItem("Gemserk/Generate Code")]
    public static void GenerateCode()
    {
        GenerateEnumCode(new List<string>()
        {
            "IceDamage", "FireDamage", "PoisonDamage"
        });
    }

    private static void GenerateEnumCode(IReadOnlyList<string> typesInOrder)
    {
        var targetUnit = new CodeCompileUnit();
        var targetNamespace = new CodeNamespace("GeneratedCode");
        targetNamespace.Imports.Add(new CodeNamespaceImport("System"));

        var targetClass = new CodeTypeDeclaration("GeneratedEnum")
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

        var generatedClassFolder = "Assets/GeneratedCode";
        var generatedClassFile = "GeneratedClass.cs";
        
        // Path.Combine(Directory.GetCurrentDirectory(), generatedClassFolder)

        if (!Directory.Exists(generatedClassFolder))
        {
            Directory.CreateDirectory(generatedClassFolder);
        }
        
        using (var sourceWriter = new StreamWriter(Path.Combine(generatedClassFolder, generatedClassFile)))
        {
            provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
        }
        
        AssetDatabase.Refresh();
    }
}
