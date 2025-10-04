## Caso: Biblioteca Digital

La biblioteca municipal quiere modernizarse. Actualmente prestan libros en una libreta de apuntes y hojas de Excel, lo que genera problemas como:

- Libros prestados a varios usuarios al mismo tiempo.
- No hay control sobre qué usuario tiene un libro y hasta cuándo.
- Dificultad para consultar el historial de préstamos de un usuario.
  
### Objetivo:
  Desarrollar una aplicación en C# con MVC + EF Core que permita gestionar usuarios, libros y préstamos de manera digital.

### Requisitos

#### 1. Gestión de Usuarios

- Registrar un nuevo usuario con: Nombre, Documento, Correo, Teléfono.
- Validar que el documento sea único (no se pueden duplicar usuarios).
- Listar todos los usuarios registrados.

#### 2. Gestión de Libros

- Registrar libros con: Título, Autor, codigo, EjemplaresDisponibles.
- Validar que el codigo sea único.
- Listar todos los libros registrados.

#### 3. Gestión de Préstamos

- Un préstamo debe asociar un usuario, un libro y una fecha de devolución.
- Validar que un libro no se pueda prestar si no tiene ejemplares disponibles.
- Al prestar un libro, se debe restar 1 ejemplar del stock.
- Al devolver un libro, se debe sumar 1 ejemplar al stock.
- Consultar los préstamos de un usuario específico (historial).
- Consultar los préstamos de un libro específico (qué usuarios lo tienen actualmente).

#### 4. Persistencia y Validaciones

- Implementar el sistema con EF Core conectado a MySQL.
- Usar List<> y LINQ en controladores para filtrar (ej: préstamos por usuario).
- Validar en tiempo de ejecución (try-catch) errores como:
- Intentar prestar un libro sin ejemplares.
- Registrar usuarios o libros duplicados.
