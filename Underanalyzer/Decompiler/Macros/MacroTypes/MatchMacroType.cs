﻿using Underanalyzer.Decompiler.AST;

namespace Underanalyzer.Decompiler.Macros;

/// <summary>
/// Conditional for matching an AST node by type name and/or by value.
/// </summary>
public class MatchMacroType : ConditionalMacroType
{
    /// <summary>
    /// Type name to match.
    /// </summary>
    public string ConditionalTypeName { get; }

    /// <summary>
    /// Value content to match, or null if none.
    /// </summary>
    public string ConditionalValue { get; }

    public MatchMacroType(IMacroType innerType, string typeName, string value = null) : base(innerType)
    {
        ConditionalTypeName = typeName;
        ConditionalValue = value;
    }

    public override bool EvaluateCondition(ASTCleaner cleaner, IConditionalValueNode node)
    {
        if (ConditionalValue is not null && node.ConditionalValue != ConditionalValue)
        {
            return false;
        }
        return node.ConditionalTypeName == ConditionalTypeName;
    }
}
