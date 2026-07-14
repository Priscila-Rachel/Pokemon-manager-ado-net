# Pokémon Desktop Manager (`pokemon-manager-ado-net`)

Es una aplicación de escritorio en la cual se utilizo Windows Forms, orientada a la gestión integral de un catálogo de Pokémon. El proyecto fue diseñado aplicando el paradigma de **Programación Orientada a Objetos (POO)** y una **arquitectura en capas**, logrando un código desacoplado, mantenible y escalable.

Se utiliza ADO.NET para conectar la aplicación directamente con la base de datos SQL Server y gestionar la persistencia de los datos.

##  Características y Funcionalidades

* **CRUD Completo:** Gestión total de entidades (Listar, Agregar, Modificar y Eliminar Pokémon).
* **Filtros de Búsqueda Duplicados:**
    * *Filtro Rápido:* Búsqueda ágil en tiempo real desde la interfaz de usuario.
    * *Filtro Avanzado:* Consultas dinámicas parametrizadas directamente contra la base de datos (filtrando por campos numéricos, texto, tipos, etc.).
* **Gestión de Archivos:** Funcionalidad para levantar, manipular y guardar imágenes locales asociadas a cada Pokémon.
* **Validaciones Robustas:** Control de ingreso de datos requeridos, tipos de datos correctos y manejo de estados para evitar excepciones en la UI.
* **Controles Dinámicos:** Implementación avanzada de componentes de Windows Forms (`ComboBox` enlazados a datos, `Label`, `PictureBox`, etc.).

##  Tecnologías y Herramientas

* **Lenguaje:** C# (.NET)
* **Interfaz de Usuario (UI):** Windows Forms (WinForms)
* **Motor de Base de Datos:** SQL Server
* **Gestor de BD:** SQL Server Management Studio (SSMS)
* **Acceso a Datos:** ADO.NET (Conexión nativa mediante comandos y lectores de datos)

---

##  Arquitectura del Proyecto (Estructura en Capas)

Para separar las responsabilidades de la aplicación de manera profesional, el diseño se dividió en tres capas principales:

### 1. Capa de Dominio (Modelos de Objetos)
Contiene las plantillas de los objetos de negocio, representando fielmente el modelo de datos:
* `Pokemon.cs`: Clase principal que modela los atributos de un Pokémon (nombre, descripción, tipos, imagen, etc.).
* `Elemento.cs`: Clase que representa los tipos o elementos de los Pokémon, permitiendo relaciones de objetos complejas.

### 2. Capa de Negocio / Acceso a Datos
Encargada de procesar las reglas del sistema y centralizar la comunicación SQL de forma segura:
* `AccesoDatos.cs`: Clase genérica y reutilizable encargada de centralizar la conexión, apertura, ejecución de queries parametrizadas (`SqlCommand`, `SqlDataReader`) y el correcto cierre de conexiones.
* `PokemonNegocio.cs` y `ElementoNegocio.cs`: Clases de servicio que consumen a `AccesoDatos` para mapear las filas de la base de datos en colecciones de objetos (`List<Pokemon>`).

### 3. Capa de Presentación (UI Windows Forms)
Maneja de forma exclusiva el diseño visual y los eventos de usuario, interactuando únicamente con la capa de negocio:
* `Form1.cs`: Ventana principal que lista los Pokémon, maneja los controles de selección y los filtros de búsqueda.
* `FormAltaPokemon.cs`: Ventana modal dedicada a las altas, modificaciones y validaciones de los datos antes de persistirlos.

---



##  Capturas de Pantalla

Aquí puedes ver la aplicación en funcionamiento:

### Pantalla Principal (Listado y Búsqueda)
![Pantalla Principal](https://github.com/Priscila-Rachel/Pokemon-manager-ado-net/raw/467495bcc9785b84bf71eb71e2c2d3d02ae47788/aplicacion%20.png)

### Dar de Alta un Nuevo Pokémon
![Alta de Pokémon](https://github.com/Priscila-Rachel/Pokemon-manager-ado-net/raw/467495bcc9785b84bf71eb71e2c2d3d02ae47788/dar%20de%20alta%20un%20pokemon%20(agregar%20un%20nuevo%20pokemon).png)

### Modificar Datos de un Pokémon
![Modificación de Pokémon](https://github.com/Priscila-Rachel/Pokemon-manager-ado-net/raw/467495bcc9785b84bf71eb71e2c2d3d02ae47788/modificar%20pokemon.png)



