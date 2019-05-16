using System;

namespace SchemeSharp
{
    public class Interpreter
    {
        public Context Global { get; }

        public Interpreter()
        {
            Global = new Context(null);
        }

        public void Interpret(Value input)
        {
            input.Evaluate(Global, value =>
            {
                Console.WriteLine(value.ToString());
            });
        }
    }
}