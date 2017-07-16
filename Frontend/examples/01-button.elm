import Html exposing (Html, button, div, text)
import Html.Events exposing (onClick)



main =
  Html.beginnerProgram
    { model = model
    , view = view
    , update = update
    }



-- MODEL


type alias Model =
  { counter: Int
  , total: Int
  }


model : Model
model =
  { counter = 0, total = 0 }



-- UPDATE


type Msg
  = Increment
  | Decrement
  | Reset


update : Msg -> Model -> Model
update msg model =
  case msg of
    Increment ->
      { counter = model.counter + 1, total = model.total + 1 }

    Decrement ->
      { counter = model.counter - 1, total = model.total + 1 }
    
    Reset ->
      { counter = 0, total = model.total }



-- VIEW


view : Model -> Html Msg
view model =
  div []
    [ button [ onClick Decrement ] [ text "-" ]
    , div [] [ text (toString model.counter) ]
    , button [ onClick Increment ] [ text "+" ]
    , div [] [ text (toString model.total) ]
    , button [ onClick Reset ] [ text "reset" ]
    ]
