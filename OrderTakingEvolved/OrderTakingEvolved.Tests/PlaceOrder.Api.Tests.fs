module PlaceOrder.Api.Tests

open System
open Xunit
open Swensen.Unquote
open OrderTaking.PlaceOrder.Api

[<Fact>]
let ``Happy case`` () =
    
    
    let body = ""
    let request = {
        Action = ""
        Uri = ""
        Body = body
    }
    
    
    let (result: HttpResponse) =
        async {
            let! response = placeOrderApi request
            return response
        } |> Async.RunSynchronously
        
    result.HttpStatusCode =! 200
    result.Body =! "some body"