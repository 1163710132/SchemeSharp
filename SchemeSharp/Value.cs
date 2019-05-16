using System;

namespace SchemeSharp
{
    public delegate bool ListConsumer(Value current, bool hasMore);
    
    public abstract class Value
    {
        public virtual bool IsBoolean => false;
        public virtual bool IsNumber => false;
        public virtual bool IsComplex => false;
        public virtual bool IsPair => false;
        public virtual bool IsNil => false;
        public virtual bool IsString => false;
        public virtual bool IsVector => false;
        public virtual bool IsProcedure => false;
        public virtual bool IsSymbol => false;
        public virtual bool IsList => false;

        public virtual bool AsBoolean() => true;

        public virtual Value Left => null;
        public virtual Value Right => null;

        public virtual int ListLength => 0;

        public virtual void CallProcedure(Value args, Continuation continuation)
            => continuation(null);

        public virtual bool CanSelfEvaluate => false;
        public virtual Value SelfEvaluate() => null;

        public virtual void Evaluate(Context context, Continuation continuation)
            => continuation(null);

        public virtual bool ForEach(ListConsumer consumer) => true;

        public virtual Symbol AsSymbol()
        {
            return null;
        }
        
        public T As<T>() where T: class
        {
            return this as T;
        }
    }
}