# FSharp in Unity Sample

This project demonstrates a bug in using FSharp 4.1 with Unity 2017.3.0f3 and
2018.1.0b5.

I ran into a problem using FSharp's `set` collection and `Set.contains` with a
struct type. It doesn't work when running under Unity. This project demonstrates
the problem.

The Visual Studio solution includes a simple Unity project, a `Library1` FSharp
library, and an FSharp `ConsoleApplication1`.

The `Library1` project includes a post-build task to copy `FSharp.Core.dll` and
`System.ValueTuple.dll` into the Unity project's `Assets/Plugins` folder, as well
as copy the test code in `Library1.dll` so that Unity can access it all.

Running the tests from the `ConsoleApplication1` shows they all succeed in a
non-Unity environment. Running the app from within the Unity Editor shows they fail.

I suspect the problem is with `LanguagePrimitives.FastGenericComparer` which for
some reason does not work under Unity's version of the .NET 4.6 framework.

The `master` branch contains the files for Unity 2017.3.0f3 and the
`unity-2018.1.0b5` branch contains the files for that version. (There is no
difference in code -- just changes applied by Unity during the version
updgrade.)

Unity calls the .NET 4.6 support "experimental," so I would expect problems like
this to occur, but it is still disconcerting.

## The tests

The test code is in `Library1/Library1.fs` and should be pretty self explanatory.

There is a `[<Struct>]` record type called `V3` which is a version of the type
that first exhibted the problem in my own project. Thinking the problem had been
introduced recently, I also tested the older form of `struct` types, and this
one is called `V3Struct`.

Both types seem to exhibit the same behavior.

## Build notes (Windows)

1. The `Library1` projects needs a reference to `UnityEngine.dll`. You might
   have to `Add reference` and Browse to wherever your Unity
   `Editor/Data/Managed` folder is.

2. Because the Unity MonoBehaviour script in `NewBehaviourScript.cs` requires
   the FSharp `Library1,` the first successful build of the solution will fail.
   On a successful build of the Library1 project, the `Library1.dll` file (and
   supporting FSharp dlls) are copied into the Unity project. You have to give
   the Unity Editor a second to notice the changes, and then build again. At
   that point, the `NewBehaviourScript.cs` file will be able to find `Library1`.

3. You might have to right-click on `ConsoleApplication1` and select `Set as
   StartUp Project.`

4. I don't know why Visual Studio copies `UnityEngine.dll into the
   `ConsoleApplication1` but it shouldn't be required and shouldn't affect
   things.

## Running the tests

### Console

Right-click the `ConsoleApplication1` in the Solution Explorer and select `Set
as StartUp project.` Then click the `> Start` button on the toolbar.

### Unity

Launch the Unity Editor and open the project folder. You might have to open the
`Main Scene` asset in the `Assets` folder. The scene contains a `GameObject`
which calls the FSharp code in its `Start` method.

The tests output messages to the `Console` tab.

Let me know if you run into problems.

## Workarounds

### Use Set.exists

Using `Set.exists` with a simple `fun x -> x = y` predicate instead of using
`Set.contains` works fine. This implies that the comparer used for `(=)` is not
the same as that used by the `FastGenericComparer`. I'm not sure if that implies
the problem is with Unity's support of .NET 4.6, or with FSharp's implementation
of  `FastGenericComparer`.

### Use tuples instead of struct types

In some of my tests (not shown), using an `int * int * int` tuple instead of the
struct types shown here seemed to avoid the problem as well.


