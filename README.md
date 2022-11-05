[![build][buildstatus_img]][buildstatus]
[![test][teststatus_img]][teststatus]

# TaskSeq

An implementation [`IAsyncEnumerable<'T>`][3] as a `taskSeq` CE for F# with accompanying `TaskSeq` module.

The `IAsyncEnumerable` interface was added to .NET in `.NET Core 3.0` and is part of `.NET Standard 2.1`. The main use-case was for iterative asynchronous enumeration over some resource. For instance, an event stream or a REST API interface with pagination, where each page is a [`MoveNextAsync`][4] call on the [`IAsyncEnumerator<'T>`][5] given by a call to [`GetAsyncEnumerator()`][6]. It has been relatively challenging to work properly with this type and dealing with each step being asynchronous, and the enumerator implementing [`IAsyncDisposable`][7] as well, which requires careful handling.

## Short-term feature planning

Not necessarily in order of importance:

* [x] Stabilize and battle-test `taskSeq` resumable code. **DONE**
* [x] A growing set of module functions `TaskSeq`, see below for progress. **DONE & IN PROGRESS**
* [ ] Packaging and publishing on Nuget, **PLANNED: 13 November 2022**.
* [x] Add `Async` variants for functions taking HOF arguments. **DONE**
* [ ] Expand surface area based on `AsyncSeq`.
* [ ] User requests?

## Implementation progress

### `taskSeq` CE

The _resumable state machine_ backing the `taskSeq` CE is now finished. Focus is now on adding functionality there, like adding more useful overloads for `yield` and `let!`. Suggestions are welcome!

### `TaskSeq` module functions

We are working hard on getting a full set of module functions on `TaskSeq` that can be used with `IAsyncEnumerable` sequences. Our guide is the set of F# `Seq` functions in F# Core and, where applicable, the functions provided from `AsyncSeq`. Each implemented function is documented through XML doc comments to provide the necessary context-sensitive help.

The following is the progress report:

