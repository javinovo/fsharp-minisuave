namespace MiniSuave

module Console =
  open Http

  let execute inputContext webpart =
    async {
      let! outputContext = webpart inputContext
      match outputContext with
      | Some context ->
        printfn "--------------"
        printfn "Code : %d" context.Response.StatusCode
        printfn "Output : %s" context.Response.Content
        printfn "--------------"
      | None ->
        printfn "No Output"
    } |> Async.RunSynchronously

  let parseRequest (input : System.String) =
    let parts = input.Split(';')
    match parts.Length with
    | 2 ->
        let rawType = parts.[0].Trim().ToUpper()
        let route = parts.[1].Trim()
        match rawType with
        | "GET" -> {Type = GET; Route = route}
        | "POST" -> {Type = POST; Route = route}
        | _ -> failwith "Invalid request"
    | _ -> failwith "Invalid syntax. Must conform to: {http_request_type};{route}"    

  let executeInLoop inputContext webpart =
    let mutable continueLooping = true
    while continueLooping do
      printf "Enter Input Route : "
      let input = System.Console.ReadLine()
      try
        if input = "exit" then
          continueLooping <- false
        else
          let context = {inputContext with Request = parseRequest input}
          execute context webpart
      with
      | ex ->
        printfn "Error : %s" ex.Message