/// <summary>Provides type definitions.</summary>
module ObjectModel

/// <summary>Represents a domino with its two values.</summary>
type Domino = {v1:int; v2:int}

/// <summary>Represents the position of a domino, with the position of the first and second squares.</summary>
type Position = {x1:int; y1:int; x2:int; y2:int;}

/// <summary>Represents the result of a game action.</summary>
type ActionResult =
    | Success of string
    | Failure of string

/// <summary>represents a domino deck.</summary>
type Deck = Domino list

/// <summary>Represents the board where dominos are placed.</summary>
type Board = (Domino * Position) list

/// <summary>represents a domino game.</summary>
type Game = {id:int; player1: string; player2: string; turn:string; board: Board; main_deck: Deck; deck1: Deck; deck2: Deck}