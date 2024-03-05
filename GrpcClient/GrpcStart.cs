using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    public class GrpcStart
    {
        public  GrpcStart()
        {
            try
            {
               
                    var channel = GrpcChannel.ForAddress("https://localhost:7012");

                    var authenticationClient = new Authentication.AuthenticationClient(channel);
                    var authenticationResponse = authenticationClient.Authenticate(new AuthenticationRequest
                    {
                        Email = "admin@admin.com",
                        Password = "123",
                    });
                    Console.WriteLine($"Server Cevabı : {authenticationResponse.AccessToken} - {authenticationResponse.ExpiresIn}\n\n\n\n");
                    
                var calculationClient = new Calculation.CalculationClient(channel);
                var header = new Metadata();
                header.Add("Authorization", $"Bearer {authenticationResponse.AccessToken}");
                var sumResult = calculationClient.Add(new InputNumbers { Number1 = 8 ,Number2=15},header);
                Console.WriteLine($"Add Metodu Server Cevabı : {sumResult.Result}\n\n\n\n");

                try
                {
                    var header2 = new Metadata();
                    header2.Add("Authorization", $"Bearer test123");
                    var subtractResult = calculationClient.Subtract(new InputNumbers { Number1 = 32, Number2 = 14 }, header2);

                    Console.WriteLine($"Subtract Metodu Server Cevabı : {subtractResult.Result}\n\n\n\n");
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Subtract Metodu Server Hata Cevabı : {ex.Message}\n\n\n\n");
                }

                try
                {
                    var multiplyResult = calculationClient.Multiply(new InputNumbers { Number1 = 32, Number2 = 14 },header);
                    Console.WriteLine($"multiplyResult Metodu Server Cevabı : {multiplyResult.Result}\n\n\n\n");
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"multiplyResult Metodu Server Hata Cevabı : {ex.Message}\n\n\n\n");
                }


            }
            catch (Exception ex)
            {

                Console.WriteLine( $"Hata {ex.Message}");
            }
            
        }
    }
}
