# PandahCompiler

Types:

a :: Int
varname :: Type

Assignment:
a = b
varname = varname or literal (or expression)

Declaring a variable and not using it should only give errors on release build

Expression Syntax:

The operator should be added between the operands

1 + 2 	# 3

a :: Int
b :: Int
a + b 	# anything

Extended expressions are possible too
a :: Int
a = b + c + 3 + e

Negation operator will negate the variable
a :: Int
b :: Int
a = -3
b = -a # 3

Functions should first be given a Type and Parameter types (This can be on any line)
Sum :: Int <- |Int|
Sum([]) # In case of an emtpy array
  return 0 (or null)
end
Sum(x) #If given a variable it assumes you want to actualy use it for logic
  sum :: Int
  x.foreach |i|	
    sum += i
  end
end

This allows for pattern matching in PandahV2
When there is a function that takes multiple parameters and you don't need all of them for the logic you can use an _ to say you don't care
Move :: Void <- Bool, Int # Can Move, Speed
Move(false,_)
  # Do nothing (it is not allowed to move)
end
Move(true, speed)
  @x += speed #do logic with the given parameter
end

As seen in the previous function. different variables should get different prefixes.
Private: no prefix
Public: if an instance variable use @
	if a class variable use $ (Static or global)

This is dont so one could type in 1 character to see all the variables that belong to an instance (in a reasonalbly good ide)

Pandah uses Classes to represent basic data in this way:

class Animal
  name :: String
  Move :: Int <- Bool, Int

  # Logic here
end

Pandah prefers to have all the variable names on the top so one could quickly look at what a class can do. 
Actual Logic should happen further down the class structure. (this is not yet forced)
Pandah also strives to support higher order functions (means: passing functions as parameters)

  FindSpeed :: Int <- String
  Move :: Int <- Bool, Int <- String #Can Move, findSpeed

  Move(true, function)
    function(@name)
  end
  
  #logic
  Move(true, FindSpeed)

A "Hello Pandah" in Pandah would look like this:

class Greeter
  Greet :: Void <- Void 

  Greet() do
    Print("Hello World")
  end
end

greeter :: Greeter        #Gives type
greeter = Greeter.new()   #Calls constructor
greeter.Greet()		        #Calls Greet function

Pandah also has a build-in unit test framework.
It works like this
Anywhere in code (Preferably at the end of any class you are testing) you can add a Test function like this

[RUN]  myAnimal.Move(true, 1) 
[TEST] @x == 1

As you can see the testing is based on prefixes which tell the framework what to do.
Other prefixes are

[RESET] myAnimal = Animal.new() #Resets the variables (Runs before every [RUN] Prefix)
# More to come

Like some other languages Pandah starts globally (not within any class or method)
This makes it easier to write quick snippets of code without having a whole class structure ready.

Pandah can use blocks as arguments
Blocks are made by putting & before the type that the block wants.
for example 
&Int (a block of code that does something with an int)

Blocks can be used for iterative functions like this

10.Times() do |i|
  print i
end

What this code does is call the Times function. this function takes a block of type Int.
In this case the block is "print i". i is an iterator given by the times function.
To give a block a variable to use can be done using the yield keyword. the yield keyword runs the given block.
This is what the Times function would look like 

Times :: Void <- &Int
Times(block)
  while i < this
    block(i++)
  end
end

As you can see it uses this to know when to stop. This is because the function was called on an Int.
It also has a block parameter. this parameter is a function that runs the given block. the parameters of this block will be used in the block.
There is no pattern matching in blocks.


TODO: Rethink Constructor calling (new Animal and Animal.new are both meh)
      Think some more

-----------------
--ANTLR GRAMMAR--
-----------------

program        : declaration* EOF
declaration    : vardecl | methoddecl | statement ;
methoddecl     : IDENTIFIER "::" TYPE "<-" (TYPE) (("," TYPE)* )?
vardecl        : IDENTIFIER "::" TYPE ;
statement      : expression ;
block          : DO statement END 
	       | "{" statement "}" ;
assignment     : IDENTIFIER "=" expression ;
expression     : logic | assignment ;
logic	       : equality ( ( "||" | "&&" ) equality)* ;
equality       : comparison ( ( "!=" | "==" ) comparison )* ;
comparison     : addition ( ( ">" | ">=" | "<" | "<=" ) addition )* ;
addition       : multiplication ( ( "-" | "+" ) multiplication )* ;
multiplication : unary ( ( "/" | "*" | "%" ) unary )* ;
unary          : ( "!" | "-" ) unary 
               | primary ;
primary        : NUMBER | STRING | IDENTIFIER | "false" | "true" | "null"
               | "(" expression ")" ;
while          : "while" expression "do" declaration* "end"








