# RaceDay – Event Management System

## 1. System Description

RaceDay is a web-based event management system designed for the South African road running, walking and cycling community. It provides a centralised platform for event organisers to create and manage events, categories, routes and race results. Participants can browse events, enrol in event categories and view their race performance.

The system manages event information, participant enrolments, results and supporting information such as route and weather data.

## 2. User Roles

### Event Organiser
Event Organisers can create and manage their own events, manage event categories and routes, view participant enrolments and record race results.

### Participant
Participants can browse events, enrol in event categories and view their own race results.

## 3. Database

The RaceDay database includes the following main entities:

- RaceDayUsers
- Events
- Categories
- Enrolments
- Results
- Routes
- Weather

The database uses relationships between these entities to connect events with categories, enrolments, routes, weather information and results.

The Entity Relationship Diagram (ERD) and API Endpoint Plan are available in the `docs/` directory.

## 4. Repository Structure

- `docs/` – Entity Relationship Diagram and API Endpoint Plan.
- `SQL_Queries/` – SQL scripts for database creation, table setup and sample data.
- `.github/workflows/` – GitHub Actions workflow files.
- `README.md` – Project overview and setup instructions.

The Part 2 API and test projects should also be documented here once their final repository locations are confirmed.

## 5. Database Setup (Part 1)

1. Install and open Microsoft SQL Server and SQL Server Management Studio (SSMS).
2. Connect to your SQL Server instance.
3. Open the scripts in the `SQL_Queries/` directory.
4. Execute the database creation and table creation scripts in the correct order.
5. Execute the remaining scripts in the intended order to insert sample data.
6. Verify that the tables, relationships and sample records were created successfully.

**Important:** Run the scripts in their intended order and confirm the database name and SQL Server instance before executing them. Do not rerun creation scripts against an existing database unless they are designed to handle that situation.

## 6. Development and Version Control

The project is maintained in GitHub. The repository contains the project documentation, SQL scripts and GitHub Actions workflow.

GitHub Actions is used to automate project checks. The workflow status should be checked on the repository's **Actions** tab after pushing changes.

## 7. Continuous Integration Evidence

Add a screenshot showing the successful GitHub Actions workflow run.

**Screenshot:** `[Insert your actual green GitHub Actions screenshot here]`

## 8. Demonstration Video

The demonstration video presents the RaceDay project and its implementation.

**YouTube video:** `[Insert your actual YouTube video URL here]`

## 9. Project Documentation

- **Entity Relationship Diagram:** `docs/`
- **API Endpoint Plan:** `docs/API_Endpoint_Plan.md`
- **Database scripts:** `SQL_Queries/`


- Results
- Routes
- Weather

The database structure and relationships are documented in the ERD and SQL scripts provided in this repository.

