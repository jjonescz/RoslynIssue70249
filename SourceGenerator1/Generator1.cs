using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator1;

[Generator(LanguageNames.CSharp)]
public partial class Generator1 : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static (context) =>
        {
            context.AddSource("GeneratedClass1.cs", """
                /// <summary>Generated docs.</summary>
                class GeneratedClass1 { }
                """);
        });

        var classNames = context.SyntaxProvider.CreateSyntaxProvider(
            static (n, _) => n is ClassDeclarationSyntax,
            static (c, _) => ((ClassDeclarationSyntax)c.Node).Identifier.Text);

        context.RegisterSourceOutput(classNames, static (context, className) =>
        {
            context.AddSource($"GeneratedWrapper_{className}.cs", $$"""
                /// <summary>Generated wrapper for {{className}}.</summary>
                class GeneratedWrapper_{{className}} { }
                """);
        });
    }
}
