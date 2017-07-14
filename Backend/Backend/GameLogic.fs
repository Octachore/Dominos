﻿/// <summary>Provides functions to handle dominos game logic.</summary>
module GameLogic

open Suave.Successful
open ObjectModel
open Repository
open GenericUtils

/// <summary>Generates all the 28 dominos.</summary>
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

/// <summary>The initial deck containing the 28 dominos..</summary>
let initial_deck = generate_dominos()

let enough_players = true

/// <summary>Checks if the place at a given positoin is free on the board.</summary>
let place_is_free (board : Board) (pos : Position) =
    (board |> List.where (fun (item : Domino * Position) ->
        let _, p = item
        (p.x1 = pos.x1 && p.y1 = pos.y1)
        || (p.x1 = pos.x2 && p.y1 = pos.y2)
        || (p.x2 = pos.x1 && p.y2 = pos.y1)
        || (p.x2 = pos.x2 && p.y2 = pos.y2))).Length = 0

/// <summary>Gets the squares to validate around a value of a domino, i.e. all the 9 squares around it except the one where there is the second value of the domino.</summary>
let get_squares_to_validate (pos : Position) =
    let rec around x y exclude_x exclude_y =
        [
            (x+1, y+1)
            (x, y+1)
            (x-1, y+1)
            (x+1, y)
            (x, y)
            (x-1, y)
            (x+1, y-1)
            (x, y-1)
            (x-1, y-1)
        ] |> List.where (fun (square : int*int) ->
            let a, b = square
            a <> exclude_x && b <> exclude_y)
    (around pos.x1 pos.x1 pos.x2 pos.y2, around pos.x2 pos.x2 pos.x1 pos.y1)

/// <summary>Gets the value of the board item corresponding to the x and y coordinates on the board.</summary>
let get_value x y (board_item : Domino*Position) =
    let dom, pos = board_item
    match x, y with
    | _, _ when x = pos.x1 && y = pos.y1 -> Some(dom.v1)
    | _, _ when x = pos.x2 && y = pos.y2 -> Some(dom.v2)
    | _ -> None

/// <summary>Gets the values around each square in the list on the board.</summary>
let get_values (squares : (int*int) list) (board : Board) =
    squares |> List.map (fun square -> 
        board |> List.map (get_value 1 2)
        |> List.where(fun value -> 
            match value with
            | Some(i) -> true
            | _ -> false)
        |> List.map (fun option -> option.Value))
        |> List.concat

/// <summary>Checks if the placement of a domino on the board at a given position is legal (that the values matches and that there is no "parasite" value that also touches the domino).</summary>
let place_is_legal (board : Board) (domino : Domino) (pos : Position) =
    let squares_x, squares_y = get_squares_to_validate pos
    board |> get_values squares_x |> all_same
    && board |> get_values squares_y |>  all_same
    
let do_placement game domino position = "TODO"

/// <summary>Validates (free and legal) a place action.</summary>
let valid_place (game : Game) (domino : Domino) (position : Position) =
    match place_is_free game.board position, place_is_legal game.board domino position with
    | true, true -> Success(do_placement game domino position)
    | false, true -> Failure("Place occupied")
    | true, false -> Failure("Illegal")
    | _ -> Failure("Place occupied and illegal")

/// <summary>Starts a new game.</summary>
let start_game player1 player2 = 
    let new_game = {id=read_games().Length; player1=player1; player2=player2; board=[]; main_deck=initial_deck; deck1=[]; deck2=[]}
    write_game new_game
    OK (sprintf "Starting game for players %s and %s..." player1 player2)

/// <summary>Gats the game a player is in from all the existing games.</summary>
let get_game (player_name : string) = read_games() |> List.tryFind(fun x ->
        match x with
        | {id = _; player1 = p1; player2 = p2} -> (String.equals player_name p1) || (String.equals player_name p2)
        | _ -> false)

/// <summary>Gets the main deck of a game.</summary>
let get_main_deck (game : Game) = game.main_deck

/// <summary>Gets a player's deck.</summary>
let get_player_deck (game : Game) player = 
    match game.player1, game.player2 with
    | x, _ when x = player -> Some(game.deck1)
    | _, y when y = player -> Some(game.deck2)
    | _ -> None

/// <summary>Gives a random domino from a deck.</summary>
let deal_domino (deck : Deck) = deck |> shuffle |> Seq.head
    
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
    | _, "place" -> Success (sprintf "%s places a domino of value %i:%i on the board at position %i:%i/%i:%i" name domino.v1 domino.v2 position.x1 position.y1 position.x2 position.y2)
    | _, "quit" -> Success (sprintf "%s quits the game" name)
    | _, "win" -> Success (sprintf "%s wins the game!" name)
    | _ -> Failure "Unknown action"

let handle_no_game_available watch =
        match watch with
        | true -> OK "Watching a game while waiting..."
        | _ -> OK "Just waiting..."
