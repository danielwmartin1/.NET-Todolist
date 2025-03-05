# Todo List Application

This is a simple Todo List application built using ASP.NET MVC. The application allows users to manage their todo items with functionalities to create, read, update, and delete (CRUD) tasks.

## Project Structure

- **Controllers**
  - `TodoController.cs`: Handles HTTP requests related to todo items.
  
- **Models**
  - `TodoItem.cs`: Represents a todo item with properties such as Id, Title, Description, IsCompleted, and DueDate.
  
- **Views**
  - **Todo**
    - `Index.cshtml`: Displays the list of todo items.
    - `Create.cshtml`: Provides a form for creating a new todo item.
    - `Edit.cshtml`: Provides a form for editing an existing todo item.
    - `Delete.cshtml`: Confirms the deletion of a todo item.
  
- **wwwroot**
  - **css**: Contains CSS files for styling the application.
  - **js**: Contains JavaScript files for client-side functionality.
  - **lib**: Contains third-party libraries, such as jQuery or Bootstrap.
  
- **Configuration Files**
  - `appsettings.json`: Contains configuration settings for the application.
  - `Program.cs`: The entry point of the application.
  - `Startup.cs`: Configures services and the application's request pipeline.

## Features

- Create new todo items.
- View a list of all todo items.
- Edit existing todo items.
- Delete todo items.

## Getting Started

1. Clone the repository.
2. Navigate to the project directory.
3. Restore the dependencies.
4. Run the application.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for any suggestions or improvements.