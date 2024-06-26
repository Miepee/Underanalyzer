﻿using System;
using Underanalyzer.Decompiler.Macros;
using static Underanalyzer.IGMInstruction;

namespace Underanalyzer.Decompiler.AST;

/// <summary>
/// Represents a binary expression, such as basic two-operand arithmetic operations.
/// </summary>
public class BinaryNode : IExpressionNode, IMacroResolvableNode, IConditionalValueNode
{
    /// <summary>
    /// Left side of the binary operation.
    /// </summary>
    public IExpressionNode Left { get; private set; }

    /// <summary>
    /// Right side of the binary operation.
    /// </summary>
    public IExpressionNode Right { get; private set; }
    
    /// <summary>
    /// The instruction that performs this operation, as in the code.
    /// </summary>
    public IGMInstruction Instruction { get; }

    public bool Duplicated { get; set; } = false;
    public bool Group { get; set; } = false;
    public IGMInstruction.DataType StackType { get; set; }

    public string ConditionalTypeName => "Binary";
    public string ConditionalValue => ""; // TODO?

    public BinaryNode(IExpressionNode left, IExpressionNode right, IGMInstruction instruction)
    {
        Left = left;
        Right = right;  
        Instruction = instruction;

        // Type1 and Type2 on the instruction represent the data types of Left and Right on the stack.
        // Choose whichever type has a higher bias, or if equal, the smaller numerical data type value.
        int bias1 = StackTypeBias(instruction.Type1);
        int bias2 = StackTypeBias(instruction.Type2);
        if (bias1 == bias2)
        {
            StackType = (DataType)Math.Min((byte)instruction.Type1, (byte)instruction.Type2);
        }
        else
        {
            StackType = (bias1 > bias2) ? instruction.Type1 : instruction.Type2;
        }
    }

    private int StackTypeBias(DataType type)
    {
        return type switch
        {
            DataType.Int32 or DataType.Boolean or DataType.String => 0,
            DataType.Double or DataType.Int64 => 1,
            DataType.Variable => 2,
            _ => throw new DecompilerException("Unknown stack type in binary operation")
        };
    }

    private void CheckGroup(IExpressionNode node)
    {
        // TODO: verify that this works for all cases
        if (node is BinaryNode binary)
        {
            if (binary.Instruction.Kind != Instruction.Kind)
            {
                binary.Group = true;
            }
            if (binary.Instruction.Kind == Opcode.Compare && binary.Instruction.ComparisonKind != Instruction.ComparisonKind)
            {
                binary.Group = true;
            }
        }
        else if (node is ShortCircuitNode or ConditionalNode or NullishCoalesceNode)
        {
            node.Group = true;
        }
    }

    public IExpressionNode Clean(ASTCleaner cleaner)
    {
        Left = Left.Clean(cleaner);
        Right = Right.Clean(cleaner);

        // Resolve macro types carrying between left and right nodes
        if (Left is IMacroTypeNode leftTypeNode && Right is IMacroResolvableNode rightResolvableNode &&
            leftTypeNode.GetExpressionMacroType(cleaner) is IMacroType leftMacroType &&
            rightResolvableNode.ResolveMacroType(cleaner, leftMacroType) is IExpressionNode rightResolved)
        {
            Right = rightResolved;
        }
        else if (Right is IMacroTypeNode rightTypeNode && Left is IMacroResolvableNode leftResolvableNode &&
                 rightTypeNode.GetExpressionMacroType(cleaner) is IMacroType rightMacroType &&
                 leftResolvableNode.ResolveMacroType(cleaner, rightMacroType) is IExpressionNode leftResolved)
        {
            Left = leftResolved;
        }

        CheckGroup(Left);
        CheckGroup(Right);

        return this;
    }

    public void Print(ASTPrinter printer)
    {
        if (Group)
        {
            printer.Write('(');
        }

        Left.Print(printer);

        string op = Instruction switch
        {
            { Kind: Opcode.Add } => " + ",
            { Kind: Opcode.Subtract } => " - ",
            { Kind: Opcode.Multiply } => " * ",
            { Kind: Opcode.Divide } => " / ",
            { Kind: Opcode.GMLDivRemainder } => " div ",
            { Kind: Opcode.GMLModulo } => " % ",
            { Kind: Opcode.And, Type1: DataType.Boolean, Type2: DataType.Boolean } => " && ",
            { Kind: Opcode.And } => " & ",
            { Kind: Opcode.Or, Type1: DataType.Boolean, Type2: DataType.Boolean } => " || ",
            { Kind: Opcode.Or } => " | ",
            { Kind: Opcode.Xor, Type1: DataType.Boolean, Type2: DataType.Boolean } => " ^^ ",
            { Kind: Opcode.Xor } => " ^ ",
            { Kind: Opcode.ShiftLeft } => " << ",
            { Kind: Opcode.ShiftRight } => " >> ",
            { Kind: Opcode.Compare, ComparisonKind: ComparisonType.Lesser } => " < ",
            { Kind: Opcode.Compare, ComparisonKind: ComparisonType.LesserEqual } => " <= ",
            { Kind: Opcode.Compare, ComparisonKind: ComparisonType.Equal } => " == ",
            { Kind: Opcode.Compare, ComparisonKind: ComparisonType.NotEqual } => " != ",
            { Kind: Opcode.Compare, ComparisonKind: ComparisonType.GreaterEqual } => " >= ",
            { Kind: Opcode.Compare, ComparisonKind: ComparisonType.Greater } => " > ",
            _ => throw new DecompilerException("Failed to match binary instruction to string")
        };
        printer.Write(op);

        Right.Print(printer);

        if (Group)
        {
            printer.Write(')');
        }
    }

    public IExpressionNode ResolveMacroType(ASTCleaner cleaner, IMacroType type)
    {
        bool didAnything = false;

        if (Left is IMacroResolvableNode leftResolvable &&
            leftResolvable.ResolveMacroType(cleaner, type) is IExpressionNode leftResolved)
        {
            Left = leftResolved;
            didAnything = true;
        }
        if (Right is IMacroResolvableNode rightResolvable &&
            rightResolvable.ResolveMacroType(cleaner, type) is IExpressionNode rightResolved)
        {
            Right = rightResolved;
            didAnything = true;
        }

        return didAnything ? this : null;
    }
}
