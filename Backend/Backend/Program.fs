module Main

open Suave
open Routes
    

// App
let app = choose routes

[<EntryPoint>]
let main args = 
    startWebServer defaultConfig app
    0
