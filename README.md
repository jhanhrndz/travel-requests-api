


# 🚀 Travel Requests API

API para gestionar solicitudes de viajes corporativos desarrollada con .NET 8.

## ✅ Funcionalidades

- Registro e inicio de sesión con JWT
- Creación y consulta de solicitudes de viaje
- Aprobación o rechazo de solicitudes (rol "Aprobador")
- Recuperación y restablecimiento de contraseña

## ⚙️ Tecnologías

- .NET 8 + Entity Framework Core
- SQL Server (LocalBD)
- JWT para autenticación
- Swagger para pruebas interactivas

2. **Restaura dependencias**

   ```bash
   dotnet restore
   ```

3. **Configura la base de datos en `appsettings.json`**

   Ejemplo:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TravelRequestsDb;Trusted_Connection=true;"
   }
   ```

4. **Ejecuta la aplicación**

   ```bash
   dotnet run
   ```

5. **Accede a Swagger**
   [http://localhost:-PUERTO-/swagger]

## 🔐 Autenticación con JWT

1. Regístrarse O iniciar sesion en `/api/auth`
2. Copia el token recibido
3. Haz clic en `Authorize` (ícono de candado en Swagger)
4. Pega el token así:

   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...
   ```

> [!IMPORTANT]
>  Se debe de colocar _"Baerer"_ antes del token.

## ✈️ Endpoints Principales

| Método | Ruta                             | Descripción                            | Requiere Token |
| ------ | -------------------------------- | -------------------------------------- | -------------- |
| POST   | `/api/auth/register`             | Registrar usuario                      | NO              |
| POST   | `/api/auth/login`                | Iniciar sesión                         | NO              |
| POST   | `/api/auth/forgot-password`      | Enviar código de recuperación          | NO              |
| POST   | `/api/auth/reset-password`       | Cambiar contraseña con código          | NO              |
| GET    | `/api/auth/users`                | Listar usuarios (solo Aprobador)       | SI              |
| GET    | `/api/travelrequest/my-requests` | Ver mis solicitudes                    | SI              |
| POST   | `/api/travelrequest`             | Crear solicitud de viaje               | SI              |
| GET    | `/api/travelrequest/all`         | Ver todas (solo "Aprobador")           | SI              |
| PUT    | `/api/travelrequest/{id}/status` | Aprobar/Rechazar solicitud (Aprobador) | SI              |

## 👥 Roles de Usuario

* `Solicitante`: puede crear y ver sus solicitudes
* `Aprobador`: puede ver todas las solicitudes y cambiar su estado

## 🧪 Pruebas

1. **Crear un usuario Aprobador**

   ```json
   {
     "name": "Admin",
     "email": "admin@example.com",
     "password": "admin123",
     "role": "Aprobador"
   }
   ```

2. **Crear un usuario Solicitante**

   ```json
   {
     "name": "Empleado",
     "email": "empleado@example.com",
     "password": "empleado123",
     "role": "Solicitante"
   }
   ```

3. **Finalmente, se puede probar los endpoints con el token recibido**





