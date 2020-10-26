using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoorExcel
{
    class PoorExcelVisitor : PoorExcelBaseVisitor<double>
    {
        Dictionary<string, double> tableIdentifier = new Dictionary<string, double>();
        public override double VisitCompileUnit(PoorExcelParser.CompileUnitContext context)
        {
            return Visit(context.expression());
        }
        public override double VisitNumberExpr(PoorExcelParser.NumberExprContext context)
        {
            var result = double.Parse(context.GetText());
            Debug.WriteLine(result);
            return result;
        }
        public override double VisitIdentifierExpr(PoorExcelParser.IdentifierExprContext context)
        {
            var result = context.GetText();
            double value;
            if (tableIdentifier.TryGetValue(result.ToString(), out value))
                return value;
            else
                return 0.0;
        }
        public override double VisitParenthesizedExpr(PoorExcelParser.ParenthesizedExprContext context)
        {
            return Visit(context.expression());
        }
        public override double VisitAdditiveExpr([NotNull] PoorExcelParser.AdditiveExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            if (context.operatorToken.Type == PoorExcelLexer.ADD)
            {
                Debug.WriteLine("{0}+{1}", left, right);
                return left + right;
            }
            else
            {
                Debug.WriteLine("{0}-{1}", left, right);
                return left - right;
            }
        }
        public override double VisitMultiplicativeExpr([NotNull] PoorExcelParser.MultiplicativeExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            if (context.operatorToken.Type == PoorExcelLexer.MULTIPLY)
            {
                Debug.WriteLine("{0}*{1}", left, right);
                return left * right;
            }
            else
            {
                Debug.WriteLine("{0} / {1}", left, right);
                return left / right;
            }
        }

        public override double VisitUnaryMExpr([NotNull] PoorExcelParser.UnaryMExprContext context)
        {
            var number = WalkLeft(context);
            return number - 1;
        }
        public override double VisitUnaryPExpr([NotNull] PoorExcelParser.UnaryPExprContext context)
        {
            var number = WalkLeft(context);
            return number + 1;
        }
        public override double VisitMmaxMminExpr([NotNull] PoorExcelParser.MmaxMminExprContext context)
        {
            List<double> list = new List<double>();
            int idx = 0;
            while (WalkN(context, idx) != 1.0101)
            {
                list.Add(WalkN(context, idx));
                idx++;
            }
            var sortedList = from l in list orderby l select l;
            if (context.operatorToken.Type == PoorExcelLexer.MMAX)
            {
                Debug.WriteLine("mmax(");
                for (idx = 0; idx < list.Count(); ++idx)
                {
                    Debug.WriteLine("{0}", list.ElementAt(idx));
                    if (idx != list.Count() - 1)
                        Debug.WriteLine(" , ");
                }
                Debug.WriteLine(")");
                return sortedList.ElementAt(sortedList.Count() - 1);
            }
            else
            {
                Debug.WriteLine("mmin(");
                for (idx = 0; idx < list.Count(); ++idx)
                {
                    Debug.WriteLine("{0}", list.ElementAt(idx));
                    if (idx != list.Count() - 1)
                        Debug.WriteLine(" , ");
                }
                Debug.WriteLine(")");
                return sortedList.ElementAt(0);
            }
        }
        public override double VisitEqualExpr([NotNull] PoorExcelParser.EqualExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            Debug.WriteLine("{0} = {1}", left, right);
            return Convert.ToDouble(left == right);
        }
        public override double VisitLessExpr([NotNull] PoorExcelParser.LessExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            Debug.WriteLine("{0} < {1}", left, right);
            return Convert.ToDouble(left < right);
        }
        public override double VisitGreaterExpr([NotNull] PoorExcelParser.GreaterExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            Debug.WriteLine("{0} > {1}", left, right);
            return Convert.ToDouble(left > right);
        }
        public override double VisitNotExpr([NotNull] PoorExcelParser.NotExprContext context)
        {
            return Convert.ToDouble(!Convert.ToBoolean(WalkN(context, 0)));
        }
        private double WalkLeft(PoorExcelParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<PoorExcelParser.ExpressionContext>(0));
        }

        private double WalkRight(PoorExcelParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<PoorExcelParser.ExpressionContext>(1));
        }
        private double WalkN(PoorExcelParser.ExpressionContext context,int idx)
        {
            try
            {
                return Visit(context.GetRuleContext<PoorExcelParser.ExpressionContext>(idx));
            }
            catch (NullReferenceException) { return 1.0101; }
        }

    }
    
   
}