| Implemented   | `Seq` function     | `TaskSeq` function | Extra `async` variant | Remarks                                   |
|---------------|--------------------|--------------------|-----------------------|-------------------------------------------|
|               | `allPairs`         | `allPairs`         |                       |                                           |
|               | `append`           | `append`           |                       |                                           |
|               | `average`          | `averageBy`        | `averageByAsync`      |                                           |
|               | `cache`            | `cache`            |                       |                                           |
| &#x2705;      | `cast`             | `cast`             |                       |                                           |
| &#x2705;      |                    | `box`              |                       |                                           |
| &#x2705;      |                    | `unbox`            |                       |                                           |
| &#x2705;      | `choose`           | `choose`           | `chooseAsync`         |                                           |
|               | `chunkBySize`      | `chunkBySize`      |                       |                                           |
| &#x2705;      | `collect`          | `collect`          | `collectAsync`        |                                           |
|               | `compareWith`      | `compareWith`      | `compareWithAsync`    |                                           |
| &#x2705;      | `concat`           | `concat`           |                       |                                           |
| &#x2705;      | `contains`         | `contains`         |                       |                                           |
|               | `delay`            | `delay`            |                       |                                           |
|               | `distinct`         | `distinct`         |                       |                                           |
|               | `distinctBy`       | `dictinctBy`       | `distinctByAsync`     |                                           |
| &#x2705;      | `empty`            | `empty`            |                       |                                           |
| &#x2705;      | `exactlyOne`       | `exactlyOne`       |                       |                                           |
|               | `except`           | `except`           |                       |                                           |
| &#x2705;      | `exists`           | `exists`           |                       |                                           |
|               | `exists2`          | `exists2`          |                       |                                           |
| &#x2705;      | `filter`           | `filter`           | `filterAsync`         |                                           |
| &#x2705;      | `find`             | `find`             | `findAsync`           |                                           |
| _not planned_ | `findBack`         |                    |                       | _iteration from back not possible_        |
| &#x2705;      | `findIndex`        | `findIndex`        | `findIndexAsync`      |                                           |
| _not planned_ | `findIndexBack`    | n/a                | n/a                   | _iteration from back not possible_        |
| &#x2705;      | `fold`             | `fold`             | `foldAsync`           |                                           |
|               | `fold2`            | `fold2`            | `fold2Async`          |                                           |
| _not planned_ | `foldBack`         |                    |                       | _iteration from back not possible_        |
| _not planned_ | `foldBack2`        |                    |                       | _iteration from back not possible_        |
|               | `forall`           | `forall`           | `forallAsync`         |                                           |
|               | `forall2`          | `forall2`          | `forall2Async`        |                                           |
| _maybe_       | `groupBy`          | `groupBy`          | `groupByAsync`        |                                           |
| &#x2705;      | `head`             | `head`             |                       |                                           |
| &#x2705;      | `indexed`          | `indexed`          |                       |                                           |
| &#x2705;      | `init`             | `init`             | `initAsync`           |                                           |
| &#x2705;      | `initInfinite`     | `initInfinite`     | `initInfiniteAsync`   |                                           |
|               | `insertAt`         | `insertAt`         |                       |                                           |
|               | `insertManyAt`     | `insertManyAt`     |                       |                                           |
| &#x2705;      | `isEmpty`          | `isEmpty`          |                       |                                           |
| &#x2705;      | `item`             | `item`             |                       |                                           |
| &#x2705;      | `iter`             | `iter`             | `iterAsync`           |                                           |
|               | `iter2`            | `iter2`            | `iter2Async`          |                                           |
| &#x2705;      | `iteri`            | `iteri`            | `iteriAsync`          |                                           |
|               | `iteri2`           | `iteri2`           | `iteri2Async`         |                                           |
| &#x2705;      | `last`             | `last`             |                       |                                           |
| &#x2705;      | `length`           | `length`           |                       |                                           |
| &#x2705;      |                    | `lengthBy`         | `lengthByAsyn`        |                                           |
| &#x2705;      | `map`              | `map`              | `mapAsync`            |                                           |
|               | `map2`             | `map2`             | `map2Async`           |                                           |
|               | `map3`             | `map3`             | `map3Async`           |                                           |
|               | `mapFold`          | `mapFold`          | `mapFoldAsync`        |                                           |
| _not planned_ | `mapFoldBack`      |                    |                       | _iteration from back not possible_        |
| &#x2705;      | `mapi`             | `mapi`             | `mapiAsync`           |                                           |
|               | `mapi2`            | `mapi2`            | `mapi2Async`          |                                           |
|               | `max`              | `max`              |                       |                                           |
|               | `maxBy`            | `maxBy`            | `maxByAsync`          |                                           |
|               | `min`              | `min`              |                       |                                           |
|               | `minBy`            | `minBy`            | `minByAsync`          |                                           |
| &#x2705;      | `ofArray`          | `ofArray`          |                       |                                           |
| &#x2705;      |                    | `ofAsyncArray`     |                       |                                           |
| &#x2705;      |                    | `ofAsyncList`      |                       |                                           |
| &#x2705;      |                    | `ofAsyncSeq`       |                       |                                           |
| &#x2705;      | `ofList`           | `ofList`           |                       |                                           |
| &#x2705;      |                    | `ofTaskList`       |                       |                                           |
| &#x2705;      |                    | `ofResizeArray`    |                       |                                           |
| &#x2705;      |                    | `ofSeq`            |                       |                                           |
| &#x2705;      |                    | `ofTaskArray`      |                       |                                           |
| &#x2705;      |                    | `ofTaskList`       |                       |                                           |
| &#x2705;      |                    | `ofTaskSeq`        |                       |                                           |
|               | `pairwise`         | `pairwise`         |                       |                                           |
|               | `permute`          | `permute`          | `permuteAsync`        |                                           |
| &#x2705;      | `pick`             | `pick`             | `pickAsync`           |                                           |
| _not planned_ | `readOnly`         |                    |                       | _all TaskSeq sequences are readonly_      |
|               | `reduce`           | `reduce`           | `reduceAsync`         |                                           |
| _not planned_ | `reduceBack`       |                    |                       | _iteration from back not possible_        |
|               | `removeAt`         | `removeAt`         |                       |                                           |
|               | `removeManyAt`     | `removeManyAt`     |                       |                                           |
|               | `replicate`        | `replicate`        |                       |                                           |
| _maybe_       | `rev`              |                    |                       |                                           |
|               | `scan`             | `scan`             | `scanAsync`           |                                           |
| _not planned_ | `scanBack`         |                    |                       | _iteration from back not possible_        |
|               | `singleton`        | `singleton`        |                       |                                           |
|               | `skip`             | `skip`             |                       |                                           |
|               | `skipWhile`        | `skipWhile`        | `skipWhileAsync`      |                                           |
| _maybe_       | `sort`             |                    |                       |                                           |
| _maybe_       | `sortBy`           |                    |                       |                                           |
| _maybe_       | `sortByAscending`  |                    |                       |                                           |
| _maybe_       | `sortByDescending` |                    |                       |                                           |
| _maybe_       | `sortWith`         |                    |                       |                                           |
|               | `splitInto`        | `splitInto`        |                       |                                           |
|               | `sum`              | `sum`              |                       |                                           |
|               | `sumBy`            | `sumBy`            | `sumByAsync`          |                                           |
|               | `tail`             | `tail`             |                       |                                           |
|               | `take`             | `take`             |                       |                                           |
|               | `takeWhile`        | `takeWhile`        | `takeWhileAsync`      |                                           |
| &#x2705;      | `toArray`          | `toArray`          | `toArrayAsync`        |                                           |
| &#x2705;      |                    | `toIList`          | `toIListAsync`        |                                           |
| &#x2705;      | `toList`           | `toList`           | `toListAsync`         |                                           |
| &#x2705;      |                    | `toResizeArray`    | `toResizeArrayAsync`  |                                           |
| &#x2705;      |                    | `toSeq`            | `toSeqAsync`          |                                           |
|               |                    | […]                |                       | _more convenience conversions considered_ |
| _maybe_       | `transpose`        |                    |                       |                                           |
|               | `truncate`         | `truncate`         |                       |                                           |
| &#x2705;      | `tryExactlyOne`    | `tryExactlyOne`    | `tryExactlyOneAsync`  |                                           |
| &#x2705;      | `tryFind`          | `tryFind`          | `tryFindAsync`        |                                           |
| _not planned_ | `tryFindBack`      |                    |                       | _iteration from back not possible_        |
| &#x2705;      | `tryFindIndex`     | `tryFindIndex`     | `tryFindIndexAsync`   |                                           |
| _not planned_ | `tryFindIndexBack` |                    |                       | _iteration from back not possible_        |
| &#x2705;      | `tryHead`          | `tryHead`          |                       |                                           |
| &#x2705;      | `tryItem`          | `tryItem`          |                       |                                           |
| &#x2705;      | `tryLast`          | `tryLast`          |                       |                                           |
| &#x2705;      | `tryPick`          | `tryPick`          | `tryPickAsync`        |                                           |
|               | `unfold`           | `unfold`           | `unfoldAsync`         |                                           |
|               | `updateAt`         | `updateAt`         |                       |                                           |
|               | `where`            | `where`            | `whereAsync`          |                                           |
|               | `windowed`         | `windowed`         |                       |                                           |
| &#x2705;      | `zip`              | `zip`              |                       |                                           |
|               | `zip3`             | `zip3`             |                       |                                           |
|               |                    | `zip4`             |

