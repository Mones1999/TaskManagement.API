# TaskManagement
This project was created with .NET 6.0

# Installation
- Make sure you have `.NET 6.0 SDK` installed on your system.
- Install `MySQL` on your system.
- Clone the project using the command: `git clone https://github.com/Mones1999/TaskManagement.API.git`

# Database Setup
- I used **Code-First Approach** in this project.
- Edit the connection string in file `TaskManagement.API/appsettings.json` Replace the `DefaultConnection` with your connection string.
- Using package manager console write this two commands **Make Sure The Default Project Is `TaskManagement.Infrastructure`** :
  - `add-migration AddTables`
  - `update-database`
 
# Note: 
- This application URL is always `http://localhost:5000`
