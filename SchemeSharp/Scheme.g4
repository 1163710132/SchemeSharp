grammar Scheme;

prog:
    (expr)*
;

expr:
    compositedExpr
    | literal
    | word
    | operator
;

compositedExpr:
    '(' (expr)*')'
;

word:
    WORD
;

WORD:
    '_'
    | [a-zA-Z] ([a-zA-Z0-9_])*
;

literal:
    literalString
    | literalNumber
    | literalBoolean
;

literalString:
    STRING
;

literalNumber:
    NUMBER
;

LETTER:
    [a-zA-Z];
    
SPECIAL_INITIAL:
    [!$%&*/:<=>?^_~];
    
PECULIAR_IDENTIFIER:
    [+-];

STRING:
    '"' ('\\"' | .)*? '"'
;

NUMBER:
    [0-9] ('.' | [0-9])*
;

BOOLEAN:
    '#t'
    | '#f'
;

CHARACTER:
    '#\\' [a-zA-Z]*;
    


literalBoolean:
    BOOLEAN
;

//digital:
//    '0'|'1'|'2'|'3'|'4'|'5'|'6'|'7'|'8'|'9'
//;

operator:
    '+' | '-' | '*' | '/' | '=' | '&' | '!' | '|'
;

WS: [ \t\n] -> skip;