### Futher reading `IAsyncEnumerable`

* A good C#-based introduction [can be found in this blog][8].
* [An MSDN article][9] written shortly after it was introduced.
* Converting a `seq` to an `IAsyncEnumerable` [demo gist][10] as an example, though `TaskSeq` contains many more utility functions and uses a slightly different approach.
* If you're looking for using `IAsyncEnumerable` with `async` and not `task`, the excellent [`AsyncSeq`][11] library should be used. While `TaskSeq` is intended to consume `async` just like `task` does, it won't create an `AsyncSeq` type (at least not yet). If you want classic Async and parallelism, you should get this library instead.

### Futher reading on resumable state machines

* A state machine from a monadic perspective in F# [can be found here][12], which works with the pre-F# 6.0 non-resumable internals.
* The [original RFC for F# 6.0 on resumable state machines][13] 
* The [original RFC for introducing `task`][14] to F# 6.0.
* A [pre F# 6.0 `TaskBuilder`][15] that motivated the `task` CE later added to F# Core.
* [MSDN Documentation on `task`][16] and [`async`][17].

### Further reading on computation expressions

* [Docs on MSDN][18] form a good summary and starting point.
* Arguably the best [step-by-step tutorial to using and building computation expressions][19] by Scott Wlaschin.

## Building & testing

TLDR: just run `build`. Or load the `sln` file in Visual Studio or VS Code and compile.

### Prerequisites

* .NET 6 or .NET 7 Preview
* F# 6.0 or 7.0 compiler
* To use `build.cmd`, the `dotnet` command must be accessible from your path.

Just check-out this repo locally. Then, from the root of the repo, you can do:

### Build the solution

```
build [build] [release|debug]
```

With no arguments, defaults to `release`.

### Run the tests

```
build test [release|debug]
```

With no arguments, defaults to `release`. By default, all tests are output to the console. If you don't want that, you can use `--logger console;verbosity=summary`.
Furthermore, no TRX file is generated and the `--blame-xxx` flags aren't set.

### Run the CI command

```
build ci [release|debug]
```

With no arguments, defaults to `release`. This will run `dotnet test` with the `--blame-xxx` settings enabled to [prevent hanging tests][1] caused by 
an [xUnit runner bug][2].

There are no special CI environment variables that need to be set for running this locally.

### Advanced

You can pass any additional options that are valid for `dotnet test` and `dotnet build` respectively. However, 
these cannot be the very first argument, so you should either use `build build --myadditionalOptions fizz buzz`, or
just specify the build-kind, i.e. this is fine:

```
build debug --verbosity detailed
build test --logger console;verbosity=summary
```

At this moment, additional options cannot have quotes in them.

Command modifiers, like `release` and `debug`, can be specified with `-` or `/` if you so prefer: `dotnet build /release`.

### Get help (duh!)

```
build help
```

For more info, see this PR: https://github.com/abelbraaksma/TaskSeq/pull/29.


## In progress!!!

It's based on [Don Symes `taskSeq.fs`][20]
but expanded with useful utility functions and a few extra binding overloads.

## Current set of `TaskSeq` utility functions

The following is the current surface area of the `TaskSeq` utility functions. This is just a dump of the signatures with doc comments
to be used as a quick ref.

```f#
module TaskSeq =
    open System.Collections.Generic
    open System.Threading.Tasks
    open FSharp.Control.TaskSeqBuilders

    /// Initialize an empty taskSeq.
    val empty<'T> : taskSeq<'T>

    /// <summary>
    /// Returns <see cref="true" /> if the task sequence contains no elements, <see cref="false" /> otherwise.
    /// </summary>
    val isEmpty: taskSeq: taskSeq<'T> -> Task<bool>

    /// Returns taskSeq as an array. This function is blocking until the sequence is exhausted and will properly dispose of the resources.
    val toList: t: taskSeq<'T> -> 'T list

    /// Returns taskSeq as an array. This function is blocking until the sequence is exhausted and will properly dispose of the resources.
    val toArray: taskSeq: taskSeq<'T> -> 'T[]

    /// Returns taskSeq as a seq, similar to Seq.cached. This function is blocking until the sequence is exhausted and will properly dispose of the resources.
    val toSeqCached: taskSeq: taskSeq<'T> -> seq<'T>

    /// Unwraps the taskSeq as a Task<array<_>>. This function is non-blocking.
    val toArrayAsync: taskSeq: taskSeq<'T> -> Task<'T[]>

    /// Unwraps the taskSeq as a Task<list<_>>. This function is non-blocking.
    val toListAsync: taskSeq: taskSeq<'T> -> Task<'T list>

    /// Unwraps the taskSeq as a Task<ResizeArray<_>>. This function is non-blocking.
    val toResizeArrayAsync: taskSeq: taskSeq<'T> -> Task<ResizeArray<'T>>

    /// Unwraps the taskSeq as a Task<IList<_>>. This function is non-blocking.
    val toIListAsync: taskSeq: taskSeq<'T> -> Task<IList<'T>>

    /// Unwraps the taskSeq as a Task<seq<_>>. This function is non-blocking,
    /// exhausts the sequence and caches the results of the tasks in the sequence.
    val toSeqCachedAsync: taskSeq: taskSeq<'T> -> Task<seq<'T>>

    /// Create a taskSeq of an array.
    val ofArray: array: 'T[] -> taskSeq<'T>

    /// Create a taskSeq of a list.
    val ofList: list: 'T list -> taskSeq<'T>

    /// Create a taskSeq of a seq.
    val ofSeq: sequence: seq<'T> -> taskSeq<'T>

    /// Create a taskSeq of a ResizeArray, aka List.
    val ofResizeArray: data: ResizeArray<'T> -> taskSeq<'T>

    /// Create a taskSeq of a sequence of tasks, that may already have hot-started.
    val ofTaskSeq: sequence: seq<#Task<'T>> -> taskSeq<'T>

    /// Create a taskSeq of a list of tasks, that may already have hot-started.
    val ofTaskList: list: #Task<'T> list -> taskSeq<'T>

    /// Create a taskSeq of an array of tasks, that may already have hot-started.
    val ofTaskArray: array: #Task<'T> array -> taskSeq<'T>

    /// Create a taskSeq of a seq of async.
    val ofAsyncSeq: sequence: seq<Async<'T>> -> taskSeq<'T>

    /// Create a taskSeq of a list of async.
    val ofAsyncList: list: Async<'T> list -> taskSeq<'T>

    /// Create a taskSeq of an array of async.
    val ofAsyncArray: array: Async<'T> array -> taskSeq<'T>

    /// Iterates over the taskSeq applying the action function to each item. This function is non-blocking
    /// exhausts the sequence as soon as the task is evaluated.
    val iter: action: ('T -> unit) -> taskSeq: taskSeq<'T> -> Task<unit>

    /// Iterates over the taskSeq applying the action function to each item. This function is non-blocking,
    /// exhausts the sequence as soon as the task is evaluated.
    val iteri: action: (int -> 'T -> unit) -> taskSeq: taskSeq<'T> -> Task<unit>

    /// Iterates over the taskSeq applying the async action to each item. This function is non-blocking
    /// exhausts the sequence as soon as the task is evaluated.
    val iterAsync: action: ('T -> #Task<unit>) -> taskSeq: taskSeq<'T> -> Task<unit>

    /// Iterates over the taskSeq, applying the async action to each item. This function is non-blocking,
    /// exhausts the sequence as soon as the task is evaluated.
    val iteriAsync: action: (int -> 'T -> #Task<unit>) -> taskSeq: taskSeq<'T> -> Task<unit>

    /// Maps over the taskSeq, applying the mapper function to each item. This function is non-blocking.
    val map: mapper: ('T -> 'U) -> taskSeq: taskSeq<'T> -> taskSeq<'U>

    /// Maps over the taskSeq with an index, applying the mapper function to each item. This function is non-blocking.
    val mapi: mapper: (int -> 'T -> 'U) -> taskSeq: taskSeq<'T> -> taskSeq<'U>

    /// Maps over the taskSeq, applying the async mapper function to each item. This function is non-blocking.
    val mapAsync: mapper: ('T -> #Task<'U>) -> taskSeq: taskSeq<'T> -> taskSeq<'U>

    /// Maps over the taskSeq with an index, applying the async mapper function to each item. This function is non-blocking.
    val mapiAsync: mapper: (int -> 'T -> #Task<'U>) -> taskSeq: taskSeq<'T> -> taskSeq<'U>

    /// Applies the given function to the items in the taskSeq and concatenates all the results in order.
    val collect: binder: ('T -> #taskSeq<'U>) -> taskSeq: taskSeq<'T> -> taskSeq<'U>

    /// Applies the given function to the items in the taskSeq and concatenates all the results in order.
    val collectSeq: binder: ('T -> #seq<'U>) -> taskSeq: taskSeq<'T> -> taskSeq<'U>

    /// Applies the given async function to the items in the taskSeq and concatenates all the results in order.
    val collectAsync: binder: ('T -> #Task<'TSeqU>) -> taskSeq: taskSeq<'T> -> taskSeq<'U> when 'TSeqU :> taskSeq<'U>

    /// Applies the given async function to the items in the taskSeq and concatenates all the results in order.
    val collectSeqAsync: binder: ('T -> #Task<'SeqU>) -> taskSeq: taskSeq<'T> -> taskSeq<'U> when 'SeqU :> seq<'U>

    /// <summary>
    /// Returns the first element of the <see cref="IAsyncEnumerable" />, or <see cref="None" /> if the sequence is empty.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the sequence is empty.</exception>
    val tryHead: taskSeq: taskSeq<'T> -> Task<'T option>

    /// <summary>
    /// Returns the first element of the <see cref="IAsyncEnumerable" />.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the sequence is empty.</exception>
    val head: taskSeq: taskSeq<'T> -> Task<'T>

    /// <summary>
    /// Returns the last element of the <see cref="IAsyncEnumerable" />, or <see cref="None" /> if the sequence is empty.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the sequence is empty.</exception>
    val tryLast: taskSeq: taskSeq<'T> -> Task<'T option>

    /// <summary>
    /// Returns the last element of the <see cref="IAsyncEnumerable" />.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the sequence is empty.</exception>
    val last: taskSeq: taskSeq<'T> -> Task<'T>

    /// <summary>
    /// Returns the nth element of the <see cref="IAsyncEnumerable" />, or <see cref="None" /> if the sequence
    /// does not contain enough elements, or if <paramref name="index" /> is negative.
    /// Parameter <paramref name="index" /> is zero-based, that is, the value 0 returns the first element.
    /// </summary>
    val tryItem: index: int -> taskSeq: taskSeq<'T> -> Task<'T option>

    /// <summary>
    /// Returns the nth element of the <see cref="IAsyncEnumerable" />, or <see cref="None" /> if the sequence
    /// does not contain enough elements, or if <paramref name="index" /> is negative.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the sequence has insufficient length or
    /// <paramref name="index" /> is negative.</exception>
    val item: index: int -> taskSeq: taskSeq<'T> -> Task<'T>

    /// <summary>
    /// Returns the only element of the task sequence, or <see cref="None" /> if the sequence is empty of
    /// contains more than one element.
    /// </summary>
    val tryExactlyOne: source: taskSeq<'T> -> Task<'T option>

    /// <summary>
    /// Returns the only element of the task sequence.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the input sequence does not contain precisely one element.</exception>
    val exactlyOne: source: taskSeq<'T> -> Task<'T>

    /// <summary>
    /// Applies the given function <paramref name="chooser" /> to each element of the task sequence. Returns
    /// a sequence comprised of the results "x" for each element where
    /// the function returns <c>Some(x)</c>.
    /// If <paramref name="chooser" /> is asynchronous, consider using <see cref="TaskSeq.chooseAsync" />.
    /// </summary>
    val choose: chooser: ('T -> 'U option) -> source: taskSeq<'T> -> taskSeq<'U>

    /// <summary>
    /// Applies the given asynchronous function <paramref name="chooser" /> to each element of the task sequence. Returns
    /// a sequence comprised of the results "x" for each element where
    /// the function returns <see cref="Some(x)" />.
    /// If <paramref name="chooser" /> does not need to be asynchronous, consider using <see cref="TaskSeq.choose" />.
    /// </summary>
    val chooseAsync: chooser: ('T -> #Task<'U option>) -> source: taskSeq<'T> -> taskSeq<'U>

    /// <summary>
    /// Returns a new collection containing only the elements of the collection
    /// for which the given <paramref name="predicate" /> function returns <see cref="true" />.
    /// If <paramref name="predicate" /> is asynchronous, consider using <see cref="TaskSeq.filterAsync" />.
    /// </summary>
    val filter: predicate: ('T -> bool) -> source: taskSeq<'T> -> taskSeq<'T>

    /// <summary>
    /// Returns a new collection containing only the elements of the collection
    /// for which the given asynchronous function <paramref name="predicate" /> returns <see cref="true" />.
    /// If <paramref name="predicate" /> does not need to be asynchronous, consider using <see cref="TaskSeq.filter" />.
    /// </summary>
    val filterAsync: predicate: ('T -> #Task<bool>) -> source: taskSeq<'T> -> taskSeq<'T>

    /// <summary>
    /// Applies the given function <paramref name="chooser" /> to successive elements of the task sequence
    /// in <paramref name="source" />, returning the first result where the function returns <see cref="Some(x)" />.
    /// If <paramref name="chooser" /> is asynchronous, consider using <see cref="TaskSeq.tryPickAsync" />.
    /// </summary>
    val tryPick: chooser: ('T -> 'U option) -> source: taskSeq<'T> -> Task<'U option>

    /// <summary>
    /// Applies the given asynchronous function <paramref name="chooser" /> to successive elements of the task sequence
    /// in <paramref name="source" />, returning the first result where the function returns <see cref="Some(x)" />.
    /// If <paramref name="chooser" /> does not need to be asynchronous, consider using <see cref="TaskSeq.tryPick" />.
    /// </summary>
    val tryPickAsync: chooser: ('T -> #Task<'U option>) -> source: taskSeq<'T> -> Task<'U option>

    /// <summary>
    /// Returns the first element of the task sequence in <paramref name="source" /> for which the given function
    /// <paramref name="predicate" /> returns <see cref="true" />. Returns <see cref="None" /> if no such element exists.
    /// If <paramref name="predicate" /> is asynchronous, consider using <see cref="TaskSeq.tryFindAsync" />.
    /// </summary>
    val tryFind: predicate: ('T -> bool) -> source: taskSeq<'T> -> Task<'T option>

    /// <summary>
    /// Returns the first element of the task sequence in <paramref name="source" /> for which the given asynchronous function
    /// <paramref name="predicate" /> returns <see cref="true" />. Returns <see cref="None" /> if no such element exists.
    /// If <paramref name="predicate" /> does not need to be asynchronous, consider using <see cref="TaskSeq.tryFind" />.
    /// </summary>
    val tryFindAsync: predicate: ('T -> #Task<bool>) -> source: taskSeq<'T> -> Task<'T option>


    /// <summary>
    /// Applies the given function <paramref name="chooser" /> to successive elements of the task sequence
    /// in <paramref name="source" />, returning the first result where the function returns <see cref="Some(x)" />.
    /// If <paramref name="chooser" /> is asynchronous, consider using <see cref="TaskSeq.pickAsync" />.
    /// <exception cref="KeyNotFoundException">Thrown when every item of the sequence
    /// evaluates to <see cref="None" /> when the given function is applied.</exception>
    /// </summary>
    val pick: chooser: ('T -> 'U option) -> source: taskSeq<'T> -> Task<'U>

    /// <summary>
    /// Applies the given asynchronous function <paramref name="chooser" /> to successive elements of the task sequence
    /// in <paramref name="source" />, returning the first result where the function returns <see cref="Some(x)" />.
    /// If <paramref name="chooser" /> does not need to be asynchronous, consider using <see cref="TaskSeq.pick" />.
    /// <exception cref="KeyNotFoundException">Thrown when every item of the sequence
    /// evaluates to <see cref="None" /> when the given function is applied.</exception>
    /// </summary>
    val pickAsync: chooser: ('T -> #Task<'U option>) -> source: taskSeq<'T> -> Task<'U>

    /// <summary>
    /// Returns the first element of the task sequence in <paramref name="source" /> for which the given function
    /// <paramref name="predicate" /> returns <see cref="true" />.
    /// If <paramref name="predicate" /> is asynchronous, consider using <see cref="TaskSeq.findAsync" />.
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown if no element returns <see cref="true" /> when
    /// evaluated by the <paramref name="predicate" /> function.</exception>
    val find: predicate: ('T -> bool) -> source: taskSeq<'T> -> Task<'T>

    /// <summary>
    /// Returns the first element of the task sequence in <paramref name="source" /> for which the given
    /// asynchronous function <paramref name="predicate" /> returns <see cref="true" />.
    /// If <paramref name="predicate" /> does not need to be asynchronous, consider using <see cref="TaskSeq.find" />.
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown if no element returns <see cref="true" /> when
    /// evaluated by the <paramref name="predicate" /> function.</exception>
    val findAsync: predicate: ('T -> #Task<bool>) -> source: taskSeq<'T> -> Task<'T>

    /// <summary>
    /// Zips two task sequences, returning a taskSeq of the tuples of each sequence, in order. May raise ArgumentException
    /// if the sequences are or unequal length.
    /// </summary>
    /// <exception cref="ArgumentException">The sequences have different lengths.</exception>
    val zip: taskSeq1: taskSeq<'T> -> taskSeq2: taskSeq<'U> -> IAsyncEnumerable<'T * 'U>

    /// <summary>
    /// Applies the function <paramref name="folder" /> to each element in the task sequence,
    /// threading an accumulator argument of type <paramref name="'State" /> through the computation.
    /// If the accumulator function <paramref name="folder" /> is asynchronous, consider using <see cref="TaskSeq.foldAsync" />.
    /// </summary>
    val fold: folder: ('State -> 'T -> 'State) -> state: 'State -> taskSeq: taskSeq<'T> -> Task<'State>

    /// <summary>
    /// Applies the asynchronous function <paramref name="folder" /> to each element in the task sequence,
    /// threading an accumulator argument of type <paramref name="'State" /> through the computation.
    /// If the accumulator function <paramref name="folder" /> does not need to be asynchronous, consider using <see cref="TaskSeq.fold" />.
    /// </summary>
    val foldAsync: folder: ('State -> 'T -> #Task<'State>) -> state: 'State -> taskSeq: taskSeq<'T> -> Task<'State>

```

[buildstatus]: https://github.com/abelbraaksma/TaskSeq/actions/workflows/main.yaml
[buildstatus_img]: https://github.com/abelbraaksma/TaskSeq/actions/workflows/main.yaml/badge.svg
[teststatus]: https://github.com/abelbraaksma/TaskSeq/actions/workflows/test.yaml
[teststatus_img]: https://github.com/abelbraaksma/TaskSeq/actions/workflows/test.yaml/badge.svg

[1]: https://github.com/abelbraaksma/TaskSeq/issues/25
[2]: https://github.com/xunit/xunit/issues/2587
[3]: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1?view=net-7.0
[4]: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerator-1.movenextasync?view=net-7.0
[5]: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerator-1?view=net-7.0
[6]: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1.getasyncenumerator?view=net-7.0
[7]: https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable?view=net-7.0
[8]: https://stu.dev/iasyncenumerable-introduction/
[9]: https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/november/csharp-iterating-with-async-enumerables-in-csharp-8
[10]: https://gist.github.com/akhansari/d88812b742aa6be1c35b4f46bd9f8532
[11]: https://fsprojects.github.io/FSharp.Control.AsyncSeq/AsyncSeq.html
[12]: http://blumu.github.io/ResumableMonad/TheResumableMonad.html
[13]: https://github.com/fsharp/fslang-design/blob/main/FSharp-6.0/FS-1087-resumable-code.md
[14]: https://github.com/fsharp/fslang-design/blob/main/FSharp-6.0/FS-1097-task-builder.md
[15]: https://github.com/rspeele/TaskBuilder.fs/
[16]: https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/task-expressions
[17]: https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/async-expressions
[18]: https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/computation-expressions
[19]: https://fsharpforfunandprofit.com/series/computation-expressions/
[20]: https://github.com/dotnet/fsharp/blob/d5312aae8aad650f0043f055bb14c3aa8117e12e/tests/benchmarks/CompiledCodeBenchmarks/TaskPerf/TaskPerf/taskSeq.fs
