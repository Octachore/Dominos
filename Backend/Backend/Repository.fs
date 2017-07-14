/// <summary>Provides helper functions for game state persistence in JSON file.</summary>
module Repository

open System.IO
open ObjectModel
open Newtonsoft.Json

let file = @"D:\repository.json"


// IO
let read() =
    match File.Exists(file) with
    | false -> 
        File.WriteAllText(file, "")
        ""
    | true -> File.ReadAllText(file)


let write text = File.WriteAllText(file, text)

// (De)serialization
let serialize = fun x -> JsonConvert.SerializeObject(x, Formatting.Indented)

let read_games() = 
    match read() with
    | "" -> []
    | content -> content |> JsonConvert.DeserializeObject<Game list>

let read_game id = read_games() |> List.tryFind(fun g -> g.id = id)

let create_game game = game::read_games() |> serialize |> write

let update_game game =
    read_games()
    |> List.map (fun g -> 
        match g.id with
        | id when id = game.id -> game
        | _ -> g)
    |> serialize |> write

let write_game (game: Game) =
    match read_game game.id with
    | Some(g)-> update_game game
    | None -> create_game game