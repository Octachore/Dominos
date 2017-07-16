import Html exposing (Html, div, input, text)
import Html.Attributes exposing (..)
import Html.Events exposing (onInput)
import String



main =
  Html.beginnerProgram
    { model = model
    , view = view
    , update = update
    }



-- MODEL


type alias Model =
  { content : String
  }


model : Model
model =
  Model ""



-- UPDATE


type Msg
  = Change of String
  | Change2 of Int

toto : String -> Msg
toto s =
  Change s

update : Msg -> Model -> Model
update msg model =
  case msg of
    Change newContent ->
      { model | content = newContent }



-- VIEW


view : Model -> Html Msg
view model =
  div []
    [ input [ placeholder "Text to reverse", onInput toto ] []
    , div [] [ text (String.reverse model.content) ]
    ]
