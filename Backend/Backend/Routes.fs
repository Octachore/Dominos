module Routes

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open GameLogic
open ObjectModel

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
    
let route_start_post =
    request (fun r ->
        match enough_players, r.formData "player1", r.formData "player2" with
        | true, Choice1Of2 p1, Choice1Of2 p2 -> start_game p1 p2
        | _ -> handle_no_game_available true)

let handle_play result = 
    match result with
        | Success s -> OK s
        | Failure f -> BAD_REQUEST f

let route_play_post =
    request(fun r ->
        match r.formData "name",
            r.formData "action",
            r.formData "domino-v1",
            r.formData "domino-v2",
            r.formData "position-x1",
            r.formData "position-y1",
            r.formData "position-x2",
            r.formData "position-y2" with
        | Choice1Of2 name, Choice1Of2 action, Choice2Of2 _, Choice2Of2 _, Choice2Of2 _, Choice2Of2 _ ,Choice2Of2 _, Choice2Of2 _ 
            -> handle_play (play name action {v1= -1; v2= -1} {x1= -1; y1= -1; x2= -1; y2= -1})   
        | Choice1Of2 name, Choice1Of2 action,  Choice1Of2 v1 , Choice1Of2 v2, Choice1Of2 x1, Choice1Of2 y1,Choice1Of2 x2, Choice1Of2 y2 
            -> handle_play (play name action {v1= v1 |> int; v2= v2 |> int} {x1= x1 |> int; y1= y1 |> int; x2= x2 |> int; y2= y2 |> int} )
        | _ -> BAD_REQUEST "error")


let routes = [ GET >=> choose
                 [ path "/hello" >=> OK "Hello GET"
                   path "/join" >=> route_join ]
               POST >=> choose
                 [ path "/hello" >=> OK "Hello POST"
                   path "/join" >=> route_join_post
                   path "/play" >=> route_play_post
                   path "/start" >=> route_start_post ] ]