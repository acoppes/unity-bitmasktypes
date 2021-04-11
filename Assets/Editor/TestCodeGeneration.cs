using System.CodeDom;
using System.CodeDom.Compiler;
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
        var targetUnit = new CodeCompileUnit();
        var targetNamespace = new CodeNamespace("GeneratedCode");
        targetNamespace.Imports.Add(new CodeNamespaceImport("System"));

        var targetClass = new CodeTypeDeclaration("GeneratedEnum")
        {
            IsEnum = true,
            TypeAttributes = TypeAttributes.Public
        };

        // var enumField = new CodeMemberField("GeneratedEnum", "FirstValue");

        var initExpression = new CodeSnippetExpression ("1 << 0");
        
        var enumValue = new CodeMemberField
        {
            Attributes = MemberAttributes.Public,
            Name = "FirstValue", 
            InitExpression = initExpression
            // Type = new CodeTypeReference(typeof(System.Double))
        };
        
        // widthValueField.Comments.Add(new CodeCommentStatement(
        //     "The width of the object."));
        
        targetClass.Members.Add(enumValue);
        
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
