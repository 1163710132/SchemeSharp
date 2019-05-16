using System;

namespace SchemeSharp
{
    public abstract class SelfEvaluateValue : Value
    {
        public override bool CanSelfEvaluate => true;

        public override Value SelfEvaluate()
        {
            return this;
        }

        public override void Evaluate(Context context, Continuation continuation)
        {
            continuation(this);
        }
    }

    public class Symbol : Value
    {
        public string Identifier { get; }

        public Symbol(string identifier)
        {
            Identifier = identifier;
        }

        public override bool IsSymbol => true;

        public override Symbol AsSymbol()
        {
            return this;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Symbol symbol && symbol.Identifier.Equals(Identifier);
        }
    }

    public class Pair : Value
    {
        public override Value Left { get; }
        public override Value Right { get; }

        public Pair(Value left, Value right)
        {
            Left = left;
            Right = right;
        }

        public override bool ForEach(ListConsumer consumer)
        {
            Value pair = this;
            bool wantMore = true;
            while (!pair.IsNil && wantMore)
            {
                Value remaining = pair.Right;
                wantMore = consumer(pair.Left, !remaining.IsNil);
                pair = remaining;
            }

            return wantMore;
        }

        public override void Evaluate(Context context, Continuation continuation)
        {
            Left.Evaluate(context, leftValue =>
            {
                if (leftValue == null && Left.IsSymbol)
                {
                    switch (Left.AsSymbol().Identifier)
                    {
                        case "define":
                        { 
                            Value pattern = Right.Left;
                            Value body = Right.Right.Left;
                            body.Evaluate(context, bodyValue =>
                            {
                                context.DefineValue(pattern.AsSymbol(), bodyValue);
                                continuation(Nil.Instance);
                            });
                            break;
                        }

                        case "let":
                        {
                            Value localDefines = Right.Left;
                            localDefines.ForEach((localDefine, hasMore) =>
                            {
                                Value localName = localDefine.Left;
                                Value localBody = localDefine.Right.Left;
                                localBody.Evaluate(context,
                                    localValue =>
                                    {
                                        context.DefineValue(localName.AsSymbol(), localValue);
                                    });
                                return true;
                            });
                            Value letBody = Right.Right.Left;
                            letBody.Evaluate(context, continuation);
                            break;
                        }

                        case "quote":
                        {
                            Value quoteBody = Right.Left;
                            continuation(quoteBody);
                            break;
                        }

                        case "lambda":
                        {
                            Value lambdaArgs = Right.Left;
                            Value lambdaBody = Right.Right.Left;
                            Value procedure = new Procedure(lambdaArgs, lambdaBody);
                            continuation(procedure);
                            break;
                        }

                        case "if":
                        {
                            Value testBody = Right.Left;
                            Value thenBody = Right.Right.Left;
                            Value elseBody = Right.Right.Right.Left;
                            testBody.Evaluate(context, testValue =>
                            {
                                if (testValue.AsBoolean())
                                {
                                    thenBody.Evaluate(context, continuation);
                                }
                                else
                                {
                                    elseBody.Evaluate(context, continuation);
                                }
                            });
                            break;
                        }

                        case "set!":
                        {
                            Value variableName = Right.Left;
                            Value variableBody = Right.Right.Left;
                            variableBody.Evaluate(context, bodyValue =>
                                {
                                    context.SetValue(variableName.AsSymbol(), bodyValue);
                                    continuation(Nil.Instance);
                                });
                            break;
                        }

                        case "begin":
                        {
                            EvaluateBegin(Right, context, continuation);
                            break;
                        }

                        case "cond":
                        {
                            EvaluateCond(Right, context, continuation);
                            break;
                        }

                        case "and":
                        {
                            EvaluateAnd(Right, context, continuation);
                            break;
                        }

                        case "or":
                        {
                            EvaluateOr(Right, context, continuation);
                            break;
                        }
                        
                        default:
                            throw new Exception();
                    }
                }
            });
        }
        
        private static void EvaluateBegin(Value remaining, Context context, Continuation continuation)
        {
            if (remaining.IsNil)
            {
                continuation(Nil.Instance);
            }else if (remaining.Right.IsNil)
            {
                remaining.Left.Evaluate(context, continuation);
            }
            else
            {
                remaining.Left.Evaluate(context, _ =>
                {
                    EvaluateBegin(remaining.Right, context, continuation);
                });
            }
        }
        
        
        private static void EvaluateCond(Value remaining, Context context, Continuation continuation)
        {
            if (remaining.IsNil)
            {
                continuation(Nil.Instance);
            }else
            {
                Value condition = remaining.Left;
                Value test = condition.Left;
                Value then = condition.Right.Left;
                test.Evaluate(context, testValue =>
                {
                    if (testValue.AsBoolean())
                    {
                        then.Evaluate(context, continuation);
                    }
                    else
                    {
                        EvaluateCond(remaining.Right, context, continuation);
                    }
                });
            }
        }
        
        private static void EvaluateAnd(Value remaining, Context context, Continuation continuation)
        {
            if (remaining.IsNil)
            {
                continuation(new Boolean(true));
            }
            else if(remaining.Right.IsNil)
            {
                remaining.Left.Evaluate(context, continuation);
            }
            else
            {
                remaining.Left.Evaluate(context, testValue =>
                {
                    if (testValue.AsBoolean())
                    {
                        EvaluateAnd(remaining.Right, context, continuation);
                    }
                    else
                    {
                        continuation(new Boolean(false));
                    }
                });
            }
        }
        
        private static void EvaluateOr(Value remaining, Context context, Continuation continuation)
        {
            if (remaining.IsNil)
            {
                continuation(new Boolean(false));
            }
            else if(remaining.Right.IsNil)
            {
                remaining.Left.Evaluate(context, continuation);
            }
            else
            {
                remaining.Left.Evaluate(context, testValue =>
                {
                    if (!testValue.AsBoolean())
                    {
                        EvaluateOr(remaining.Right, context, continuation);
                    }
                    else
                    {
                        continuation(new Boolean(true));
                    }
                });
            }                  
        }
    }

    public class Nil : Value
    {
        public static readonly Nil Instance = new Nil();

        private Nil()
        {
            
        }
    }

    public class Boolean : Value
    {
        public bool Value;

        public Boolean(bool value)
        {
            Value = value;
        }
    }

    public class Number : Value
    {
        
    }

    public class Character : Value
    {
        
    }

    public class String : Value
    {
        
    }

    public class Vector : Value
    {
        
    }

    public class Procedure : Value
    {
        public Value Args;
        public Value Body;

        public Procedure(Value args, Value body)
        {
            Args = args;
            Body = body;
        }
    }
}