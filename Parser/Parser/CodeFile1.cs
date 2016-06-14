/*
        private string[] KeyWords;
        private string[] OneCharacterLongSymbols;
        private string[] TwoCharacterLongSymbols;
 *      
 *     
        KeyWords = begin end if then for from to do while return write read
        RobotKeyWords = Position Explode MoveUp MoveDown MoveLeft MoveRight
 *      OneCharacterLongSymbols =("* + - ; = ( ) & |").Split(' ');
        TwoCharacterLongSymbols =("== <= >= <> != ++ ** -- += -= || ").Split(' ');
 * 
 *      <while loop> ::= "While" "(" <condition> ")"
		"begin" 
			<statement> 
		"end"


<for loop> ::= "for" <identifier> "from" <StartInt> "to" <StopInt> "do"
	       "begin" 
			<statement> 
		"end"

<repeat loop> ::= "repeat" 
		<statement> 
	     "until" "(" <condition> ")"

<main program> ::= "MainBegin" 
			<statement> 
		   "MainEnd"

<if statement> ::= "if" "(" <condition> ")" "then" "begin" <statement> "end"
		   ["else" "if" "("<condition>")" "then" "begin" <statement> "end"] 
		   ["else" "begin" <statement> "end"]

<Aritmetic expresion> ::= <value> | <identifier> [ { <Aritmetic operator> <value> | <identifier> }  ]

<Boolean expresion> ::= <value> | <identifier> [ {<Boolean operator> <value> | <identifier>} ]

<Condition> ::= <Boolean expresion>

<Atribution> ::= ( <IntIdentifier> "<-" <Aritmetic expresion> ) | (<BoolIdentifier> "<-" <Boolean espresion>)

<Statement> ::= [<if statement>] [<while/for/repeat loop>] [<Atribution>] 
 *      
*/