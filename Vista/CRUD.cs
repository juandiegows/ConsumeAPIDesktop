using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;

namespace Vista
{
    class CRUD
    {
        static HttpClient client = new HttpClient();
        public static String RUTA_API = "http://localhost:51522/";
        public static String ESTADO = "api/Estados";
        public static String EVENTO = "api/Eventos";
        public static String HISTORIAL = "api/Historials";
        public static String TIPO_EVENTO = "api/TipoEventos";

        public CRUD()
        {
            client.BaseAddress = new Uri(CRUD.RUTA_API);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<Respuesta> Get<T>(String Ruta,int ID)
        {

            try
            {
                Respuesta Orespuesta = new Respuesta()
                {
                    Estado = Respuesta.EXITO
                };
                HttpResponseMessage response = await client.GetAsync($"{Ruta}/{ID}");
                if (response.IsSuccessStatusCode)
                {
                    var Ot = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    Orespuesta.Data = Ot;
                    Orespuesta.Mensaje = "Exito";
                    return Orespuesta;
                }
            }
            catch (Exception er)
            {

                return new Respuesta()
                {
                    Estado = Respuesta.ERROR,
                    Data = null,
                    Mensaje = er.Message
                };
            }
            return new Respuesta()
            {
                Estado = Respuesta.ERROR,
                Data = null,
                Mensaje = "sin exito"
            };


        }
        public async Task<Respuesta> GetAll<T>(String Ruta)
        {

            try
            {
                Respuesta Orespuesta = new Respuesta()
                {
                    Estado = Respuesta.EXITO
                };
                HttpResponseMessage response = await client.GetAsync(Ruta);
                if (response.IsSuccessStatusCode)
                {
                    var Ot = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(await response.Content.ReadAsStringAsync());
                    Orespuesta.Data = Ot;
                    Orespuesta.Mensaje = "Exito";
                    return Orespuesta;
                }
            }
            catch (Exception er)
            {

                return new Respuesta()
                {
                    Estado = Respuesta.ERROR,
                    Data = null,
                    Mensaje = er.Message
                };
            }
            return new Respuesta()
            {
                Estado = Respuesta.ERROR,
                Data = null,
                Mensaje = "sin exito"
            };


        }
        public async Task<Respuesta> Post<T>(String Ruta, T E)
        {
            try
            {

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(E);
                httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{RUTA_API}{Ruta}", httpRequestMessage.Content);
                return new Respuesta()
                {
                    Estado = Respuesta.EXITO,
                    Data = response.StatusCode,
                    Mensaje = null
                };
            }
            catch (Exception er)
            {
                return new Respuesta()
                {
                    Estado = Respuesta.EXITO,
                    Data = HttpStatusCode.NotModified,
                    Mensaje = er.Message
                };
            }
        
           
        }

        public async Task<Respuesta> Put<T>(String Ruta, T E, int ID)
        {

            try
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(E);
                httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"{RUTA_API}{Ruta}/{ID}", httpRequestMessage.Content);
                return new Respuesta()
                {
                    Estado = Respuesta.EXITO,
                    Data = response.StatusCode,
                    Mensaje = null
                };

            }
            catch (Exception er)
            {

                return new Respuesta()
                {
                    Estado = Respuesta.EXITO,
                    Data = HttpStatusCode.NotModified,
                    Mensaje =er.Message
                };
            }

        }

        public async Task<Respuesta> Delete(String Ruta, int ID)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"{RUTA_API}{Ruta}/{ID}");
                return new Respuesta()
                {
                    Estado = Respuesta.EXITO,
                    Data = response.StatusCode,
                    Mensaje = null
                };
            }
            catch (Exception er)
            {
                return new Respuesta()
                {
                    Estado = Respuesta.EXITO,
                    Data = HttpStatusCode.NotModified,
                    Mensaje = er.Message
                };

            }
        }
    }
}
