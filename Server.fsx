#r "nuget: Suave, 2.6.1"
#r "nuget: Thoth.Json.Net, 7.1.0"

open System.Net
open Suave
open Suave.CORS
open Suave.Filters
open Suave.Logging
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Suave.Writers
open Thoth.Json.Net

let JSON jsonValue : WebPart =
  let jsonString = jsonValue |> Encode.toString 2
  OK jsonString
  >=> setMimeType "application/json"

let logger = Targets.create Verbose [||]

let corsConfig =
  {
    defaultCORSConfig with
      allowedUris =
        InclusiveOption.All
        // InclusiveOption.Some [ "http://localhost:8080" ]
  }

let app =
  cors corsConfig
  >=> choose [
    GET >=> pathCi "/" >=> OK "Hello, world. "
    POST >=> pathCi "/" >=> JSON (Encode.object [ "message", Encode.string "Posted" ])
    NOT_FOUND "Not found"
  ]
  >=> logStructured logger logFormatStructured

let config =
  {
    defaultConfig with
      bindings =
        [ HttpBinding.create HTTP IPAddress.Loopback 5000us ]
  }

startWebServer config app
