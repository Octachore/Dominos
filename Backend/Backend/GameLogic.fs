module GameLogic

type Domino = {v1:int; v2:int}

type Position = {x:int; y:int}

type ActionResult =
    | Success of string
    | Failure of string

let play name action (domino : Domino) (position : Position) =
    match action with
    | "draw" -> Success (sprintf "%s draws a new domino from the deck" name)
    | "place" -> Success (sprintf "%s places a domino of value %i:%i on the board at position %i:%i" name domino.v1 domino.v2 position.x position.y)
    | "quit" -> Success (sprintf "%s quits the game" name)
    | "win" -> Success (sprintf "%s wins the game!" name)
    | _ -> Failure "Unknown action"

