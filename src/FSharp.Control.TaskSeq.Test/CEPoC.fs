namespace TaskSeq.Tests

open Xunit
open FsUnit.Xunit

open FSharp.Control

///
/// EXAMPLE OF USING BIND AND YIELD AND CUSTOM:
/// https://github.com/cannorin/FSharp.CommandLine/blob/master/src/FSharp.CommandLine/commands.fs
///

module Cmd =
    open FSharp.CommandLine

    let fileOption = commandOption {
        names [ "f"; "file" ]
        description "Name of a file to use (Default index: 0)"
        takes (format("%s:%i").withNames [ "filename"; "index" ])
        takes (format("%s").map (fun filename -> (filename, 0)))
        suggests (fun _ -> [ CommandSuggestion.Files None ])
    }

    type Verbosity =
        | Quiet
        | Normal
        | Full
        | Custom of int

    let verbosityOption = commandOption {
        names [ "v"; "verbosity" ]
        description "Display this amount of information in the log."
        takes (regex @"q(uiet)?$" |> asConst Quiet)
        takes (regex @"n(ormal)?$" |> asConst Quiet)
        takes (regex @"f(ull)?$" |> asConst Full)
        takes (format("custom:%i").map (fun level -> Custom level))
        takes (format("c:%i").map (fun level -> Custom level))
    }

    let mainCommand () =
        let x = CommandBuilder()

        let c1 = command {
            name "main"
            description "The main command."
            opt files in fileOption |> CommandOption.zeroOrMore

            opt verbosity in verbosityOption
                             |> CommandOption.zeroOrExactlyOne
                             |> CommandOption.whenMissingUse Normal

            do printfn "%A, %A" files verbosity
            let! x = command { name "main" }
            name "foo"
            //for x in 1 .. 3 do
            //    yield 42

            //for x in 1 .. 3 do
            //    yield 42

            do printfn "%A, %A" files verbosity
            let! x = command { name "main" }
            description "The main command."

            return "foo"
        }

        command {
            let! x = c1
            name "main"
            description "The main command."
            opt files in fileOption |> CommandOption.zeroOrMore

            opt verbosity in verbosityOption
                             |> CommandOption.zeroOrExactlyOne
                             |> CommandOption.whenMissingUse Normal

            do printfn "%A, %A" files verbosity
            return "foo"
        }

