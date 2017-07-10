module Main

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Routes
    

// App
let app = choose routes

[<EntryPoint>]
let main args = 
    startWebServer defaultConfig app
    0
