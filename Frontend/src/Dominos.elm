module Dominos exposing (Model, Msg, init, view, update, subscriptions)

import List exposing (..)
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (..)
import Http exposing (..)

main : Program Never Model Msg
main =
    Html.program
        { init = init
        , view = view
        , update = update
        , subscriptions = subscriptions
    }



-- MODEL

type alias Model =
    { player: Player
    , adversary: Player
    , board: Board
    , isMyTurn: Bool
    , currentView: AppViews
    }

type alias Player =
    { name: String
    , dominos: List Domino
    }

type alias Domino =
    { value1: Int
    , value2: Int
    }

type alias Board =
    { dimensions: Vector
    , deck: List Domino
    , placedDominos: List PlacedDomino
    , message: String
    }

type alias Vector =
    { x: Int
    , y: Int
    }

type alias PlacedDomino =
    { position: Vector
    , domino: Domino
    , angle: Int
    }

type AppViews
    = HomeView
    | BoardView



type Msg
    = OnPlayerNameInputLeave String
    | OnJoinClick
    | OnPostJoinResponse (Result Http.Error)



-- UPDATE

update : Msg -> Model -> (Model, Cmd Msg)
update msg model =
    case msg of
        OnPlayerNameInputLeave newName ->
            let
                player = model.player
                newPlayer = { player | name = newName }
            in
                ({ model | player = newPlayer }, Cmd.none)
        
        OnJoinClick ->
            ({ model | currentView = BoardView }, Cmd.none)
        
        OnPostJoinResponse Ok ->
            (model, Cmd.none)
        
        OnPostJoinResponse Err ->
            (model, Cmd.none)



-- VIEW

view : Model -> Html Msg
view model =
    case model.currentView of
        HomeView ->
            div []
                [ input [ type_ "text", placeholder "Enter your name", onInput OnPlayerNameInputLeave ] []
                , button [ onClick OnJoinClick ] [ text "Join" ]
                ]
        
        BoardView ->
            div []
                [ div [] [ text "BoardView" ]
                ]



-- SUB

subscriptions : Model -> Sub Msg
subscriptions model =
    Sub.none



-- HTTP

getUrl: String -> String
getUrl uri =
    "http://10.8.110.198:8080" ++ uri

postJoin: String -> Cmd Msg
postJoin name =
    let
        url = getUrl "/join"
        body = Http.jsonBody { name = model.player.name }
    in
        Http.send OnPostJoinResponse (Http.request
            { method = "GET"
            , url = url
            })



-- INIT

init : (Model, Cmd Msg)
init = 
    ({
        player = {
            name = "Toto",
            dominos = []
        },
        adversary = {
            name = "Titi",
            dominos = []
        },
        board = {
            dimensions = { x = 40, y = 40 },
            deck = [],
            placedDominos = [],
            message = "Loading..."
        },
        isMyTurn = False,
        currentView = HomeView
    }, Cmd.none)

