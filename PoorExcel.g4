 grammar PoorExcel;

 /*
 * Parser Rules
 */

 compileUnit:expression EOF;

 expression:
 LPAREN expression RPAREN #ParenthesizedExpr
 | expression operatorToken=(MULTIPLY | DIVIDE) expression #MultiplicativeExpr
 | operatorToken=(MMAX | MMIN) LPAREN expression (COMMA expression)* RPAREN #MmaxMminExpr
 | expression operatorToken=(ADD | SUBSTRACT) expression #AdditiveExpr
 | expression EQUAL expression #EqualExpr
 | expression GREATER expression #GreaterExpr
 | expression LESS expression #LessExpr
 | NOT LPAREN expression RPAREN #NotExpr
 | NUMBER #NumberExpr
 | IDENTIFIER #IdentifierExpr
 ;

 /*
 * Lexer Rules
 */

 NUMBER:INT('.'INT)?;
 IDENTIFIER:[a-zA-Z]+[1-9][0-9]+;
 INT : ('0'..'9')+;
 MULTIPLY:'*';
 DIVIDE:'/';
 ADD:'+';
 SUBSTRACT:'-';
 EQUAL:'=';
 LESS:'<';
 GREATER:'>';
 NOT:'not';
 MMAX:'mmax';
 MMIN:'mmin';
 LPAREN:'(';
 RPAREN:')';
 COMMA:',';
 WS:[\t\r\n]->channel(HIDDEN);

