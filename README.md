**Architectural Overview**
We followed Clean Architecture principles, splitting the system into four major projects/layers:

**1. API Layer**
Contains: Controllers that expose HTTP endpoints.

Depends on: Application Layer.

Responsibility: Maps HTTP requests to service calls, returns proper HTTP responses.

**2. Application Layer**
Contains: Business logic, use cases, service interfaces.

Depends on: Domain Layer (for models and contracts).

Responsibility: Orchestrates behavior by calling domain/repository services.

**3. Domain Layer**
Contains: Entities (Recipe), interfaces (IRecipeRepository).

Independent: No external dependencies.

Responsibility: Defines the core model and behavior.

**4. Infrastructure Layer (Persistence)**
Contains: Implementations of repositories (e.g., RecipeRepository), DbContext.

Depends on: Domain Layer and EF Core.

Responsibility: Handles data access to the underlying database.

**5. Frontend (React)**
Separate Vite React App: Fetches recipes from the backend via HTTP.

Responsibility: User interface for interacting with recipes.

**Trade-offs, Pros & Cons**
**Pros**
**Feature**	                                  **Benefit**
Clean separation of concerns	            Each layer has a focused responsibility. Easy to test and maintain.
Domain-centric design	                    Core business logic is isolated and technology-agnostic.
Easily extensible	                        New features can be added with minimal cross-layer impact.
Mockable layers	                         Infrastructure and database can be mocked for unit testing.

**Cons / Trade-offs**
**Issue**	                                    **Trade-off**
No Identity/Auth implemented	            We skipped JWT-based authentication for simplicity/time, leaving it vulnerable.
Lack of authorization control	            Any user can hit endpoints; no role-based or permission validation.
Added complexity from layering	          Overkill for small apps, adds verbosity and learning curve.
Initial dev cost	                        More code and boilerplate upfront compared to a simple monolith.

**Security & Monitoring Considerations**
**Aspect**	                                      **Notes**
Authentication	                           Not implemented — no JWT or token-based auth. Vulnerable to open access.
Fix:	                                     Integrate JWT via Identity Server, Firebase Auth, or ASP.NET Identity.
Monitoring	                               Not configured — no logging, tracing, or telemetry setup.
Fix:	                                     Add Serilog, Seq, Application Insights, or ELK stack.
Data Validation	                           Done via models and controller-level validation.
CORS	                                     Not explicitly configured — ensure only trusted frontends can call the API. Accepting all CORS for simplicity of dev environment however IPs should either be whitelisted and requests coming from the same netwrok in prod

**Cost Implications**
**Component**	                                      **Cost Trade-off**
Clean Architecture	                      More upfront dev time; pays off long-term for large codebases.
No Auth	                                  Security risk → could lead to data leaks. Minimal effort to mitigate via JWT.
No Monitoring                           	Harder to troubleshoot; may lead to higher maintenance cost.
Test Coverage	                            Added effort but improves confidence and reduces regression risks.

**Summary**
We used Clean Architecture with a proper layering strategy.
Unit tests cover repositories and services.
The frontend is being developed using modern React (Vite).
We deliberately omitted authentication and monitoring for time and scope constraints.
However, that decision leaves our API exposed and should be addressed before production.

![image](https://github.com/user-attachments/assets/c9998a58-9c2d-425c-ac1e-d80605049f6f)

**Stopwatch Result**
![image](https://github.com/user-attachments/assets/f36b5969-e2b7-4ec3-bc21-7dc9acee95cf)

