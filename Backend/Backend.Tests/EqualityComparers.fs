module EqualityComparers

open ObjectModel
open System.Collections.Generic

let domino_equals x y = x.v1 = y.v1 && x.v2 = y.v2

let position_equals x y = x.x = y.x && x.y = y.y

let board_equals (x : Board) (y : Board) =
    x.Length = y.Length
    && List.zip x y |> List.forall (fun (x : (Domino * Position) * (Domino * Position)) ->
        let tuple1, tuple2 = x
        let domino1, position1 = tuple1
        let domino2, position2 = tuple2

        let titi = 2

        let value = domino_equals domino1 domino2 && position_equals position1 position2
        value
    )
    
let deck_equals (x : Deck) (y : Deck) =
    x.Length = y.Length
    && List.zip x y |> List.forall (fun x ->
        let domino1, domino2 = x
        domino_equals domino1 domino2
    )

let game_equals (x : Game) (y : Game) =
    x.id = y.id
    && x.player1 = y.player1
    && x.player2 = y.player2
    && board_equals x.board y.board
    && deck_equals x.main_deck y.main_deck
    && deck_equals x.deck1 y.deck1
    && deck_equals x.deck2 y.deck2

type DominoEqualityComparer() =
    interface IEqualityComparer<Domino> with
        member this.Equals(x: Domino, y: Domino): bool = 
            domino_equals x y
        member this.GetHashCode(obj: Domino): int = 
            raise (System.NotImplementedException())

type PositionEqualityComparer() =
    interface IEqualityComparer<Position> with
        member this.Equals(x: Position, y: Position): bool = 
            position_equals x y
        member this.GetHashCode(obj: Position): int = 
            raise (System.NotImplementedException())

type BoardEqualityComparer() =
    interface IEqualityComparer<Board> with
        member this.Equals(x: Board, y: Board): bool = 
            board_equals x y
        member this.GetHashCode(obj: Board): int = 
            raise (System.NotImplementedException())

type DeckEqualityComparer() =
    interface IEqualityComparer<Deck> with
        member this.Equals(x: Deck, y: Deck): bool = 
            deck_equals x y
        member this.GetHashCode(obj: Deck): int = 
            raise (System.NotImplementedException())

type GameEqualityComparer() =
    interface IEqualityComparer<Game> with
        member this.Equals(x: Game, y: Game): bool = 
            game_equals x y
        member this.GetHashCode(obj: Game): int = 
            raise (System.NotImplementedException())