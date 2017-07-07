open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors

let enough_players = true

let start_game = OK "Starting game..."

let handle_no_game_available watch =
        match watch with
        | true -> OK "Watching a game while waiting..."
        | _ -> OK "Just waiting..."
    
type Domino(value1 : int, value2 : int) = 
    member this.Value1 : int = value1
    member this.Value2 : int = value2

type Position(x : int, y: int) = 
    member this.X : int = x
    member this.Y : int = y

let play name action (domino : Domino) (position : Position) =
    match action with
    | "draw" -> OK (sprintf "%s draws a new domino from the deck" name)
    | "place" -> OK (sprintf "%s places a domino of value %i:%i on the board at position %i:%i" name domino.Value1 domino.Value2 position.X position.Y)
    | "quit" -> OK (sprintf "%s quits the game" name)
    | "win" -> OK (sprintf "%s wins the game!" name)
    | _ -> BAD_REQUEST "Unknown action"

// Routes

let route_join =
    request (fun r ->
        match r.queryParam "name" with
        | Choice1Of2 name -> OK (sprintf "%s joined the game." name)
        | Choice2Of2 msg -> BAD_REQUEST msg)

let route_join_post =
    request (fun r ->
        match r.formData "name" with
        | Choice1Of2 name -> OK (sprintf "%s joined the game." name)
        | Choice2Of2 msg -> BAD_REQUEST msg)
    
let route_start_game =
    request (fun r ->
        match enough_players with
        | true -> start_game
        | _ -> handle_no_game_available true)

let route_play_post =
    request(fun r ->
        match r.formData "name", r.formData "action" with // todo : add domino and position
        | Choice1Of2 name, Choice1Of2 action -> play name action (Domino(2, 5)) (Position(17, 23))
        | _ -> BAD_REQUEST "error")

// App
let app =
  choose
    [ GET >=> choose
        [ path "/hello" >=> OK "Hello GET"
          path "/join" >=> route_join
          path "/start" >=> route_start_game]
      POST >=> choose
        [ path "/hello" >=> OK "Hello POST"
          path "/join" >=> route_join_post
          path "/play" >=> route_play_post ] ]

startWebServer defaultConfig app

// créer un compte
// se connecter (met l'utilisateur en attente)
// démarrer une nouvelle partie (quald il y a 2 joueurs)
// Jouer
//  piocher un domino
//  placer un domino
//  gagner/perdre une partie
//  quitter une partie
//