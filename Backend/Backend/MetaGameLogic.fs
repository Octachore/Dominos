module MetaGameLogic

open Suave
open Suave.Successful
open System
open System.IO

type Game = Game of int * string * string

let mutable games = []

let enough_players = true

let start_game player1 player2 = 
    games <- Game(games.Length, player1, player2) :: games
    File.WriteAllText(@"C:\Users\Nicolas Maurice\Desktop\log.txt", sprintf "%A" games)
    OK "Starting game..."

let handle_no_game_available watch =
        match watch with
        | true -> OK "Watching a game while waiting..."
        | _ -> OK "Just waiting..."