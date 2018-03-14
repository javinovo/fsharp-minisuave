namespace MiniSuave

module Filters =
  open Http

  // (Context -> bool) -> Context -> Async<Context option>
  let private iff condition context =
    match condition context with
    | true -> context |> Some |> async.Return
    | false -> None |> async.Return

  let GET = iff (fun context -> context.Request.Type = GET)
  let POST = iff (fun context -> context.Request.Type = POST)
  let Path path = iff (fun context -> context.Request.Route.Equals(path, System.StringComparison.InvariantCultureIgnoreCase))

  // WebPart list -> WebPart
  let rec Choose webparts context = async {
    match webparts with
    | [] -> return None
    | x :: xs ->
      let! result = x context
      match result with
      | Some x -> return Some x
      | None -> return! Choose xs context
  }