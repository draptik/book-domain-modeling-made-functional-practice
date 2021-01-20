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
        VipStatus = "Normal"
    }
    
    let addressDto = {
        AddressDto.AddressLine1 = "Evergreen Terrace 1"
        AddressLine2 = ""
        AddressLine3 = ""
        AddressLine4 = ""
        City = "Springfield"
        ZipCode = "12345"
        State = "NY"
        Country = "USA"
    }
    
    let orderFormLineDto = {
        OrderFormLineDto.OrderLineId = "1"
        ProductCode = "W1234"
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

    // Arrange
    let body = createOrderFormDto |> toJson

    let request = {
        Action = ""
        Uri = ""
        Body = body
    }
    
    let (result: HttpResponse) =
        async {
            // Act
            let! response = placeOrderApi request
            return response
        } |> Async.RunSynchronously
        
    // Assert
    let expectedReturnBody = "[{\"OrderAcknowledgmentSent\":{\"OrderId\":\"1\",\"EmailAddress\":\"homer.simpson@aol.com\"}},{\"ShippableOrderPlaced\":{\"OrderId\":\"1\",\"ShippingAddress\":{\"AddressLine1\":\"Evergreen Terrace 1\",\"AddressLine2\":null,\"AddressLine3\":null,\"AddressLine4\":null,\"City\":\"Springfield\",\"ZipCode\":\"12345\",\"State\":\"NY\",\"Country\":\"USA\"},\"ShipmentLines\":[{\"ProductCode\":\"W1234\",\"Quantity\":1.0}],\"Pdf\":{\"Name\":\"Order1.pdf\",\"Bytes\":\"\"}}},{\"BillableOrderPlaced\":{\"OrderId\":\"1\",\"BillingAddress\":{\"AddressLine1\":\"Evergreen Terrace 1\",\"AddressLine2\":null,\"AddressLine3\":null,\"AddressLine4\":null,\"City\":\"Springfield\",\"ZipCode\":\"12345\",\"State\":\"NY\",\"Country\":\"USA\"},\"AmountToBill\":10.0}}]"

    result.HttpStatusCode =! 200
    result.Body =! expectedReturnBody