module CEs =

    type M<'T, 'Vars> = {
        Name: string option
        IsMember: bool option
        IsMember2: bool // confirming that shape of the container is not restricted, just need clear default the CE understands
        Members: 'T list
        Variables: 'Vars
    }

    type M<'T> = M<'T, obj List>

    type CE() =

        member _.Zero() : M<'T> = {
            Name = None
            IsMember = None
            IsMember2 = false
            Members = []
            Variables = []
        }

        member _.Combine(model1: M<'T>, model2: M<'T>) : M<'T> =
            let newName =
                match model2.Name with
                | None -> model1.Name
                | res -> res

            let newIsMember =
                match model2.IsMember with
                | None -> model1.IsMember
                | res -> res

            let newIsMember2 =
                match model2.IsMember2 with
                | true -> true
                | res -> res

            {
                Name = newName
                IsMember = newIsMember
                IsMember2 = newIsMember2
                Members = List.append model1.Members model2.Members
                Variables = []
            }

        member _.Delay(f) : M<'T, 'Vars> = f ()

        member _.Run(model: M<'T, 'Vars>) : M<'T> = {
            Name = model.Name
            IsMember = model.IsMember
            IsMember2 = model.IsMember2
            Members = model.Members
            Variables = []
        }

        member this.For(methods, f) : M<'T> =
            let methodList = Seq.toList methods

            match methodList with
            | [] -> this.Zero()
            | [ x ] -> f (x)
            | head :: tail ->
                let mutable headResult = f (head)

                for x in tail do
                    headResult <- this.Combine(headResult, f (x))

                headResult

        member _.Yield(item: 'T) : M<'T> = {
            Name = None
            IsMember = None
            IsMember2 = false
            Members = [ item ]
            Variables = []
        }

        // Only for packing/unpacking the implicit variable space
        member _.Bind(model1: M<'T, 'Vars>, f: ('Vars -> M<'T>)) : M<'T> =
            let model2 = f model1.Variables

            let newName =
                match model2.Name with
                | None -> model1.Name
                | res -> res

            let newIsMember =
                match model2.IsMember with
                | None -> model1.IsMember
                | res -> res

            let newIsMember2 =
                match model2.IsMember2 with
                | true -> true
                | res -> res

            {
                Name = newName
                IsMember = newIsMember
                IsMember2 = newIsMember2
                Members = model1.Members @ model2.Members
                Variables = model2.Variables
            }

        // Only for packing/unpacking the implicit variable space
        member _.Return(varspace: 'Vars) : M<'T, 'Vars> = {
            Name = None
            IsMember = None
            IsMember2 = false
            Members = []
            Variables = varspace
        }

        [<CustomOperation("Name", MaintainsVariableSpaceUsingBind = true)>]
        member _.setName(model: M<'T, 'Vars>, [<ProjectionParameter>] name: ('Vars -> string)) : M<'T, 'Vars> = {
            model with
                Name = let m = Unchecked.defaultof<M<_, _>> in Some(name m.Variables)
        }

        //[<CustomOperation("Name", MaintainsVariableSpaceUsingBind = true)>]
        //member _.setName(model: M<'T, 'Vars * 'Vars2>, [<ProjectionParameter>] name: ('Vars -> string)) : M<'T, 'Vars> = Unchecked.defaultof<_>

        [<CustomOperation("IsMember", MaintainsVariableSpaceUsingBind = true)>]
        member _.setIsMember(model: M<'T, 'Vars>, [<ProjectionParameter>] isMember: ('Vars -> bool)) : M<'T, 'Vars> = {
            model with
                IsMember = Some(isMember model.Variables)
        }

        // We can skip
        [<CustomOperation("IsMember2", MaintainsVariableSpaceUsingBind = true)>]
        member _.setIsMember2(model: M<'T, 'Vars>, [<ProjectionParameter>] isMember: ('Vars -> bool)) : M<'T, 'Vars> = {
            model with
                IsMember2 = isMember model.Variables
        }

        [<CustomOperation("Member", MaintainsVariableSpaceUsingBind = true)>]
        member _.addMember(model: M<'T, 'Vars>, [<ProjectionParameter>] item: ('Vars -> 'T)) : M<'T, 'Vars> = {
            model with
                Members = List.append model.Members [ item model.Variables ]
        }

        // Note, using ParamArray doesn't work in conjunction with ProjectionParameter
        [<CustomOperation("Members", MaintainsVariableSpaceUsingBind = true)>]
        member _.addMembers(model: M<'T, 'Vars>, [<ProjectionParameter>] items: ('Vars -> 'T list)) : M<'T, 'Vars> = {
            model with
                Members = List.append model.Members (items model.Variables)
        }

    let ce = CE()

    module Test =
        let x: M<double> = ce { Name "Fred" }

        let queryTest = query {
            for i in [ 1..10 ] do
                where (i < 10)
                minBy (i + -i)
        //headOrDefault
        //sumBy 10
        }

        let x2: M<double> = ce {
            Name "Fred"
            IsMember true
            IsMember true
            IsMember2 true // Note, I can call this twice without compiler error, but not Name in z5
            IsMember2 true
        }

        let y = ce { 42 }

        let z1 = ce { Member 42 }

        let z2 = ce { Members [ 41; 42 ] }

        let z3 = ce {
            Name "Fred"
            42
        }

        let z4 = ce {
            Member 41
            Member 42
        }

        let z5: M<double> = ce {
            Name("a" + "b")
            Name "b"
        //42 // removing this line results in compiler error
        }

        let z6: M<double> = ce {
            let x = 1
            let y = 2
            let z = 3
            let! foo = Unchecked.defaultof<M<double>>
            // leaving the following two lines in creates an "expected to have type unit" error
            //Name "a"
            //Member 4.0
            () // cannot end with a let!
        }

        let z7: M<double> = ce {
            let a = "a"
            let b = "b"
            let c = "c"
            let! (d: int) = Unchecked.defaultof<_>
            let e = ""
            let! (f: System.Guid) = Unchecked.defaultof<_>
            //Name("b")
            //Member 4.0
            //return ()
            //let x = "b"
            //let! x = Unchecked.defaultof<_>
            return []
        }

        let z8: M<double> = ce {
            let x1 = 1.0
            let y2 = 2.0
            Member(x1 + 3.0)
            Member(y2 + 4.0)
        }

        let z9 = ce {
            let x = 1.0
            Members [ 42.3; 43.1 + x ]
        }
//let empty =
//    ce { }
