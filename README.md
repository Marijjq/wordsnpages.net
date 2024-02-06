# WordsnPages

WordsnPages is a simple e-commerce bookstore web application developed using the .NET MVC framework. The application allows users to browse and purchase books online.

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/Marijjq/wordsnpages.net.git
    ```

2. Navigate to the project directory:
    ```bash
    cd wordsnpages
    ```

3. Restore dependencies:
    ```bash
    dotnet restore
    ```

4. Run the application:
    ```bash
    dotnet run
    ```

The application will be accessible at http://localhost:7079.

## Configuration

### SQL Server:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server={yourserver};Database={yourdatabasename};Trusted_Connection=True;TrustServerCertificate=True;"
}

 ## PostgreSQL:
 "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres!;"
}
In Program.cs:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(
                        configuration.GetConnectionString("DefaultConnection")));

Open your web browser and navigate to http://localhost:7079 (or the specified port).

On the first run, the application creates an admin account. You can find the details in Dbinitializer.

Browse books, add them to the cart, and complete the checkout process.

For support or inquiries, please contact Marija at ms30059@seeu.edu.mk.
Contributing
If you would like to contribute to the project, follow these steps:

Fork the repository.

Create a new branch for your feature or bug fix.

Make changes and submit a pull request.

License
This project is licensed under the MIT License.

