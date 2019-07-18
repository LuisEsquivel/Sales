

namespace Sales.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Sales.Common.Models;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Sales.Helpers;

    public class ApiService
    {


        //ESTA CLASE ESTÁ EN EL PROYECTO DE jZuluaga y es para checar que el usuario tiene conexión a internet
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.TurnOnInternet,
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.NoInternet,
                };
            }

            return new Response
            {
                IsSuccess = true,
            };
        }





        //CREAMOS UN METODO GENERICO PARA CONSUMIR DE CUALQUIER SERVICIO API
        //CUALQUIER LISTA DE UN OBJETO DEBES ASIGNAR T ASI>>> GetList<T>

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)


        {
            try
            {
                //creamos el objeto de tipo HttpClient
                var client = new HttpClient();


                //le pasamos la URL
                client.BaseAddress = new Uri(urlBase);

                //igual a usar un String.Format y concatenar variables
                var url = $"{prefix}{controller}";

                //solicitamos la respuesta
                var response = await client.GetAsync(url);

                // obtenemos el contenido de la respuesta JSON
                var answer = await response.Content.ReadAsStringAsync();


                //si la respuesta es failled
                if (!response.IsSuccessStatusCode)
                {

                    //creamos una nueva respuesta para el usuario
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };

                }

                //procedemos a deserializar el objeto o la respuesta obtenida
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };



            }
            catch (Exception EX)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = EX.Message,
                };


            }
        }



        public async Task<Response> PostList<T>(string urlBase, string prefix, string controller, T model)
        {

            {
                try
                {

                    var request = JsonConvert.SerializeObject(model);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");


                    //creamos el objeto de tipo HttpClient
                    var client = new HttpClient();


                    //le pasamos la URL
                    client.BaseAddress = new Uri(urlBase);

                    //igual a usar un String.Format y concatenar variables
                    var url = $"{prefix}{controller}";

                    //enviamos la respuesta
                    var response = await client.PostAsync(url, content);

                    // obtenemos el contenido de la respuesta JSON
                    var answer = await response.Content.ReadAsStringAsync();


                    //si la respuesta es failled
                    if (!response.IsSuccessStatusCode)
                    {

                        //creamos una nueva respuesta para el usuario
                        return new Response
                        {
                            IsSuccess = false,
                            Message = answer,
                        };

                    }

                    //procedemos a deserializar el objeto o la respuesta obtenida
                    var objeto = JsonConvert.DeserializeObject<T>(answer);
                    return new Response
                    {
                        IsSuccess = true,
                        Result = objeto,
                    };



                }
                catch (Exception EX)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = EX.Message,
                    };


                }
            }


        }
    }
}
