module GameLogic

open Suave.Successful
open ObjectModel
open Repository

// Fields
let rnd = System.Random()

let generate_dominos() =
    let rec gen_dom line col list = 
        match line, col with
        | 0, _ -> list @ [{v1=0; v2=0}]
        | _ when col > line ->
            list @ gen_dom (line-1) 0 []
        | _ ->
            let new_list = {v1=line; v2=col}::list
            new_list @ gen_dom line (col+1) []
    gen_dom 6 0 []

let initial_deck = generate_dominos()

// Functions
let enough_players = true

let start_game player1 player2 = 
    let new_game = {id=read_games().Length; player1=player1; player2=player2; board=[]; main_deck=[]; deck1=[]; deck2=[]}
    write_game new_game
    OK (sprintf "Starting game for players %s and %s..." player1 player2)

let handle_no_game_available watch =
        match watch with
        | true -> OK "Watching a game while waiting..."
        | _ -> OK "Just waiting..."

let get_game (player_name : string) = read_games() |> List.tryFind(fun x ->
        match x with
        | {id = _; player1 = p1; player2 = p2} -> (String.equals player_name p1) || (String.equals player_name p2)
        | _ -> false)

let get_main_deck (game : Game) = game.main_deck

let get_player_deck (game : Game) player = 
    match game.player1, game.player2 with
    | x, _ when x = player -> Some(game.deck1)
    | _, y when y = player -> Some(game.deck2)
    | _ -> None

let schuffle (l : 'a list) = l |> List.sortBy(fun i -> rnd.Next())

let deal_domino (deck : Deck) = deck |> schuffle |> Seq.head
    
let draw player = get_game player |> Option.map get_main_deck |> Option.map deal_domino

let action_draw player = 
    match draw player with
    | Some d ->
        let deck = get_game player |> Option.map get_player_deck
        
        // TODO : add domino to player deck
        // TODO : remove domino from main deck

        sprintf "Player %s draw a new domino (%i:%i)" player d.v1 d.v2
    | None -> "Main deck is empty"

let play name action (domino : Domino) (position : Position) =
    match get_game name, action with
    | None, _ -> Failure (sprintf "Player %s not found in any game" name)
    | _, "draw" -> Success (sprintf "%s draws a new domino from the deck" name)
    | _, "place" -> Success (sprintf "%s places a domino of value %i:%i on the board at position %i:%i" name domino.v1 domino.v2 position.x position.y)
    | _, "quit" -> Success (sprintf "%s quits the game" name)
    | _, "win" -> Success (sprintf "%s wins the game!" name)
    | _ -> Failure "Unknown action"
