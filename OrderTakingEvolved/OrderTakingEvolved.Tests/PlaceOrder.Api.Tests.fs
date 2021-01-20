module PlaceOrder.Api.Tests

open Xunit
open Swensen.Unquote
open Newtonsoft.Json
open OrderTaking.PlaceOrder
open OrderTaking.PlaceOrder.Api

let createOrderFormDto : OrderFormDto =
    
    let orderId = "1"
    
    let customerInfoDto = {
        CustomerInfoDto.FirstName = "Homer"
        LastName = "Simpson"
        EmailAddress = "homer.simpson@aol.com"
        VipStatus = "unknown"
    }
    
    let addressDto = {
        AddressDto.AddressLine1 = "Evergreen Terrace 1"
        AddressLine2 = ""
        AddressLine3 = ""
        AddressLine4 = ""
        City = "Springfield"
        ZipCode = "12345"
        State = "Some State"
        Country = "USA"
    }
    
    let orderFormLineDto = {
        OrderFormLineDto.OrderLineId = "1"
        ProductCode = "abc"
        Quantity = 1m
    }
    
    let orderFormLineDtos = [orderFormLineDto]
    
    let promotionCode = "promoCode123"
    
    let orderFormDto = {
        OrderFormDto.OrderId = orderId
        CustomerInfo = customerInfoDto
        ShippingAddress = addressDto
        BillingAddress = addressDto
        Lines = orderFormLineDtos
        PromotionCode = promotionCode
    }
    
    orderFormDto

let toJson orderFormDto = JsonConvert.SerializeObject(orderFormDto)

[<Fact>]
let ``Happy case`` () =

    let body = createOrderFormDto |> toJson

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