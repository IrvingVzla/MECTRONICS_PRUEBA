DOCUMENTO INSTALACIÓN
1.	Tener en cuenta el repositorio https://github.com/IrvingVzla/MECTRONICS_PRUEBA.git se encuentran toda la lógica de la aplicación, por favor abrir “MECTRONICS.sln” con VS2022 para correr los servicios web.

Dentro del archivo repositorio se encuentra el archivo “FrontendReact” se trata del frontend de la aplicación, para esto simplemente bastara simplemente con correr npm run dev en dicha carpeta de la aplicación. 

NOTA: No olvidar correr npm install en FrontendReact.

2.	Tener en cuenta los archivos de bases de datos DDL.sql y DML.sql, se deberá correr en ese mismo orden. Con eso se crearan las bases de datos SQL SERVER y adicionalmente se agregara información a las tablas de MATERIAS Y PROFESORES.

NOTA: Tener en cuenta que se debe crear cuenta por cada estudiante en el formulario de registro.

3.	Debemos configurar las variables de entorno en appsettings.json en el proyecto MECTRONICS (.NET 8)

DefaultConnection

	Sever: Servidor de SQL SERVER
	Database: Base de datos (MECTRONICS por defecto)
	User Id: Usuario de la base de datos
	Password: Contraseña de la base de datos.
JWT
	Issuer: Url del servidor local .NET 8
	Audience: Url del servidor FrontendReact

4.	Debemos configurar las variables de entorno .env en el proyecto FrontendReact

VITE_URL_API: Debemos escribir la url del servicio a consumir, de esta forma.
https://localhost:7243

NOTA: En el proyecto aparecen 2 autores, ambas son mis cuentas (tuve inconvenientes al subir el repositorio)

 
