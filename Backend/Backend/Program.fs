open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful

let app =
  choose
    [ GET >=> choose
        [ path "/hello" >=> OK "Hello GET"
          path "/goodbye" >=> OK "Good bye GET" ]
      POST >=> choose
        [ path "/hello" >=> OK "Hello POST"
          path "/goodbye" >=> OK "Good bye POST" ] ]

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