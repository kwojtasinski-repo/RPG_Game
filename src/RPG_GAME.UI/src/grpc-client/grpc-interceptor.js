/**
 * @constructor
 * @implements {UnaryInterceptor}
 */
 export const SimpleUnaryInterceptor = function() {};

 /** @override */
 SimpleUnaryInterceptor.prototype.intercept = function(request, invoker) {
   // Update the request message before the RPC.
   console.log('invoke interceptor');
   console.log(request);
   const reqMsg = request.getRequestMessage();
   console.log('reqMsg', reqMsg);
//    reqMsg.setMessage('[Intercept request]' + reqMsg.getMessage());
// Add bearer token in header (medadata) it is required to connnect with API
// Transform this into grpc-client setup or sth like that

   // After the RPC returns successfully, update the response.
   return invoker(request).then((response) => {
    // TODO: edit this block of code
     // You can also do something with response metadata here.
     console.log(response.getMetadata());
 
     // Update the response message.
     const responseMsg = response.getResponseMessage();
     responseMsg.setMessage('[Intercept response]' + responseMsg.getMessage());
 
     return response;
   });
 };