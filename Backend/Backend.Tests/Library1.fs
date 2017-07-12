namespace Backend.Tests

open Repository
open ObjectModel
open Xunit
open System
open System.IO

type Class1() = 
            
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
            ({v1=5; v2=1}, {x=4894; y=64565})
            ({v1=4; v2=2}, {x=123; y=84})
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
          \"x\": 4894,
          \"y\": 64565
        }
      },
      {
        \"Item1\": {
          \"v1\": 4,
          \"v2\": 2
        },
        \"Item2\": {
          \"x\": 123,
          \"y\": 84
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
        let content = File.ReadAllText file
        Assert.Equal(expected, content)


    [<Fact>]
    let ``repository write_game updates game``() =
        init()
        // arrange
        let board = [
            ({v1=5; v2=1}, {x=4894; y=64565})
            ({v1=4; v2=2}, {x=123; y=84})
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
        let expected = "[
  {
    \"id\": 12,
    \"player1\": \"Alice\",
    \"player2\": \"Eve\",
    \"board\": [
      {
        \"Item1\": {
          \"v1\": 5,
          \"v2\": 1
        },
        \"Item2\": {
          \"x\": 4894,
          \"y\": 64565
        }
      },
      {
        \"Item1\": {
          \"v1\": 4,
          \"v2\": 2
        },
        \"Item2\": {
          \"x\": 123,
          \"y\": 84
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
        let content = File.ReadAllText file
        Assert.Equal(expected, content)

    [<Fact>]
    let ``repository read_game reads game``()=
        init()
        // arrange
        let text = "[
  {
    \"id\": 12,
    \"player1\": \"Alice\",
    \"player2\": \"John\",
    \"board\": [
      {
        \"Item1\": {
          \"v1\": 5,
          \"v2\": 1
        },
        \"Item2\": {
          \"x\": 4894,
          \"y\": 64565
        }
      },
      {
        \"Item1\": {
          \"v1\": 4,
          \"v2\": 2
        },
        \"Item2\": {
          \"x\": 123,
          \"y\": 84
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
          \"v1\": 5,
          \"v2\": 1
        },
        \"Item2\": {
          \"x\": 4894,
          \"y\": 64565
        }
      },
      {
        \"Item1\": {
          \"v1\": 4,
          \"v2\": 2
        },
        \"Item2\": {
          \"x\": 123,
          \"y\": 84
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
            ({v1=5; v2=1}, {x=4894; y=64565})
            ({v1=4; v2=2}, {x=123; y=84})
        ]
        let game2_board = [
            ({v1=1; v2=1}, {x=4; y=0})
            ({v1=3; v2=5}, {x=751; y=7987})
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
        Assert.Equal<Board>(game1_board, game1.Value.board)
        Assert.Equal<Deck>(game1_main_deck, game1.Value.main_deck)
        Assert.Equal<Deck>(game1_deck1, game1.Value.deck1)
        Assert.Equal<Deck>(game1_deck2, game1.Value.deck2)
        
        Assert.True(game2.IsSome)
        Assert.Equal(56, game2.Value.id)
        Assert.Equal("Charlie", game2.Value.player1)
        Assert.Equal("Eve", game2.Value.player2)
        Assert.Equal<Board>(game2_board, game2.Value.board)
        Assert.Equal<Deck>(game2_main_deck, game2.Value.main_deck)
        Assert.Equal<Deck>(game2_deck1, game2.Value.deck1)
        Assert.Equal<Deck>(game2_deck2, game2.Value.deck2)
        

    [<Fact>]
    let ``repository write_game writes multiple games``() =
        init()
        // arrange
        let game1_board = [
            ({v1=5; v2=1}, {x=4894; y=64565})
            ({v1=4; v2=2}, {x=123; y=84})
        ]
        let game2_board = [
            ({v1=1; v2=1}, {x=4; y=0})
            ({v1=3; v2=5}, {x=751; y=7987})
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
        let expected = "[
  {
    \"id\": 12,
    \"player1\": \"Alice\",
    \"player2\": \"John\",
    \"board\": [
      {
        \"Item1\": {
          \"v1\": 5,
          \"v2\": 1
        },
        \"Item2\": {
          \"x\": 4894,
          \"y\": 64565
        }
      },
      {
        \"Item1\": {
          \"v1\": 4,
          \"v2\": 2
        },
        \"Item2\": {
          \"x\": 123,
          \"y\": 84
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
          \"v1\": 5,
          \"v2\": 1
        },
        \"Item2\": {
          \"x\": 4894,
          \"y\": 64565
        }
      },
      {
        \"Item1\": {
          \"v1\": 4,
          \"v2\": 2
        },
        \"Item2\": {
          \"x\": 123,
          \"y\": 84
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
        let content = File.ReadAllText file
        Assert.Equal(expected, content)
