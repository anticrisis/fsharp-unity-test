namespace Library1

open UnityEngine

[<Struct>]
type V3 = {x: int; y: int; z: int} 

type V3Struct = 
    struct
        val x: int
        val y: int
        val z: int
        new(x, y, z)  = { x = x; y = y; z = z }
    end

module Foo =
    let debug s = printfn "%s" s
    let unityDebug s = Debug.Log(s)

    let test debug =
        let testSet0 = Set.empty<V3>
        let testSet1 = testSet0.Add({x = 123; y = 123; z = 123})
        let testSet2 = testSet1.Add({x = 999; y = 999; z = 999})

        // succeeds under Unity
        if testSet0.Contains {x = 0; y = 0; z = 1}
        then debug (sprintf "FAIL test: testSet0")
        else debug (sprintf "SUCCESS test: testSet0")

        // fails under Unity
        if testSet1.Contains {x = 0; y = 0; z = 1}
        then debug (sprintf "FAIL test: testSet1")
        else debug (sprintf "SUCCESS test: testSet1")

        // fails under Unity
        if testSet2.Contains {x = 0; y = 0; z = 1}
        then debug (sprintf "FAIL test: testSet2")
        else debug (sprintf "SUCCESS test: testSet2")

    let test2 debug =
        let testSet0 = Set.empty<V3Struct>
        let testSet1 = testSet0.Add(V3Struct(123, 123, 123))
        let testSet2 = testSet1.Add(V3Struct(999, 999, 999))

        // succeeds under Unity
        if testSet0.Contains (V3Struct(0, 0, 1))
        then debug (sprintf "FAIL test2: testSet0")
        else debug (sprintf "SUCCESS test2: testSet0")

        // fails under Unity
        if testSet1.Contains (V3Struct(0, 0, 1))
        then debug (sprintf "FAIL test2: testSet1")
        else debug (sprintf "SUCCESS test2: testSet2")

        // fails under Unity
        if testSet2.Contains (V3Struct(0, 0, 1))
        then debug (sprintf "FAIL test2: testSet2")
        else debug (sprintf "SUCCESS test2: testSet2")

    let test3 debug =
        // this is the comparer that is used by FSharp.Core.Set
        let comparer = LanguagePrimitives.FastGenericComparer<V3Struct>

        // fails under Unity
        if comparer.Compare(V3Struct(123, 123, 123), V3Struct(999, 999, 999)) = 0
        then debug (sprintf "FAIL test3: comparer")
        else debug (sprintf "SUCCESS test3: comparer")

    let workaround debug =
        let testSet0 = Set.empty<V3>
        let testSet1 = testSet0.Add({x = 123; y = 123; z = 123})
        let testSet2 = testSet1.Add({x = 999; y = 999; z = 999})

        // succeeds under Unity
        if Set.exists (fun x -> x = {x = 0; y = 0; z = 1}) testSet0
        then debug (sprintf "FAIL workaround: testSet0")
        else debug (sprintf "SUCCESS workaround: testSet0")

        // fails under Unity
        if Set.exists (fun x -> x = {x = 0; y = 0; z = 1}) testSet1
        then debug (sprintf "FAIL workaround: testSet1")
        else debug (sprintf "SUCCESS workaround: testSet1")

        // fails under Unity
        if Set.exists (fun x -> x = {x = 0; y = 0; z = 1}) testSet2
        then debug (sprintf "FAIL workaround: testSet2")
        else debug (sprintf "SUCCESS workaround: testSet2")


type Class1() = 
    member this.X = "F#"

    member this.Hello () =
        Debug.Log(sprintf "Hello world!")

        Foo.test Foo.unityDebug
        Foo.test2 Foo.unityDebug 
        Foo.test3 Foo.unityDebug
        Foo.workaround Foo.unityDebug
