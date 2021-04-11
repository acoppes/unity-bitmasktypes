using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using UnityEditor;

public static class TestCodeGeneration 
{
    [MenuItem("Gemserk/Generate Code")]
    public static void GenerateCode()
    {
        var targetUnit = new CodeCompileUnit();
        var targetNamespace = new CodeNamespace("GeneratedCode");
        targetNamespace.Imports.Add(new CodeNamespaceImport("System"));

        var targetClass = new CodeTypeDeclaration("GeneratedClass")
        {
            IsClass = true, 
            TypeAttributes = TypeAttributes.Public
        };
        
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
