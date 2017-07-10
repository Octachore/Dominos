module GameLogic

open Suave.Successful
open Suave.RequestErrors

type Domino(value1 : int, value2 : int) = 
    member this.Value1 : int = value1
    member this.Value2 : int = value2

type Position(x : int, y: int) = 
    member this.X : int = x
    member this.Y : int = y

type ActionResult =
    | Success
    | Failure

let play name action (domino : Domino) (position : Position) =
    match action with
    | "draw" -> OK (sprintf "%s draws a new domino from the deck" name)
    | "place" -> OK (sprintf "%s places a domino of value %i:%i on the board at position %i:%i" name domino.Value1 domino.Value2 position.X position.Y)
    | "quit" -> OK (sprintf "%s quits the game" name)
    | "win" -> OK (sprintf "%s wins the game!" name)
    | _ -> BAD_REQUEST "Unknown action"