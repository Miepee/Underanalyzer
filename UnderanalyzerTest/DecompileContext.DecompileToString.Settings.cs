﻿using Underanalyzer.Decompiler;
using Underanalyzer.Decompiler.Warnings;

namespace UnderanalyzerTest;

public class DecompileContext_DecompileToString_Settings
{
    [Fact]
    public void TestDataLeftoverOnStack()
    {
        DecompileContext context = TestUtil.VerifyDecompileResult(
            """
            pushi.e 0
            """,
            """
            /// Decompiler warnings:
            // root: Data left over on VM stack at end of fragment (1 elements).
            """,
            null,
            new DecompileSettings()
            {
                AllowLeftoverDataOnStack = true
            }
        );
        Assert.Single(context.Warnings);
        Assert.IsType<DecompileDataLeftoverWarning>(context.Warnings[0]);
        Assert.Equal("root", context.Warnings[0].CodeEntryName);
        Assert.Equal(1, ((DecompileDataLeftoverWarning)context.Warnings[0]).NumberOfElements);
    }

    [Fact]
    public void TestSpaciousBranchesNoSemicolons()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            pushi.e 0
            pop.v.i self.a
            push.v self.a
            pushi.e 1
            cmp.i.v EQ
            bf [4]

            :[1]
            push.v self.b
            pushi.e 0
            cmp.i.v GT
            bf [3]

            :[2]
            b [1]

            :[3]
            b [4]

            :[4]
            pushi.e 1
            pop.v.i self.d
            """,
            """
            a = 0

            if (a == 1)
            {
                while (b > 0)
                {
                }
            }
            else
            {
            }

