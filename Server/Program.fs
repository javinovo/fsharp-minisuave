open MiniSuave.Http
open MiniSuave.Console
open MiniSuave.Successful
open MiniSuave.Combinators
open MiniSuave.Filters

[<EntryPoint>]
let main argv =
    let request = {Route = ""; Type = MiniSuave.Http.GET}
    let response = {Content = ""; StatusCode = 200}
    let context = {Request = request; Response = response}

    let app = Choose [
                GET >=> Path "/hello" >=> OK "Hello GET"
                POST >=> Path "/hello" >=> OK "Hello POST"
                Path "/foo" >=> Choose [                           
                                  GET >=> OK "Foo GET"
                                  POST >=> OK "Foo POST"
                                ]
              ]

    executeInLoop context app

    0