// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System
open Library1


[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    Foo.test Foo.debug
    Foo.test2 Foo.debug
    Foo.test3 Foo.debug
    Foo.workaround Foo.debug

    Console.WriteLine();
    Console.WriteLine("Press any key...");

    Console.ReadKey(true) |> ignore

    0 // return an integer exit code
