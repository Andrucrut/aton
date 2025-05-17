# User Management API

REST API для управления пользователями: регистрация, авторизация, обновление профиля, удаление и восстановление. Реализовано на ASP.NET Core.

## 🚀 Требования

Перед запуском убедитесь, что у вас установлено:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [PostgreSQL](https://www.postgresql.org/download/) или другая БД (если используется)
- (опционально) [Visual Studio](https://visualstudio.microsoft.com/) / [Rider](https://www.jetbrains.com/rider/) / VS Code

## ⚙️ Запуск проекта
cd /Users/andrey/RiderProjects/WebApplication1/WebApplication1/
dotnet run

Стуктура проекта
WebApplication1/
├── Controllers/
│   └── UserController.cs
├── DTOs/
│   ├── UserCreateDto.cs
│   ├── UserLoginDto.cs
│   ├── UserLoginChangeDto.cs
│   ├── UserPasswordChangeDto.cs
│   ├── UserUpdateDto.cs
│   ├── UserAgeFilterDto.cs
│   └── UserViewDto.cs
├── Interfaces/
│   └── IUserService.cs
├── Services/
│   └── UserService.cs
├── Models/
│   └── User.cs
├── Program.cs
└── appsettings.json

Запросы
POST /api/Users - Создание пользователя
PUT /api/Users/{login} - Изменение имени, пола или даты рождения
PUT /api/Users/{login}/password - Изменение пароля пользователя
PUT /api/Users/{login}/login - Изменение логина пользователя (логин должен оставаться уникальным)
GET /api/Users/active - Запрос списка всех активных пользователей (отсутствует RevokedOn), отсортированных по CreatedOn
GET /api/Users/{login} - Запрос информации о пользователе по логину (возвращает имя, пол, дату рождения и статус активности)
POST /api/Users/login - Запрос пользователя по логину и паролю (только для самого пользователя, если он активен)
GET /api/Users/older-than/{age} - Запрос всех пользователей старше определённого возраста
DELETE /api/Users/{login}/soft - Мягкое удаление пользователя (устанавливает RevokedOn и RevokedBy)
DELETE /api/Users/{login}/hard - Полное удаление пользователя
POST /api/Users/{login}/restore - Восстановление пользователя (очищает поля RevokedOn и RevokedBy)
