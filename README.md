Descripción:
	Este proyecto es un inicio para partir de una configuración básica que ayude a crear juegos o gemelos digitales en 3D. Está pensado para una navegación básica con vista cenital en perspectiva, de modo que el usuario pueda mover, rotar y hacer zoom de manera más o menos libre. 
  Además, contiene un sistema de bloqueo para la cámara en caso de que el cursor se encuentre sobre elementos de UI, y también un sistema para colocar indicadores 2D anclados a objetos 3D de la escena.

Características:
•	Control de la cámara con el ratón y teclado.
•	Movimiento fluido con zoom adaptable.
•	Orbitación alrededor de un punto de interés.
•	Paneo de la cámara en el espacio 3D.
•	Ajuste dinámico de capas visibles con máscaras de capas. (Falta su uso en ejemplo)
•	Interacción con InputController para capturar eventos de entrada.
•	Bloqueo de movimiento cuando se detectan bloqueadores de UI.

Uso:
1.	Agrega el prefab TwinCamera a la escena.
2.	Configura los parámetros de zoom y rotation y speed a tu gusto.
3.	Asegúrate de que el InputController esté presente en otro objeto de la escena.
4.	Configura los bloqueadores de la cámara agregando el componente TwinCameraBlocker si es necesario en cada uno de los elementos de UI que quieras que bloqueen el uso de la cámara.
5.	Añade TwinDynamicMarker a los elementos 2D que quieres que se anclen a objetos 3D de la escena. Configura en el script el objeto al que se tiene que anclar.
6.	Ejecuta la escena y usa el ratón y el teclado para controlar la cámara.

Autor:
Desarrollado para su uso en Unity por Álvaro Arranz Arnanz
