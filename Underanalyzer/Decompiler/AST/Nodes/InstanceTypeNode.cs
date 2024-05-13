﻿using System;

namespace Underanalyzer.Decompiler.AST;

/// <summary>
/// Represents an instance type (<see cref="IGMInstruction.InstanceType"/>) in the AST.
/// </summary>
public class InstanceTypeNode : IExpressionNode
{
    /// <summary>
    /// The instance type for this node.
    /// </summary>
    public IGMInstruction.InstanceType InstanceType { get; }

    public bool Duplicated { get; set; } = false;
    public bool Group { get; set; } = false;
    public IGMInstruction.DataType StackType { get; set; } = IGMInstruction.DataType.Int32;

    public InstanceTypeNode(IGMInstruction.InstanceType instType)
    {
        InstanceType = instType;
    }

    public IExpressionNode Clean(ASTCleaner cleaner)
    {
        return this;
    }

    public void Print(ASTPrinter printer)
    {
        printer.Write(InstanceType switch
        {
            IGMInstruction.InstanceType.Self => "self",
            IGMInstruction.InstanceType.Other => "other",
            IGMInstruction.InstanceType.All => "all",
            IGMInstruction.InstanceType.Noone => "noone",
            IGMInstruction.InstanceType.Global => "global",
            _ => throw new DecompilerException($"Printing unknown instance type {InstanceType}")
        });
    }
}
