module Main

open Suave
open Routes
    

// App
let app = choose routes

let config = { defaultConfig with bindings = [ (*HttpBinding.createSimple HTTP "10.8.110.198" 8080;*) HttpBinding.createSimple HTTP "127.0.0.1" 8080 ]}

[<EntryPoint>]
let main args = 
    startWebServer config app
    0
