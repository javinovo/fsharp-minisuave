namespace MiniSuave

module Combinators =
  // WebPart -> WebPart -> WebPart
  let compose first second context = async {
    let! firstContext = first context
    match firstContext with
    | None -> return None
    | Some context -> return! second context
  }

  let (>=>) = compose