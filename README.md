# Plantilla .NET para Integraciones con Proveedores de Hoteles

Este proyecto proporciona una plantilla en .NET para facilitar la integración con proveedores de hoteles, utilizando llamadas HTTP directas en lugar de WCF para los servicios SOAP. Esto ofrece mayor flexibilidad y rendimiento en muchos escenarios.

## Características

*   **Enfoque HTTP:** Utiliza `HttpClient` para realizar llamadas directas a APIs SOAP y REST, evitando la sobrecarga de WCF.
*   **Soporte SOAP:** Implementación para consumir servicios SOAP sin depender de la generación de proxies con `svcutil` para la invocación directa a través de http con el header SoapAction
*   **Estructura modular:** Diseño que facilita la extensión para soportar múltiples proveedores y tipos de integraciones.
*   **Manejo de errores:** Incluye manejo robusto de excepciones y respuestas HTTP.
*   **Código limpio y comentado:** Facilita la comprensión y el mantenimiento del código.

## Requisitos

*   .NET 8.0 o superior.
*   Un editor de código como Visual Studio o Visual Studio Code.

## Configuración

1.  **Clonar el repositorio:**
    ```bash
    git clone [URL]
    ```
2.  **Navegar al directorio del proyecto:**
    ```bash
    cd NombreDelRepositorio
    ```

## Uso

### Integración SOAP

Tradicionalmente, para consumir un servicio SOAP en .NET, se utilizaba WCF y la herramienta `svcutil` para generar un proxy. Este proyecto adopta un enfoque diferente, realizando llamadas HTTP directas y manipulando los mensajes SOAP manualmente.

1.  **Generación de Modelos (Opcional pero Recomendado):**

    Si bien no es estrictamente necesario, generar las clases de modelo a partir del WSDL puede facilitar el trabajo con los datos. Puedes usar `dotnet-svcutil` para este propósito:

    *   **Instalar la herramienta globalmente:**

        ```bash
        dotnet tool install --global dotnet-svcutil
        ```

    *   **Ejecutar `dotnet-svcutil` con la URL del WSDL:**

        ```bash
        dotnet-svcutil [URL_del_WSDL] -o Models/NombreDelServicio.cs
        ```

        Por ejemplo:

        ```bash
        dotnet-svcutil [https://dev.hotelston.com/ws/HotelServiceV2?wsdl](https://dev.hotelston.com/ws/HotelServiceV2?wsdl) -o Models/HotelstonService.cs
        ```

        Esto generará las clases C# correspondientes al WSDL en la carpeta `Models/` del proyecto.

2.  **Llamadas SOAP con HttpClient (Ejemplo):**

    A continuación, se muestra un ejemplo de cómo realizar una llamada SOAP usando `HttpClient`:

    ```csharp
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    // ... otros usings y clases ...

    public async Task<string> LlamarServicioSoapAsync(string soapAction, string mensajeSoap)
    {
        using (var cliente = new HttpClient())
        {
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml")); // o application/soap+xml si el servicio lo requiere
            cliente.DefaultRequestHeaders.Add("SOAPAction", soapAction); // Cabecera SOAPAction

            var contenido = new StringContent(mensajeSoap, Encoding.UTF8, "text/xml"); // Cuerpo del mensaje SOAP
            var respuesta = await cliente.PostAsync("URL_del_Servicio", contenido);

            if (!respuesta.IsSuccessStatusCode)
            {
                 //TODO: Manejar errores de la llamada
                var errorContent = await respuesta.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error en la llamada SOAP: {respuesta.StatusCode}. Contenido del error: {errorContent}");
            }
            return await respuesta.Content.ReadAsStringAsync();
        }
    }
    //Ejemplo de uso
    //string resultado = await LlamarServicioSoapAsync("GetHotelAvailability","<soapenv:Envelope xmlns:soapenv=\"[http://schemas.xmlsoap.org/soap/envelope/](http://schemas.xmlsoap.org/soap/envelope/)\" xmlns:hot=\"[http://tempuri.org/](http://tempuri.org/)\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <hot:GetHotelAvailability>\r\n         <hot:HotelId>123</hot:HotelId>\r\n      </hot:GetHotelAvailability>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>");

    ```

    **TODO:** Implementar el manejo adecuado de errores, como el análisis del mensaje de error SOAP y el lanzamiento de excepciones personalizadas.

### Integración REST (A Implementar)

**TODO:** Agregar ejemplos de cómo realizar llamadas REST usando `HttpClient` y cómo deserializar las respuestas JSON o XML.

## Contribuciones

Las contribuciones son bienvenidas. Por favor, abre un issue o un pull request para proponer cambios o mejoras.

## Licencia

[MIT]
