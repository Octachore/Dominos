namespace Backend.Tests

open ObjectModel
open Repository
open GameLogic
open Xunit
open System
open System.IO
open EqualityComparers

type Tests() = 
    let init() = if File.Exists file then File.Delete file
    
    [<Fact>]
    let ``repository write writes good data``() =
        init()
        // arrange
        let text = Guid.NewGuid().ToString()

        // act
        write text

        // assert
        let content = File.ReadAllText file
        Assert.Equal(text, content)

    [<Fact>]
    let ``repository write_game writes new game``() =
        init()
        // arrange
        let board = [
            ({v1=5; v2=1}, {x1=4894; y1=64565; x2=9469; y2=4654})
            ({v1=4; v2=2}, {x1=123; y1=84; x2=0; y2=42})
        ]
        let main_deck = [
            {v1=5; v2=5}
            {v1=5; v2=6}
            {v1=3; v2=1}
            {v1=1; v2=1}
        ]
        let deck1 = [
            {v1=0; v2=4}
        ]
        let deck2 = [
            {v1=2; v2=4}
            {v1=3; v2=3}
            {v1=1; v2=5}
        ]
        let game = {id=12; player1="Alice"; player2="Bob"; board=board; main_deck=main_deck; deck1=deck1; deck2=deck2}

        // act
        write_game game

        // assert
        let expected = "[
  {
    \"id\": 12,
    \"player1\": \"Alice\",
    \"player2\": \"Bob\",
    \"board\": [
      {
        \"Item1\": {
          \"v1\": 5,
          \"v2\": 1
        },
        \"Item2\": {
          \"x1\": 4894,
          \"y1\": 64565,
          \"x2\": 9469,
          \"y2\": 4654
        }
      },
      {
        \"Item1\": {
          \"v1\": 4,
          \"v2\": 2
        },
        \"Item2\": {
          \"x1\": 123,
          \"y1\": 84,
          \"x2\": 2,
          \"y2\": 42
        }
      }
    ],
    \"main_deck\": [
      {
        \"v1\": 5,
        \"v2\": 5
      },
      {
        \"v1\": 5,
        \"v2\": 6
      },
      {
        \"v1\": 3,
        \"v2\": 1
      },
      {
        \"v1\": 1,
        \"v2\": 1
      }
    ],
    \"deck1\": [
      {
        \"v1\": 0,
        \"v2\": 4
      }
    ],
    \"deck2\": [
      {
        \"v1\": 2,
        \"v2\": 4
      },
      {
        \"v1\": 3,
        \"v2\": 3
      },
      {
        \"v1\": 1,
        \"v2\": 5
      }
    ]
  }
]" 
        let retrieved_game = read_game 12
        Assert.Equal(game, retrieved_game.Value, new GameEqualityComparer()) 


    [<Fact>]
    let ``repository write_game updates game``() =
        init()
        // arrange
        let board = [
            ({v1=5; v2=1}, {x1=4894; y1=64565; x2=9469; y2=4654})
            ({v1=4; v2=2}, {x1=123; y1=84; x2=0; y2=42})
        ]
        let main_deck = [
            {v1=5; v2=5}
            {v1=5; v2=6}
            {v1=3; v2=1}
            {v1=1; v2=1}
        ]
        let deck1 = [
            {v1=0; v2=4}
        ]
        let deck2 = [
            {v1=2; v2=4}
            {v1=3; v2=3}
            {v1=1; v2=5}
        ]
        let game = {id=12; player1="Alice"; player2="Bob"; board=board; main_deck=main_deck; deck1=deck1; deck2=deck2}

        // act
        write_game game
        write_game { game with player2="Eve" }

        // assert
        let retrieved_game = read_game 12
        Assert.Equal({ game with player2="Eve" }, retrieved_game.Value, new GameEqualityComparer())

    [<Fact>]
    let ``repository read_game reads game``()=
        init()
        // arrange
        let text = "[
  {
    \"id\": 12,
    \"player1\": \"Alice\",
    \"player2\": \"Bob\",
    \"board\": [
      {
        \"Item1\": {
          \"v1\": 5,
          \"v2\": 1
        },
        \"Item2\": {
          \"x1\": 4894,
          \"y1\": 64565,
          \"x2\": 9469,
          \"y2\": 4654
        }
      },
      {
        \"Item1\": {
          \"v1\": 4,
          \"v2\": 2
        },
        \"Item2\": {
          \"x1\": 123,
          \"y1\": 84,
          \"x2\": 2,
          \"y2\": 42
        }
      }
    ],
    \"main_deck\": [
      {
        \"v1\": 5,
        \"v2\": 5
      },
      {
        \"v1\": 5,
        \"v2\": 6
      },
      {
        \"v1\": 3,
        \"v2\": 1
      },
      {
        \"v1\": 1,
        \"v2\": 1
      }
    ],
    \"deck1\": [
      {
        \"v1\": 0,
        \"v2\": 4
      }
    ],
    \"deck2\": [
      {
        \"v1\": 2,
        \"v2\": 4
      },
      {
        \"v1\": 3,
        \"v2\": 3
      },
      {
        \"v1\": 1,
        \"v2\": 5
      }
    ]
  },
  {
    \"id\": 56,
    \"player1\": \"Charlie\",
    \"player2\": \"Eve\",
    \"board\": [
      {
        \"Item1\": {
          \"v1\": 1,
          \"v2\": 1
        },
        \"Item2\": {
          \"x1\": 4,
          \"y1\": 0,
          \"x2\": 3,
          \"y2\": 9
        }
      },
      {
        \"Item1\": {
          \"v1\": 3,
          \"v2\": 5
        },
        \"Item2\": {
          \"x1\": 751,
          \"y1\": 7987,
          \"x2\": 8,
          \"y2\": 0
        }
      }
    ],
    \"main_deck\": [],
    \"deck1\": [
      {
        \"v1\": 1,
        \"v2\": 4
      },
      {
        \"v1\": 12,
        \"v2\": 6
      }
    ],
    \"deck2\": [
      {
        \"v1\": 1,
        \"v2\": 1
      }
    ]
  }
]"
        File.WriteAllText(file, text)
        
        // act
        let game1 = read_game 12
        let game2 = read_game 56

        // assert
        let game1_board = [
            ({v1=5; v2=1}, {x1=4894; y1=64565; x2=9469; y2=4654})
            ({v1=4; v2=2}, {x1=123; y1=84; x2=2; y2=42})
        ]
        let game2_board = [
            ({v1=1; v2=1}, {x1=4; y1=0; x2=3; y2=9})
            ({v1=3; v2=5}, {x1=751; y1=7987; x2=8; y2=0})
        ]
        let game1_main_deck = [
            {v1=5; v2=5}
            {v1=5; v2=6}
            {v1=3; v2=1}
            {v1=1; v2=1}
        ]
        let game2_main_deck = [
        ]
        let game1_deck1 = [
            {v1=0; v2=4}
        ]
        let game2_deck1 = [
            {v1=1; v2=4}
            {v1=12; v2=6}
        ]
        let game1_deck2 = [
            {v1=2; v2=4}
            {v1=3; v2=3}
            {v1=1; v2=5}
        ]
        let game2_deck2 = [
            {v1=1; v2=1}
        ]
        Assert.True(game1.IsSome)
        Assert.Equal(12, game1.Value.id)
        Assert.Equal("Alice", game1.Value.player1)
        Assert.Equal("Bob", game1.Value.player2)
        Assert.Equal(game1_board, game1.Value.board, new BoardEqualityComparer())
        Assert.Equal<Deck>(game1_main_deck, game1.Value.main_deck, new DeckEqualityComparer())
        Assert.Equal<Deck>(game1_deck1, game1.Value.deck1, new DeckEqualityComparer())
        Assert.Equal<Deck>(game1_deck2, game1.Value.deck2, new DeckEqualityComparer())
        
        Assert.True(game2.IsSome)
        Assert.Equal(56, game2.Value.id)
        Assert.Equal("Charlie", game2.Value.player1)
        Assert.Equal("Eve", game2.Value.player2)
        Assert.Equal<Board>(game2_board, game2.Value.board, new BoardEqualityComparer())
        Assert.Equal<Deck>(game2_main_deck, game2.Value.main_deck, new DeckEqualityComparer())
        Assert.Equal<Deck>(game2_deck1, game2.Value.deck1, new DeckEqualityComparer())
        Assert.Equal<Deck>(game2_deck2, game2.Value.deck2, new DeckEqualityComparer())
        

    [<Fact>]
    let ``repository write_game writes multiple games``() =
        init()
        // arrange
        let game1_board = [
            ({v1=5; v2=1}, {x1=4894; y1=64565; x2=0; y2=0})
            ({v1=4; v2=2}, {x1=123; y1=84; x2=3; y2=2})
        ]
        let game2_board = [
            ({v1=5; v2=1}, {x1=4894; y1=64565; x2=9469; y2=4654})
            ({v1=4; v2=2}, {x1=123; y1=84; x2=0; y2=42})
        ]
        let game1_main_deck = [
            {v1=5; v2=5}
            {v1=5; v2=6}
            {v1=3; v2=1}
            {v1=1; v2=1}
        ]
        let game2_main_deck = [
        ]
        let game1_deck1 = [
            {v1=0; v2=4}
        ]
        let game2_deck1 = [
            {v1=1; v2=4}
            {v1=12; v2=6}
        ]
        let game1_deck2 = [
            {v1=2; v2=4}
            {v1=3; v2=3}
            {v1=1; v2=5}
        ]
        let game2_deck2 = [
            {v1=1; v2=1}
        ]

        let game1 = {id=12; player1="Alice"; player2="Bob"; board=game1_board; main_deck=game1_main_deck; deck1=game1_deck1; deck2=game1_deck2}
        let game2 = {id=56; player1="Charlie"; player2="Eve"; board=game2_board; main_deck=game2_main_deck; deck1=game2_deck1; deck2=game2_deck2}

        // act
        write_game game1
        write_game game2
        write_game { game1 with player2="John" }

        // assert
        let retrieved_game1 = read_game 12
        let retrieved_game2 = read_game 56
        Assert.Equal({ game1 with player2="John" }, retrieved_game1.Value, new GameEqualityComparer())
        Assert.Equal(game2, retrieved_game2.Value, new GameEqualityComparer())

    [<Fact>]
    let ``game logic start new games``() =
        init()
        Assert.True(true)

        // act
        start_game "Alice" "Bob" |> ignore 
        start_game "Alice" "Bob" |> ignore
        start_game "Charlie" "Eve" |> ignore

        // assert
        let expected_game1 = {id=0; player1="Alice"; player2="Bob"; board=[]; main_deck=initial_deck; deck1=[]; deck2=[]}
        let expected_game2 = {id=1; player1="Alice"; player2="Bob"; board=[]; main_deck=initial_deck; deck1=[]; deck2=[]}
        let expected_game3 = {id=2; player1="Charlie"; player2="Eve"; board=[]; main_deck=initial_deck; deck1=[]; deck2=[]}

        let games = read_games()
        Assert.Equal(3, games.Length)
        Assert.Equal(expected_game1, games.Item 2, new GameEqualityComparer())
        Assert.Equal(expected_game2, games.Item 1, new GameEqualityComparer())
        Assert.Equal(expected_game3, games.Item 0, new GameEqualityComparer())

    [<Fact>]
    let ``game logic generates dominos``() =
        let value = generate_dominos()
        Assert.Equal(28, value.Length)

    [<Fact>]
    let ``game logic checks place is free``() =
        // arrange
        let board = [
            ({v1=0; v2=1}, {x1=10; y1=10; x2=10; y2=11})
            ({v1=1; v2=5}, {x1=11; y1=11; x2=12; y2=11})
        ]

        // act
        let res1 = place_is_free board {x1=10; y1=9; x2=10; y2=10}
        let res2 = place_is_free board {x1=12; y1=11; x2=13; y2=11}
        let res3 = place_is_free board {x1=10; y1=12; x2=10; y2=13}
        let res4 = place_is_free board {x1=10; y1=13; x2=10; y2=12}

        Assert.False(res1)
        Assert.False(res2)
        Assert.True(res3)
        Assert.True(res4)

    [<Fact>]
    let ``game logic checks place is legal``() =
        // arrange
        let board = [
            ({v1=0; v2=1}, {x1=10; y1=10; x2=10; y2=11})
            ({v1=1; v2=5}, {x1=11; y1=11; x2=12; y2=11})
        ]
        let domino = {v1=1; v2=6}

        // act
        let res1 = place_is_legal board domino {x1=10; y1=9; x2=10; y2=10}
        let res2 = place_is_legal board domino {x1=12; y1=11; x2=13; y2=11}
        let res3 = place_is_legal board domino {x1=10; y1=12; x2=10; y2=13}
        let res4 = place_is_legal board domino {x1=10; y1=13; x2=10; y2=12}

        Assert.False(res1)
        Assert.True(res2)
        Assert.True(res3)
        Assert.False(res4)

    [<Fact>]
    let ``game logic places domino``() =
        init()
        // arrange
        let board = [
            ({v1=0; v2=1}, {x1=10; y1=10; x2=10; y2=11})
            ({v1=1; v2=5}, {x1=11; y1=11; x2=12; y2=11})
        ]
        let game = {id=0; player1="Alice"; player2="Bob"; board=board; main_deck=initial_deck; deck1=[]; deck2=[]}
        let domino = {v1=1; v2=6}
        let game_with_empty_board = { game with id=1; board=[] }
        write_game game
        write_game game_with_empty_board

        // act
        let res1 = valid_place game domino {x1=10; y1=9; x2=10; y2=10}
        let res2 = valid_place game domino {x1=12; y1=11; x2=13; y2=11}
        let res3 = valid_place game domino {x1=10; y1=12; x2=10; y2=13}
        let res4 = valid_place game domino {x1=10; y1=13; x2=10; y2=12}
        let res5 = valid_place game_with_empty_board domino {x1=10; y1=9; x2=10; y2=10}

        Assert.Equal(Failure("Illegal"), res1)
        Assert.Equal(Failure("Illegal"), res2)
        Assert.Equal(Success(sprintf "Placed a domino of value %i:%i on the board at position %i:%i/%i:%i" domino.v1 domino.v2 10 12 10 13), res3)
        Assert.Equal(Failure("Illegal"), res4)
        Assert.Equal(Success(sprintf "Placed a domino of value %i:%i on the board at position %i:%i/%i:%i" domino.v1 domino.v2 10 9 10 10), res5)

        let saved_game1 = (read_game 0).Value
        let saved_game2 = (read_game 1).Value
        Assert.Equal(3, saved_game1.board.Length)
        Assert.Equal(1, saved_game2.board.Length)
