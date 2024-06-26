﻿using System;
using System.Collections.Generic;
using Underanalyzer.Decompiler.ControlFlow;

namespace Underanalyzer.Decompiler.AST;

/// <summary>
/// Represents a single fragment context within the AST.
/// </summary>
public class ASTFragmentContext
{
    /// <summary>
    /// The fragment we belong to.
    /// </summary>
    private Fragment Fragment { get; }

    /// <summary>
    /// The name of the code entry this fragment belongs to.
    /// </summary>
    public string CodeEntryName { get => Fragment.CodeEntry.Name?.Content; }

    /// <summary>
    /// The name of the function this fragment belongs to, or null if none.
    /// </summary>
    public string FunctionName { get; internal set; } = null;

    /// <summary>
    /// Children of this fragment, e.g. sub-functions.
    /// </summary>
    internal List<ASTFragmentContext> Children { get; } = new();
    
    /// <summary>
    /// Current working VM expression stack.
    /// </summary>
    internal Stack<IExpressionNode> ExpressionStack { get; } = new();

    /// <summary>
    /// If true, the current AST is within a static variable initialization block (when printing).
    /// </summary>
    internal bool InStaticInitialization { get; set; } = false;

    /// <summary>
    /// If not null, represents the list of arguments getting passed into this fragment (which is a struct).
    /// </summary>
    public List<IExpressionNode> StructArguments { get; internal set; } = null;

    /// <summary>
    /// Function call to the parent constructor function, if this is a constructor function that inherits
    /// another constructor function, or null otherwise.
    /// </summary>
    internal IExpressionNode BaseParentCall { get; set; } = null;

    /// <summary>
    /// Contains all local variables referenced from within this fragment.
    /// </summary>
    public HashSet<string> LocalVariableNames { get; } = new();

    /// <summary>
    /// Contains all local variables referenced from within this fragment, in order of occurrence.
    /// </summary>
    public List<string> LocalVariableNamesList { get; } = new();

    /// <summary>
    /// Map of code entry names to function names, for all children fragments/sub-functions of this context.
    /// </summary>
    public Dictionary<string, string> SubFunctionNames { get; } = new();

    /// <summary>
    /// The loop surrounding the currently-building position in the AST.
    /// </summary>
    internal Loop SurroundingLoop { get; set; } = null;

    /// <summary>
    /// Contains local variable names that should be entirely removed from the fragment. 
    /// (For removing compiler-generated code.)
    /// </summary>
    internal HashSet<string> LocalVariablesToPurge { get; } = new();

    /// <summary>
    /// Stack of the number of statements contained in all enveloping try finally blocks.
    /// </summary>
    internal Stack<int> FinallyStatementCount { get; set; } = new();

    internal ASTFragmentContext(Fragment fragment)
    {
        Fragment = fragment;
    }

    internal void RemoveLocal(string name)
    {
        if (LocalVariableNames.Contains(name))
        {
            LocalVariableNames.Remove(name);
            LocalVariableNamesList.Remove(name);
        }
    }
}
