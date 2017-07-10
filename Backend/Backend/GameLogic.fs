module GameLogic

open Suave.Successful
open Suave.RequestErrors


type Domino = {v1:int; v2:int}

type Position = {x:int; y:int}

type ActionResult =
    | Success
    | Failure

let play name action (domino : Domino) (position : Position) =
    match action with
    | "draw" -> OK (sprintf "%s draws a new domino from the deck" name)
    | "place" -> OK (sprintf "%s places a domino of value %i:%i on the board at position %i:%i" name domino.v1 domino.v2 position.x position.y)
    | "quit" -> OK (sprintf "%s quits the game" name)
    | "win" -> OK (sprintf "%s wins the game!" name)
    | _ -> BAD_REQUEST "Unknown action"