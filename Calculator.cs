using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoorExcel
{
    public static class Calculator
    {
        public static double Evaluate(string expression)
        {
            var lexer = new PoorExcelLexer(new AntlrInputStream(expression));
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new ThrowExceptionErrorListener());
            var tokens = new CommonTokenStream(lexer);
            var parser = new PoorExcelParser(tokens);
            var tree = parser.compileUnit();
            var visitor = new PoorExcelVisitor();
            return visitor.Visit(tree);
        }
    }
}
