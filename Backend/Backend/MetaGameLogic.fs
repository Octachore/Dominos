module MetaGameLogic

open Suave
open Suave.Successful

let enough_players = true

let start_game = OK "Starting game..."

let handle_no_game_available watch =
        match watch with
        | true -> OK "Watching a game while waiting..."
        | _ -> OK "Just waiting..."