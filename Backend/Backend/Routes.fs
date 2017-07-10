module Routes

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open MetaGameLogic
open GameLogic
open System

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
        match r.formData "name", r.formData "action", r.formData "domino-v1", r.formData "domino-v2", r.formData "position-x", r.formData "position-y" with
        | Choice1Of2 name, Choice1Of2 action, Choice2Of2 _, Choice2Of2 _, Choice2Of2 _, Choice2Of2 _ -> play name action {v1= -1; v2= -1} {x= -1; y= -1}
        | Choice1Of2 name, Choice1Of2 action,  Choice1Of2 v1 , Choice1Of2 v2, Choice1Of2 x, Choice1Of2 y ->  play name action {v1= v1 |> int; v2= v2 |> int} {x= x |> int; y= y |> int}
        | _ -> BAD_REQUEST "error")

let routes = [ GET >=> choose
                 [ path "/hello" >=> OK "Hello GET"
                   path "/join" >=> route_join
                   path "/start" >=> route_start_game ]
               POST >=> choose
                 [ path "/hello" >=> OK "Hello POST"
                   path "/join" >=> route_join_post
                   path "/play" >=> route_play_post ] ]