            d = 1
            """,
            null,
            new DecompileSettings()
            {
                EmptyLineAroundBranchStatements = true,
                UseSemicolon = false
            }
        );
    }

    [Fact]
    public void TestSpaciousBranchesAndSwitchBeforeCases()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            dup.v 0
            pushi.e 0
            cmp.i.v EQ
            bt [6]

            :[1]
            dup.v 0
            pushi.e 1
            cmp.i.v EQ
            bt [6]

            :[2]
            dup.v 0
            pushi.e 3
            cmp.i.v EQ
            bt [7]

            :[3]
            dup.v 0
            pushi.e 4
            cmp.i.v EQ
            bt [7]

            :[4]
            b [10]

            :[5]
            b [10]

            :[6]
            pushi.e 2
            pop.v.i self.b
            b [10]

            :[7]
            push.v self.c
            pushi.e 5
            cmp.i.v EQ
            bf [8]

            :[8]
            push.v self.d
            pushi.e 6
            cmp.i.v EQ
            bf [9]

            :[9]
            b [10]

            :[10]
            popz.v
            """,
            """
            switch (a)
            {
                case 0:
                case 1:
                    b = 2;
                    break;
                
                case 3:
                case 4:
                    if (c == 5)
                    {
                    }
                    
                    if (d == 6)
                    {
                    }
                    
                    break;
                
                default:
            }
            """,
            null,
            new DecompileSettings()
            {
                EmptyLineAroundBranchStatements = true,
                EmptyLineBeforeSwitchCases = true
            }
        );
    }

    [Fact]
    public void TestSpaciousBranchesAndSwitchBeforeAndAfterCases()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            dup.v 0
            pushi.e 0
            cmp.i.v EQ
            bt [6]

            :[1]
            dup.v 0
            pushi.e 1
            cmp.i.v EQ
            bt [6]

            :[2]
            dup.v 0
            pushi.e 3
            cmp.i.v EQ
            bt [7]

            :[3]
            dup.v 0
            pushi.e 4
            cmp.i.v EQ
            bt [7]

            :[4]
            b [10]

            :[5]
            b [10]

            :[6]
            pushi.e 2
            pop.v.i self.b
            b [10]

            :[7]
            push.v self.c
            pushi.e 5
            cmp.i.v EQ
            bf [8]

            :[8]
            push.v self.d
            pushi.e 6
            cmp.i.v EQ
            bf [9]

            :[9]
            b [10]

            :[10]
            popz.v
            """,
            """
            switch (a)
            {
                case 0:
                case 1:
                
                    b = 2;
                    break;
                
                case 3:
                case 4:
                
                    if (c == 5)
                    {
                    }
                    
                    if (d == 6)
                    {
                    }
                    
                    break;
                
                default:
            }
            """,
            null,
            new DecompileSettings()
            {
                EmptyLineAroundBranchStatements = true,
                EmptyLineBeforeSwitchCases = true,
                EmptyLineAfterSwitchCases = true
            }
        );
    }

    [Fact]
    public void TestSpaciousFunctionDeclsAndStatics()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            b [12]

            > example
            :[1]
            isstaticok.e
            bt [7]

            :[2]
            b [4]

            > a_sub_example
            :[3]
            exit.i

            :[4]
            push.i [function]a_sub_example
            conv.i.v
            pushi.e -16
            conv.i.v
            call.i method 2
            pop.v.v static.a
            b [6]

            > b_sub_example
            :[5]
            exit.i

            :[6]
            push.i [function]b_sub_example
            conv.i.v
            pushi.e -16
            conv.i.v
            call.i method 2
            pop.v.v static.b

            :[7]
            setstatic.e
            b [11]

            > struct_sub_example
            :[8]
            pushi.e 123
            pop.v.i self.d
            b [10]

            > anon_sub_struct_sub_example
            :[9]
            exit.i

            :[10]
            push.i [function]anon_sub_struct_sub_example
            conv.i.v
            pushi.e -1
            conv.i.v
            call.i method 2
            pop.v.v builtin.e
            exit.i

            :[11]
            push.i [function]struct_sub_example
            conv.i.v
            call.i @@NullObject@@ 0
            call.i method 2
            dup.v 0
            pushi.e -5
            pop.v.v [stacktop]global.___struct___0
            call.i @@NewGMLObject@@ 1
            pop.v.v self.c
            exit.i

            :[12]
            push.i [function]example
            conv.i.v
            call.i @@NullObject@@ 0
            call.i method 2
            dup.v 0
            pushi.e -1
            pop.v.v [stacktop]self.example
            popz.v
            b [14]

            > second
            :[13]
            exit.i

            :[14]
            push.i [function]second
            conv.i.v
            pushi.e -1
            conv.i.v
            call.i method 2
            dup.v 0
            pushi.e -1
            pop.v.v [stacktop]self.second
            popz.v
            """,
            """
            function example() constructor
            {
                static a = function()
                {
                };
                
                static b = function()
                {
                };
                
                c = 
                {
                    d: 123,
                    
                    e: function()
                    {
                    }
                };
            }
            
            function second()
            {
            }
            """,
            null,
            new DecompileSettings()
            {
                EmptyLineAroundFunctionDeclarations = true,
                EmptyLineAroundStaticInitialization = true
            }
        );
    }

    [Fact]
    public void TestOpenBraceOnSameLine()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v builtin.a
            conv.v.b
            bf [4]

            :[1]
            push.v builtin.b
            conv.v.b
            bf [3]

            :[2]
            pushi.e 1
            pop.v.i builtin.c
            b [1]

            :[3]
            b [10]

            :[4]
            push.v builtin.d
            conv.v.b
            bf [7]

            :[5]
            push.v builtin.e
            conv.v.b
            bf [5]

            :[6]
            b [10]

            :[7]
            b [9]

            > struct
            :[8]
            pushi.e 123
            pop.v.i self.g
            exit.i

            :[9]
            push.i [function]struct
            conv.i.v
            call.i @@NullObject@@ 0
            call.i method 2
            dup.v 0
            pushi.e -5
            pop.v.v [stacktop]global.___struct___0
            call.i @@NewGMLObject@@ 1
            pop.v.v builtin.f

            :[10]
            """,
            """
            if (a) {
                while (b) {
                    c = 1;
                }
            } else if (d) {
                do {
                } until (e);
            } else {
                f = {
                    g: 123
                };
            }
            """,
            null,
            new DecompileSettings()
            {
                OpenBlockBraceOnSameLine = true
            }
        );
    }

    [Fact]
    public void TestOpenBraceOnSameLineAndSpacious()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.i 100
            conv.i.v
            push.i 52
            conv.i.v
            call.i @@try_hook@@ 2
            popz.v
            pushi.e 1
            pop.v.i builtin.a
            b [2]

            :[1]
            pop.v.v local.ex
            call.i @@try_unhook@@ 0
            popz.v
            pushi.e 1
            pop.v.i builtin.b
            call.i @@finish_catch@@ 0
            popz.v
            b [3]

            :[2]
            call.i @@try_unhook@@ 0
            popz.v

            :[3]
            pushi.e 1
            pop.v.i builtin.c
            call.i @@finish_finally@@ 0
            popz.v
            b [4]

            :[4]
            b [10]

            > fun
            :[5]
            push.v builtin.d
            conv.v.b
            bf [7]

            :[6]
            pushi.e 1
            pop.v.i builtin.e

            :[7]
            push.v builtin.f
            conv.v.b
            bf [9]

            :[8]
            pushi.e 1
            pop.v.i builtin.g

            :[9]
            exit.i

            :[10]
            push.i [function]fun
            conv.i.v
            pushi.e -1
            conv.i.v
            call.i method 2
            dup.v 0
            pushi.e -1
            pop.v.v [stacktop]self.fun
            popz.v
            """,
            """
            try {
                a = 1;
            } catch (ex) {
                b = 1;
            } finally {
                c = 1;
            }

            function fun() {
                if (d) {
                    e = 1;
                }
                
                if (f) {
                    g = 1;
                }
            }
            """,
            null,
            new DecompileSettings()
            {
                OpenBlockBraceOnSameLine = true,
                EmptyLineAroundBranchStatements = true
            }
        );
    }
}
