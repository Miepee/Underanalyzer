﻿namespace UnderanalyzerTest;

public class DecompileContext_DecompileToString
{
    [Fact]
    public void TestBasic()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            pushi.e 123
            pop.v.i self.a
            push.v self.b
            conv.v.b
            bf [2]

            :[1]
            push.s "B is true"
            pop.v.s self.msg
            b [7]

            :[2]
            push.v self.c
            conv.v.b
            bf [4]

            :[3]
            push.v self.d
            conv.v.b
            b [5]

            :[4]
            push.e 0

            :[5]
            bf [7]

            :[6]
            push.s "C and D are both true"
            pop.v.s self.msg

            :[7]
            """,
            """
            a = 123;
            if (b)
            {
                msg = "B is true";
            }
            else if (c && d)
            {
                msg = "C and D are both true";
            }
            """
        );
    }

    [Fact]
    public void TestWhileIfElseEmpty()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            conv.v.b
            bf [4]

            :[1]
            push.v self.b
            conv.v.b
            bf [3]

            :[2]
            b [3]

            :[3]
            b [0]

            :[4]
            """,
            """
            while (a)
            {
                if (b)
                {
                }
                else
                {
                }
            }
            """
        );
    }

    [Fact]
    public void TestNestedDoUntil()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.c
            push.v self.d
            add.v.v
            pushi.e 2
            conv.i.d
            div.d.v
            pop.v.v self.b
            push.v self.b
            pushi.e 200
            cmp.i.v GT
            bf [0]

            :[1]
            push.v self.a
            pushi.e 1
            add.i.v
            pop.v.v self.a
            push.v self.a
            pushi.e 100
            cmp.i.v GT
            bf [0]
            """,
            """
            do
            {
                do
                {
                    b = (c + d) / 2;
                }
                until (b > 200);
                a += 1;
            }
            until (a > 100);
            """
        );
    }

    [Fact]
    public void TestBasicSwitch()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            dup.v 0
            pushi.e 1
            cmp.i.v EQ
            bt [5]

            :[1]
            dup.v 0
            pushi.e 2
            cmp.i.v EQ
            bt [7]

            :[2]
            dup.v 0
            pushi.e 3
            cmp.i.v EQ
            bt [7]

            :[3]
            b [6]

            :[4]
            b [8]

            :[5]
            push.s "Case 1"
            pop.v.s self.msg
            b [8]

            :[6]
            push.s "Default"
            pop.v.s self.msg
            b [8]

            :[7]
            push.s "Case 2 and 3"
            pop.v.s self.msg
            b [8]

            :[8]
            popz.v
            """,
            """
            switch (a)
            {
                case 1:
                    msg = "Case 1";
                    break;
                default:
                    msg = "Default";
                    break;
                case 2:
                case 3:
                    msg = "Case 2 and 3";
                    break;
            }
            """
        );
    }

    [Fact]
    public void TestPrePostfix()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.b
            dup.v 0
            push.e 1
            add.i.v
            pop.v.v self.b
            pop.v.v self.a
            push.v self.b
            push.e 1
            add.i.v
            dup.v 0
            pop.v.v self.b
            pop.v.v self.a
            push.v self.b
            conv.v.i
            push.v [stacktop]self.c
            conv.v.i
            dup.i 0
            push.v [stacktop]self.d
            dup.v 0
            pop.e.v 5
            push.e 1
            add.i.v
            pop.i.v [stacktop]self.d
            pop.v.v self.a
            push.v self.b
            conv.v.i
            push.v [stacktop]self.c
            conv.v.i
            dup.i 0
            push.v [stacktop]self.d
            push.e 1
            add.i.v
            dup.v 0
            pop.e.v 5
            pop.i.v [stacktop]self.d
            pop.v.v self.a
            pushi.e -1
            pushi.e 0
            dup.l 0
            push.v [array]self.b
            dup.v 0
            pop.e.v 6
            push.e 1
            add.i.v
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            pushi.e 0
            dup.l 0
            push.v [array]self.b
            push.e 1
            add.i.v
            dup.v 0
            pop.e.v 6
            pop.i.v [array]self.b
            pop.v.v self.a
            push.v self.a
            conv.v.i
            push.v [stacktop]self.b
            conv.v.i
            pushi.e 0
            dup.l 0
            push.v [array]self.c
            dup.v 0
            pop.e.v 6
            push.e 1
            add.i.v
            pop.i.v [array]self.c
            pop.v.v self.a
            push.v self.a
            conv.v.i
            push.v [stacktop]self.b
            conv.v.i
            pushi.e 0
            dup.l 0
            push.v [array]self.c
            push.e 1
            add.i.v
            dup.v 0
            pop.e.v 6
            pop.i.v [array]self.c
            pop.v.v self.a
            push.v self.a
            conv.v.i
            pushi.e 0
            push.v [array]self.b
            conv.v.i
            dup.i 0
            push.v [stacktop]self.c
            dup.v 0
            pop.e.v 5
            push.e 1
            add.i.v
            pop.i.v [stacktop]self.c
            pop.v.v self.a
            push.v self.a
            conv.v.i
            pushi.e 0
            push.v [array]self.b
            conv.v.i
            dup.i 0
            push.v [stacktop]self.c
            push.e 1
            add.i.v
            dup.v 0
            pop.e.v 5
            pop.i.v [stacktop]self.c
            pop.v.v self.a
            push.v self.a
            conv.v.i
            pushi.e 0
            push.v [array]self.b
            conv.v.i
            pushi.e 0
            dup.l 0
            push.v [array]self.c
            dup.v 0
            pop.e.v 6
            push.e 1
            add.i.v
            pop.i.v [array]self.c
            pop.v.v self.a
            push.v self.a
            conv.v.i
            pushi.e 0
            push.v [array]self.b
            conv.v.i
            pushi.e 0
            dup.l 0
            push.v [array]self.c
            push.e 1
            add.i.v
            dup.v 0
            pop.e.v 6
            pop.i.v [array]self.c
            pop.v.v self.a
            pushi.e -1
            push.v self.c
            dup.v 0
            push.e 1
            add.i.v
            pop.v.v self.c
            conv.v.i
            dup.l 0
            push.v [array]self.b
            dup.v 0
            pop.e.v 6
            push.e 1
            add.i.v
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            push.v self.c
            push.e 1
            add.i.v
            dup.v 0
            pop.v.v self.c
            conv.v.i
            dup.l 0
            push.v [array]self.b
            dup.v 0
            pop.e.v 6
            push.e 1
            add.i.v
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            push.v self.c
            push.e 1
            add.i.v
            dup.v 0
            pop.v.v self.c
            conv.v.i
            dup.l 0
            push.v [array]self.b
            push.e 1
            add.i.v
            dup.v 0
            pop.e.v 6
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            push.v self.c
            dup.v 0
            push.e 1
            add.i.v
            pop.v.v self.c
            conv.v.i
            dup.l 0
            push.v [array]self.b
            push.e 1
            add.i.v
            dup.v 0
            pop.e.v 6
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            pushi.e -1
            pushi.e 0
            dup.l 0
            push.v [array]self.c
            dup.v 0
            pop.e.v 6
            push.e 1
            add.i.v
            pop.i.v [array]self.c
            conv.v.i
            dup.l 0
            push.v [array]self.b
            dup.v 0
            pop.e.v 6
            push.e 1
            add.i.v
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            pushi.e -1
            pushi.e 0
            dup.l 0
            push.v [array]self.c
            push.e 1
            add.i.v
            dup.v 0
            pop.e.v 6
            pop.i.v [array]self.c
            conv.v.i
            dup.l 0
            push.v [array]self.b
            dup.v 0
            pop.e.v 6
            push.e 1
            add.i.v
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            pushi.e -1
            pushi.e 0
            dup.l 0
            push.v [array]self.c
            push.e 1
            add.i.v
            dup.v 0
            pop.e.v 6
            pop.i.v [array]self.c
            conv.v.i
            dup.l 0
            push.v [array]self.b
            push.e 1
            add.i.v
            dup.v 0
            pop.e.v 6
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            pushi.e -1
            pushi.e 0
            dup.l 0
            push.v [array]self.c
            dup.v 0
            pop.e.v 6
            push.e 1
            add.i.v
            pop.i.v [array]self.c
            conv.v.i
            dup.l 0
            push.v [array]self.b
            push.e 1
            add.i.v
            dup.v 0
            pop.e.v 6
            pop.i.v [array]self.b
            pop.v.v self.a
            """,
            """
            a = b++;
            a = ++b;
            a = b.c.d++;
            a = ++b.c.d;
            a = b[0]++;
            a = ++b[0];
            a = a.b.c[0]++;
            a = ++a.b.c[0];
            a = a.b[0].c++;
            a = ++a.b[0].c;
            a = a.b[0].c[0]++;
            a = ++a.b[0].c[0];
            a = b[c++]++;
            a = b[++c]++;
            a = ++b[++c];
            a = ++b[c++];
            a = b[c[0]++]++;
            a = b[++c[0]]++;
            a = ++b[++c[0]];
            a = ++b[c[0]++];
            """
        );
    }

    [Fact]
    public void TestPrePostfix_GMLv2()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.b
            dup.v 0
            push.e 1
            add.i.v
            pop.v.v self.b
            pop.v.v self.a
            push.v self.b
            push.e 1
            add.i.v
            dup.v 0
            pop.v.v self.b
            pop.v.v self.a
            push.v self.b
            pushi.e -9
            push.v [stacktop]self.c
            pushi.e -9
            dup.i 4
            push.v [stacktop]self.d
            dup.v 0
            dup.i 4 9
            push.e 1
            add.i.v
            pop.i.v [stacktop]self.d
            pop.v.v self.a
            push.v self.b
            pushi.e -9
            push.v [stacktop]self.c
            pushi.e -9
            dup.i 4
            push.v [stacktop]self.d
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 9
            pop.i.v [stacktop]self.d
            pop.v.v self.a
            pushi.e -1
            pushi.e 0
            dup.i 1
            push.v [array]self.b
            dup.v 0
            dup.i 4 6
            push.e 1
            add.i.v
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            pushi.e 0
            dup.i 1
            push.v [array]self.b
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 6
            pop.i.v [array]self.b
            pop.v.v self.a
            push.v self.a
            pushi.e -9
            push.v [stacktop]self.b
            pushi.e -9
            pushi.e 0
            dup.i 5
            push.v [array]self.c
            dup.v 0
            dup.i 4 10
            push.e 1
            add.i.v
            pop.i.v [array]self.c
            pop.v.v self.a
            push.v self.a
            pushi.e -9
            push.v [stacktop]self.b
            pushi.e -9
            pushi.e 0
            dup.i 5
            push.v [array]self.c
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 10
            pop.i.v [array]self.c
            pop.v.v self.a
            push.v self.a
            pushi.e -9
            pushi.e 0
            push.v [array]self.b
            pushi.e -9
            dup.i 4
            push.v [stacktop]self.c
            dup.v 0
            dup.i 4 9
            push.e 1
            add.i.v
            pop.i.v [stacktop]self.c
            pop.v.v self.a
            push.v self.a
            pushi.e -9
            pushi.e 0
            push.v [array]self.b
            pushi.e -9
            dup.i 4
            push.v [stacktop]self.c
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 9
            pop.i.v [stacktop]self.c
            pop.v.v self.a
            push.v self.a
            pushi.e -9
            pushi.e 0
            push.v [array]self.b
            pushi.e -9
            pushi.e 0
            dup.i 5
            push.v [array]self.c
            dup.v 0
            dup.i 4 10
            push.e 1
            add.i.v
            pop.i.v [array]self.c
            pop.v.v self.a
            push.v self.a
            pushi.e -9
            pushi.e 0
            push.v [array]self.b
            pushi.e -9
            pushi.e 0
            dup.i 5
            push.v [array]self.c
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 10
            pop.i.v [array]self.c
            pop.v.v self.a
            pushi.e -1
            push.v self.c
            dup.v 0
            push.e 1
            add.i.v
            pop.v.v self.c
            conv.v.i
            dup.i 1
            push.v [array]self.b
            dup.v 0
            dup.i 4 6
            push.e 1
            add.i.v
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            push.v self.c
            push.e 1
            add.i.v
            dup.v 0
            pop.v.v self.c
            conv.v.i
            dup.i 1
            push.v [array]self.b
            dup.v 0
            dup.i 4 6
            push.e 1
            add.i.v
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            push.v self.c
            push.e 1
            add.i.v
            dup.v 0
            pop.v.v self.c
            conv.v.i
            dup.i 1
            push.v [array]self.b
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 6
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            push.v self.c
            dup.v 0
            push.e 1
            add.i.v
            pop.v.v self.c
            conv.v.i
            dup.i 1
            push.v [array]self.b
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 6
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            pushi.e -1
            pushi.e 0
            dup.i 1
            push.v [array]self.c
            dup.v 0
            dup.i 4 6
            push.e 1
            add.i.v
            pop.i.v [array]self.c
            conv.v.i
            dup.i 1
            push.v [array]self.b
            dup.v 0
            dup.i 4 6
            push.e 1
            add.i.v
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            pushi.e -1
            pushi.e 0
            dup.i 1
            push.v [array]self.c
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 6
            pop.i.v [array]self.c
            conv.v.i
            dup.i 1
            push.v [array]self.b
            dup.v 0
            dup.i 4 6
            push.e 1
            add.i.v
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            pushi.e -1
            pushi.e 0
            dup.i 1
            push.v [array]self.c
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 6
            pop.i.v [array]self.c
            conv.v.i
            dup.i 1
            push.v [array]self.b
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 6
            pop.i.v [array]self.b
            pop.v.v self.a
            pushi.e -1
            pushi.e -1
            pushi.e 0
            dup.i 1
            push.v [array]self.c
            dup.v 0
            dup.i 4 6
            push.e 1
            add.i.v
            pop.i.v [array]self.c
            conv.v.i
            dup.i 1
            push.v [array]self.b
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 6
            pop.i.v [array]self.b
            pop.v.v self.a
            """,
            """
            a = b++;
            a = ++b;
            a = b.c.d++;
            a = ++b.c.d;
            a = b[0]++;
            a = ++b[0];
            a = a.b.c[0]++;
            a = ++a.b.c[0];
            a = a.b[0].c++;
            a = ++a.b[0].c;
            a = a.b[0].c[0]++;
            a = ++a.b[0].c[0];
            a = b[c++]++;
            a = b[++c]++;
            a = ++b[++c];
            a = ++b[c++];
            a = b[c[0]++]++;
            a = b[++c[0]]++;
            a = ++b[++c[0]];
            a = ++b[c[0]++];
            """
        );
    }

    [Fact]
    public void TestNullishTernary()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            isnullish.e
            bf [5]
            
            :[1]
            popz.v
            push.v self.b
            conv.v.b
            bf [3]
            
            :[2]
            push.v self.c
            b [4]
            
            :[3]
            push.v self.d
            
            :[4]
            pop.v.v self.a
            b [6]
            
            :[5]
            popz.v
            
            :[6]
            """,
            """
            a ??= b ? c : d;
            """
        );
    }

    [Fact]
    public void TestMultiWithBreak()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            conv.v.b
            bf [10]

            :[1]
            push.v self.b
            pushi.e -9
            pushenv [6]

            :[2]
            push.v self.c
            pushi.e -9
            pushenv [4]

            :[3]
            pushi.e 1
            pop.v.i self.d

            :[4]
            popenv [3]

            :[5]
            pushi.e 1
            pop.v.i self.e
            b [8]

            :[6]
            popenv [2]

            :[7]
            b [9]

            :[8]
            popenv <drop>

            :[9]
            b [12]

            :[10]
            push.v self.f
            pushi.e -9
            pushenv [11]

            :[11]
            popenv [11]

            :[12]
            """,
            """
            if (a)
            {
                with (b)
                {
                    with (c)
                    {
                        d = 1;
                    }
                    e = 1;
                    break;
                }
            }
            else
            {
                with (f)
                {
                }
            }
            """
        );
    }

    [Fact]
    public void TestMultiWithBreak2()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            conv.v.b
            bf [10]

            :[1]
            push.v self.b
            pushi.e -9
            pushenv [6]

            :[2]
            push.v self.c
            pushi.e -9
            pushenv [4]

            :[3]
            pushi.e 1
            pop.v.i self.d

            :[4]
            popenv [3]

            :[5]
            pushi.e 1
            pop.v.i self.e
            b [8]

            :[6]
            popenv [2]
            
            :[7]
            b [9]

            :[8]
            popenv <drop>

            :[9]
            b [18]

            :[10]
            push.v self.b
            pushi.e -9
            pushenv [15]

            :[11]
            push.v self.c
            pushi.e -9
            pushenv [13]

            :[12]
            pushi.e 1
            pop.v.i self.d

            :[13]
            popenv [12]
            
            :[14]
            pushi.e 1
            pop.v.i self.e
            b [17]

            :[15]
            popenv [11]
            
            :[16]
            b [18]

            :[17]
            popenv <drop>

            :[18]
            """,
            """
            if (a)
            {
                with (b)
                {
                    with (c)
                    {
                        d = 1;
                    }
                    e = 1;
                    break;
                }
            }
            else
            {
                with (b)
                {
                    with (c)
                    {
                        d = 1;
                    }
                    e = 1;
                    break;
                }
            }
            """
        );
    }

    [Fact]
    public void TestIfElseWithBreak()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            conv.v.b
            bf [2]

            :[1]
            b [7]

            :[2]
            push.v self.b
            pushi.e -9
            pushenv [4]

            :[3]
            b [6]

            :[4]
            popenv [3]

            :[5]
            b [7]

            :[6]
            popenv <drop>

            :[7]
            """,
            """
            if (a)
            {
            }
            else
            {
                with (b)
                {
                    break;
                }
            }
            """
        );
    }

    [Fact]
    public void TestSwitchIfShortCircuit()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            dup.v 0
            pushi.e 1
            cmp.i.v EQ
            bt [2]

            :[1]
            b [3]

            :[2]
            b [3]

            :[3]
            popz.v
            push.v self.b
            conv.v.b
            bf [5]

            :[4]
            push.v self.c
            conv.v.b
            b [6]

            :[5]
            push.e 0

            :[6]
            bf [7]

            :[7]
            """,
            """
            switch (a)
            {
                case 1:
                    break;
            }
            if (b && c)
            {
            }
            """
        );
    }

    [Fact]
    public void TestMultiArrays()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            exit.i

            :[1]
            pushi.e -1
            pushi.e 0
            push.v [multipush]self.array
            pushi.e 1
            pushac.e
            pushi.e 2
            pushaf.e
            pop.v.v self.basic_push_multi
            exit.i

            :[2]
            pushi.e 3
            conv.i.v
            pushi.e -1
            pushi.e 0
            push.v [multipushpop]self.basic_pop_multi
            pushi.e 1
            pushac.e
            pushi.e 2
            popaf.e
            exit.i

            :[3]
            pushi.e -1
            pushi.e 0
            push.v [multipushpop]self.prefix_multi
            pushi.e 1
            pushac.e
            pushi.e 2
            dup.i 4
            pushaf.e
            push.e 1
            add.i.v
            dup.i 4 5
            popaf.e
            exit.i

            :[4]
            pushi.e -1
            pushi.e 0
            push.v [multipushpop]self.postfix_multi
            pushi.e 1
            pushac.e
            pushi.e 2
            dup.i 4
            pushaf.e
            push.e 1
            add.i.v
            dup.i 4 5

            popaf.e
            exit.i

            :[5]
            pushi.e -1
            pushi.e 0
            push.v [multipushpop]self.compound_multi
            pushi.e 1
            pushac.e
            pushi.e 2
            dup.i 4
            savearef.e
            pushaf.e
            pushi.e 3
            add.i.v
            restorearef.e
            dup.i 4 5
            popaf.e
            exit.i

            :[6]
            pushi.e -1
            pushi.e 0
            push.v [multipushpop]self.prefix_multi
            pushi.e 1
            pushac.e
            pushi.e 2
            dup.i 4
            pushaf.e
            push.e 1
            add.i.v
            dup.v 0
            dup.i 4 9
            dup.i 4 5
            popaf.e
            pop.v.v self.a
            exit.i

            :[7]
            pushi.e -1
            pushi.e 0
            push.v [multipushpop]self.postfix_multi
            pushi.e 1
            pushac.e
            pushi.e 2
            dup.i 4
            pushaf.e
            dup.v 0
            dup.i 4 9
            push.e 1
            add.i.v
            dup.i 4 5
            popaf.e
            pop.v.v self.a
            exit.i
            """,
            """
            exit;
            basic_push_multi = array[0][1][2];
            exit;
            basic_pop_multi[0][1][2] = 3;
            exit;
            prefix_multi[0][1][2]++;
            exit;
            postfix_multi[0][1][2]++;
            exit;
            compound_multi[0][1][2] += 3;
            exit;
            a = ++prefix_multi[0][1][2];
            exit;
            a = postfix_multi[0][1][2]++;
            exit;
            """
        );
    }

    [Fact]
    public void TestSwitchReturn()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            dup.v 0
            pushi.e 0
            cmp.i.v EQ
            bt [2]

            :[1]
            b [3]

            :[2]
            b [3]

            :[3]
            popz.v
            call.i game_end 0
            popz.v
            push.v self.b
            ret.v
            """,
            """
            switch (a)
            {
                case 0:
                    break;
            }
            game_end();
            return b;
            """
        );
    }

    [Fact]
    public void TestNestedSwitchThenExit()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            dup.v 0
            pushi.e 0
            cmp.i.v EQ
            bt [2]

            :[1]
            b [8]

            :[2]
            push.v self.b
            conv.v.b
            bf [7]

            :[3]
            push.v self.c
            dup.v 0
            pushi.e 0
            cmp.i.v EQ
            bt [5]

            :[4]
            b [6]

            :[5]
            b [6]

            :[6]
            popz.v
            pushi.e 0
            pop.v.i self.d
            popz.v
            exit.i

            :[7]
            b [8]

            :[8]
            popz.v
            """,
            """
            switch (a)
            {
                case 0:
                    if (b)
                    {
                        switch (c)
                        {
                            case 0:
                                break;
                        }
                        d = 0;
                        exit;
                    }
                    break;
            }
            """
        );
    }

    [Fact]
    public void TestNestedSwitchExitInFragment()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            dup.v 0
            pushi.e 0
            cmp.i.v EQ
            bt [2]

            :[1]
            b [9]

            :[2]
            b [8]

            > inner_fragment (locals=0, args=0)
            :[3]
            push.v self.b
            dup.v 0
            pushi.e 0
            cmp.i.v EQ
            bt [5]

            :[4]
            b [6]

            :[5]
            b [6]

            :[6]
            popz.v
            exit.i

            :[7]
            exit.i

            :[8]
            push.i [function]inner_fragment
            conv.i.v
            pushi.e -1
            conv.i.v
            call.i method 2
            pop.v.v self.func
            b [9]

            :[9]
            popz.v
            """,
            """
            switch (a)
            {
                case 0:
                    func = function()
                    {
                        switch (b)
                        {
                            case 0:
                                break;
                        }
                        exit;
                    };
                    break;
            }
            """
        );
    }

    [Fact]
    public void TestWithNestedWhile()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.a
            pushi.e -9
            pushenv [3]

            :[1]
            push.v self.b
            conv.v.b
            bf [3]

            :[2]
            b [1]

            :[3]
            popenv [1]
            """,
            """
            with (a)
            {
                while (b)
                {
                }
            }
            """
        );
    }

    [Fact]
    public void TestRoomInstanceReference()
    {
        TestUtil.VerifyDecompileResult(
            """
            pushref.i 123123 RoomInstance
            pushi.e -9
            push.v [stacktop]self.a
            pop.v.v self.b
            pushref.i 456456 RoomInstance
            pop.v.v self.c
            """,
            """
            b = inst_id_123123.a;
            c = inst_id_456456;
            """
        );
    }

    [Fact]
    public void TestWithContinue()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            pushi.e 123
            pushenv [2]
            
            :[1]
            b [2]
            
            :[2]
            popenv [1]
            """,
            """
            with (123)
            {
                continue;
            }
            """
        );
    }

    [Fact]
    public void TestWithBreakContinue()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            pushi.e 123
            pushenv [5]
            
            :[1]
            push.v self.a
            conv.v.b
            bf [3]
            
            :[2]
            b [7]
            
            :[3]
            push.v self.b
            conv.v.b
            bf [5]
            
            :[4]
            b [5]
            
            :[5]
            popenv [1]
            
            :[6]
            b [8]
            
            :[7]
            popenv <drop>
            
            :[8]
            """,
            """
            with (123)
            {
                if (a)
                {
                    break;
                }
                if (b)
                {
                    continue;
                }
            }
            """
        );
    }

    [Fact]
    public void TestConditionalNestedNullish()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.b
            conv.v.b
            bf [2]

            :[1]
            push.v self.c
            b [4]

            :[2]
            push.v self.d
            isnullish.e
            bf [4]

            :[3]
            popz.v
            push.v self.e

            :[4]
            pop.v.v self.a
            """,
            """
            a = b ? c : (d ?? e);
            """
        );
    }

    [Fact]
    public void TestConditionalNestedNullish2()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.b
            conv.v.b
            bf [2]

            :[1]
            push.v self.c
            b [6]

            :[2]
            push.v self.d
            isnullish.e
            bf [6]

            :[3]
            popz.v
            push.v self.e
            conv.v.b
            bf [5]

            :[4]
            push.v self.f
            b [6]

            :[5]
            push.v self.g

            :[6]
            pop.v.v self.a
            """,
            """
            a = b ? c : (d ?? (e ? f : g));
            """
        );
    }

    [Fact]
    public void TestConditionalNestedNullish3()
    {
        TestUtil.VerifyDecompileResult(
            """
            :[0]
            push.v self.b
            conv.v.b
            bf [2]

            :[1]
            push.v self.c
            b [5]

            :[2]
            push.v self.d
            isnullish.e
            bf [5]

            :[3]
            popz.v
            push.v self.e
            isnullish.e
            bf [5]

            :[4]
            popz.v
            push.v self.f

            :[5]
            pop.v.v self.a
            """,
            """
            a = b ? c : (d ?? (e ?? f));
            """
        );
    }
}
