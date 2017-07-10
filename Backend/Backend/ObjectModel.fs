module ObjectModel

type Domino = {v1:int; v2:int}

type Position = {x:int; y:int}

type ActionResult =
    | Success of string
    | Failure of string

type Deck = Domino list

type Board = (Domino * Position) list

type Game = {id:int; player1: string; player2: string; board: Board; main_deck: Deck; deck1: Deck; deck2: Deck}