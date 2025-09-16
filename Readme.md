 # Aplicación de Demo con Blazor Híbrido

Aplicación de demo con Blazor híbrido (Server + WebAssembly) para la autenticación de un login sin Identity, enviando la IP desde el cliente al servidor y validándola contra un archivo de configuración.

# Descripción

Este proyecto es un ejemplo sencillo de cómo implementar un sistema de login sin necesidad de usar Identity.
La validación se realiza a través de un archivo XML (FileConfigs.xml) que contiene las IPs permitidas.

El cliente introduce su IP en un formulario de login.

La IP se envía al servidor mediante un POST a api/IpConfig/validate.

El servidor comprueba si la IP está en la lista de FileConfigs.xml.

Si es válida, el usuario accede a la página Counter.

