# Ejemplo de comunicaci�n con socket
El Cliente: 

	- pide los datos de la operacion
	- crea el objeto Operacion para su posterior env�o
	- lo serializa y lo pasa a un array de bytes
	- lo envia

El Servidor:
	
	- Recibe el objeto serializado
	- Lo deserializa
	- Realiza la operaci�n y crea otro objeto con el resultado
	- Vuelve a serializar el objeto y lo env�a al cliente