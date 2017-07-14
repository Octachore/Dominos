/// <summary>Provides utility functions that are not specific to the game.</summary>
module GenericUtils

let rnd = System.Random()

/// <summary>Checks if all eleme,nts are the same (empty list -> false).</summary>
let all_same list =
    match list with
    | [] -> false
    | head::tail when tail |> List.forall ((=) head) -> true
    | _ -> false

/// <summary>Produces a new list where the elements at position x and y have been swaped.</summary>
let swap x y list =
    list |> List.mapi (fun i v ->
        match i with
        | _ when i = x -> list.[y]
        | _ when i = y -> list.[x]
        | _ -> v)

/// <summary>Produces a new shuffled list.</summary>
let shuffle (list : 'a list) =
    let rec internal_shuffle (list : 'a list) i =
        match i with
        | _ when i >= list.Length ->
            let new_list = swap i (rnd.Next(i, list.Length)) list
            internal_shuffle new_list (i+1)
        | _ -> list
    internal_shuffle list 0

/// <summary>Takes a random subset of count element from a lost.</summary>
let random_subset list count = list |> shuffle |> List.take count