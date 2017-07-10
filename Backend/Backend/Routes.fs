module Routes

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open MetaGameLogic
open GameLogic

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

let routes = [ GET >=> choose
                 [ path "/hello" >=> OK "Hello GET"
                   path "/join" >=> route_join
                   path "/start" >=> route_start_game]
               POST >=> choose
                 [ path "/hello" >=> OK "Hello POST"
                   path "/join" >=> route_join_post
                   path "/play" >=> route_play_post ] ]