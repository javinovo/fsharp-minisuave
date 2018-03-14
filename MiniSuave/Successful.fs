namespace MiniSuave

module Successful =
  open Http

  let OK content context =
    {context with Response = {Content = content; StatusCode = 200}}
    |> Some
    |> async.Return