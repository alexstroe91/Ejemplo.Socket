# Ejemplo de comunicación con socket
El Cliente: 

	- pide los datos de la operacion
	- crea el objeto Operacion para su posterior envío
	- lo serializa y lo pasa a un array de bytes
	- lo envia

El Servidor:
	
	- Recibe el objeto serializado
	- Lo deserializa
	- Realiza la operación y crea otro objeto con el resultado
	- Vuelve a serializar el objeto y lo envía al cliente