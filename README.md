# toggle-api
Hi! This is an API for managing toggles (simple name/value stores) that supports global and service-specific toggles. 
It uses .NET/ASP.NET Core, Entity Framework Core, xUnit, Fluent Assertions and OpenCover/ReportGenerator.

## Dependencies

* .NET Core 2.0 SDK
* Visual Studio 2017 15.3+ (optional)
* SQL Server LocalDB

## Running the application

* Just `dotnet run` on src\Toggle.Api
* It will automatically create and seed the database with 2 services (ids `1` and `2`) and some toggles

## Running tests

* `dotnet test` on test\Toggle.Domain.Test
* For coverage report, run `cover.bat` on the same folder

# API resources

#### GET /api/toggles/:serviceId/:version
Returns all toggles for the service and version specified. 
It merges the global toggles with the specific ones for the service, so if there's a global toggle named `isButtonRed` with value `true`
and a service-specific one with the same name but with value `false`, it will return `false`. 
Compare the results of <code>/api/toggles<b>/1/</b>1.0</code> and <code>/api/toggles<b>/2/</b>1.0</code> for reference.

#### GET /api/toggles/:id
Returns specified toggle, be it global or service-specific. If nonexistent, returns 404.

#### POST /api/toggle/:serviceId/:version
Body: `{ "name": "name", "value": "value" }`

Creates toggle for the service and version specified. Returns 404 if service doesn't exist.

#### PUT /api/toggle/:id
Body: `{ "name": "new name", "value": "new value" }`

Updates specified toggle. Returns 404 if toggle doesn't exist.

#### DELETE /api/toggle/:id

Deletes specified toggle. Returns 404 if toggle doesn't exist.
